using System;
using System.Collections.Generic;

namespace AIBehaviourAPI.US
{
    public enum USMode
    {
        None = 0,
        Min,    // execute the action with the lowest utility value, in case of more than one just the first will be executed (based on the order of registration of the actions)
        Max,    // execute the action with the highest utility value, in case of more than one just the first will be executed (based on the order of registration of the actions)
        Custom  // Custom logic to be executed must set 
    }
    
    
    public class UtilitySystem : IBehaviourGraph
    {
        protected Action<INode, INode> _onCurrentNodeDoesTransition;
        protected Status _status;
        protected string _name;
        protected List<ANodeUS> _nodes;

        protected List<UtilityAction> _actionNodes;
        protected List<float> _utilityValues;
        protected USMode _mode;
        protected Func<List<float>, int[]> _customFunction;
        
        /// <summary>
        /// In this behaviour engine this is not used
        /// </summary>
        public Action<INode, INode> OnCurrentNodeDoesTransition 
        {
            get { return _onCurrentNodeDoesTransition; }
            set { _onCurrentNodeDoesTransition = value; }
        }
        public Status Status => _status;
        public INode CurrentNode => null;
        public bool MustRestartToInitialNode => false;
        public INode InitialNode => null;
        public string Name => _name;
        public List<INode> Nodes => _nodes.ConvertAll<INode>(node => node as INode);

        public List<UtilityAction> ActionNodes => _actionNodes;
        public List<float> UtilityValues => _utilityValues;
        public USMode Mode => _mode;

        
        /// <summary>
        /// Create a Utility System
        /// </summary>
        /// <param name="name"> The name of the utility system </param>
        /// <param name="mode"> The for select the action if set to custom you must call before any update call to the US to SetCustomFunction method for give the custom mode </param>
        /// <param name="numberOfNodes"> number of nodes(including all types of nodes) just for have it the memory allocations do it at once </param>
        /// <param name="numberOfActionNodes"> number of action nodes just for have it the memory allocations do it at once </param>
        public UtilitySystem(string name, USMode mode, int numberOfNodes = 0, int numberOfActionNodes = 0)
        {
            _name = name;
            _nodes = new List<ANodeUS>(numberOfNodes);
            _actionNodes = new List<UtilityAction>(numberOfActionNodes);
            _utilityValues = new List<float>(numberOfActionNodes);

            _mode = mode;
            _status = Status.None;
        }

        /// <summary>
        /// Set the custom function for select the action based on the utility values
        /// </summary>
        /// <param name="customFunction"> The function to be set the parameter is a list with the utility values
        /// and the function must return the indices of the utility values that wants to execute the associate actions </param>
        public void SetCustomFunction(Func<List<float>, int[]> customFunction)
        {
            _customFunction = customFunction;
        }
        
        /// <summary>
        /// In this behaviour engine the Initial node is ignored just pass it null
        /// </summary>
        /// <param name="initialNode"></param>
        public void Init(INode initialNode = null)
        {
            if (_status != Status.None)
                throw new BehaviourAPIException($"Cannot initialize US: {_name} after it has been initialized.");

            _status = Status.Running;
        }

        public void Update()
        {
            if (_status != Status.Running)
                throw new BehaviourAPIException($"Cannot update US: {_name} if is not in Running state.");

            foreach (ANodeUS node in _nodes)
            {
                node.ClearCacheValue();
            }

            if (_actionNodes.Count != _utilityValues.Count)
                throw new BehaviourAPIException($"The size of the actions missmatch the size of the utility values US: {_name}; size of actions: {_actionNodes.Count}; size of  utility values: {_utilityValues.Count}.");
            
            for (int i = 0; i < _actionNodes.Count; ++i)
            {
                _utilityValues[i] = _actionNodes[i].GetUtilityValue();
            }

            switch (_mode)
            {
                case USMode.Min:
                    MinModeLogic();
                    return;
                case USMode.Max:
                    MaxModeLogic();
                    return;
                case USMode.Custom:
                    int[] indices = _customFunction?.Invoke(_utilityValues);
                    foreach (int i in indices)
                    {
                        _actionNodes[i].Action();
                    }
                    return;
                default:
                    throw new BehaviourAPIException($"The USMode is unexpected: {_mode} in the US: {_name}.");
            }
            
        }
        
        private void MinModeLogic()
        {
            float min = float.MaxValue;
            int index = -1;
            for (int i = 0; i < _utilityValues.Count; ++i)
            {
                if (_utilityValues[i] < min)
                {
                    min = _utilityValues[i];
                    index = i;
                }
            }

            if (index != -1) _actionNodes[index].Action();
        }

        private void MaxModeLogic()
        {
            float max = float.MinValue;
            int index = -1;
            for (int i = 0; i < _utilityValues.Count; ++i)
            {
                if (_utilityValues[i] > max)
                {
                    max = _utilityValues[i];
                    index = i;
                }
            }

            if (index != -1) _actionNodes[index].Action();
        }


        public void Finish()
        {
            if (_status == Status.None && _status == Status.Finished)
                throw new BehaviourAPIException($"Cannot uninitialize US: {_name} after it has been uninitialized or before he was been initialized.");

            _status = Status.Finished;
        }

        public void Pause()
        {
            if (_status != Status.Running)
                throw new BehaviourAPIException($"Cannot pause US: {_name} if is not Running.");
            
            _status = Status.Paused;
        }

        public void Resume()
        {
            if (_status != Status.Paused)
                throw new BehaviourAPIException($"Cannot resume US: {_name} if is not Paused.");
            
            _status = Status.Running;
        }
        
        /// <summary>
        /// Register a node must be called to have all the nodes of the US, except the actions that must use RegisterAction
        /// </summary>
        /// <param name="node"> The node to register </param>
        public void RegisterNode(INode node)
        {
            ANodeUS nodeUS = node as ANodeUS;
            if (nodeUS == null)
                throw new BehaviourAPIException($"The node must be an ANodeUS for added to the US: {_name}; node: {node.Name}");
            if (_nodes.Contains(nodeUS))
                throw new BehaviourAPIException($"Cannot register one node more than one time in US: {_name}; node: {node.Name}.");
            
            _nodes.Add(nodeUS);
        }

        /// <summary>
        /// Register the action in both ways as a node and as an action
        /// </summary>
        /// <param name="action"> The UtilityAction to register </param>
        public void RegisterAction(UtilityAction action)
        {
            if (_nodes.Contains(action))
                throw new BehaviourAPIException($"Cannot register one node more than one time in US: {_name}; action node: {action.Name}.");
            if (_actionNodes.Contains(action))
                throw new BehaviourAPIException($"Cannot register one action node more than one time in US: {_name}: action node: {action.Name}.");
           
            
            _actionNodes.Add(action);
            _utilityValues.Add(-1.0f);
            _nodes.Add(action);
        }

        public bool IsNodeInGraph(INode node)
        {
            return _nodes.Contains(node as ANodeUS);
        }
    }
}