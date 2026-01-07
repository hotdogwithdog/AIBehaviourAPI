using System;
using System.Collections.Generic;

namespace AIBehaviourAPI.Fsm
{
    public class FSM : IBehaviourGraph
    {
        #region --------------------------- Properties And Variables ---------------------------
        protected Action<INode, INode> _onCurrentNodeDoesTransition;
        protected Status _status;
        protected INode _initialNode;
        protected INode _currentNode;
        protected bool _mustRestartToInitialNode;
        protected string _name;
        
        protected List<INode> _nodes;
        protected List<ITransition> _transitions;

        public Action<INode, INode> OnCurrentNodeDoesTransition
        {
            get { return _onCurrentNodeDoesTransition; }
            set { _onCurrentNodeDoesTransition = value; }
        }
        public Status Status => _status;
        public INode InitialNode => _initialNode;

        public INode CurrentNode => _currentNode;
        public bool MustRestartToInitialNode => _mustRestartToInitialNode;
        public string Name => _name;
        
        public List<INode> Nodes => _nodes;
        public List<ITransition> Transitions => _transitions;
        #endregion
        
        /// <summary>
        /// Contructor of an FSM
        /// </summary>
        /// <param name="name"> The name of the FSM not recommend to have the same name on it </param>
        /// <param name="mustRestartToInitialNode"> True if the graph must restart if his execution is cut false in case not (Default = true) (used just for submachines)</param>
        /// <param name="numberOfNodes"> Number of nodes that will be allocated at start (just for reduce the memory allocations) (Default = 0)</param>
        /// <param name="numberOfTransitions"> Number of transitions that will be allocated at start (just for reduce the memory allocations) (Default = 0)</param>
        public FSM(string name, bool mustRestartToInitialNode = true, int numberOfNodes = 0, int numberOfTransitions = 0)
        {
            _name = name;
            _mustRestartToInitialNode = mustRestartToInitialNode;
            
            _nodes = new List<INode>(numberOfNodes);
            _transitions = new List<ITransition>(numberOfTransitions);
            
            _status = Status.None;
            _currentNode = null;
        }
        
        #region --------------------------- Control Methods ---------------------------
        public void Init(INode initialNode)
        {
            if (_status != Status.None)
                throw new BehaviourAPIException($"Cannot initialize FSM: {_name} after it has been initialized.");
            if (!_nodes.Contains(initialNode))
                throw new BehaviourAPIException($"The initialNode is not register in the FSM: {_name}.");

            _status = Status.Running;
            _initialNode = initialNode;
            _onCurrentNodeDoesTransition?.Invoke(_currentNode, initialNode);
            _currentNode = initialNode;
        }

        /// <summary>
        /// Update the FSM the first transition that is success will be called
        /// </summary>
        /// <exception cref="BehaviourAPIException"></exception>
        public void Update()
        {
            if (_status != Status.Running)
                throw new BehaviourAPIException($"Cannot update FSM: {_name} if is not in Running state.");

            for (int i = 0; i < _currentNode.OutputNodes.Count; ++i)
            {
                if (_currentNode.OutputNodes[i].Condition())
                {
                    _currentNode.OutputNodes[i].Action?.Invoke();
                    _onCurrentNodeDoesTransition?.Invoke(_currentNode, _currentNode.OutputNodes[i].OutputNodes[0]); // A little tricky that this nodes are transitions but instead of cast to ITransition just take the first that is like the Transitions Store that access
                    _currentNode = _currentNode.OutputNodes[i].OutputNodes[0]; // Same as line before, this node is in truth a transition for how this FSM class is build
                    break;
                }
            }
            _currentNode.Action?.Invoke();
        }

        public void Finish()
        {
            if (_status == Status.None && _status == Status.Finished)
                throw new BehaviourAPIException($"Cannot uninitialize FSM: {_name} after it has been uninitialized or before he was been initialized.");

            _status = Status.Finished;
            _currentNode = null;
        }
        public void Pause()
        {
            if (_status != Status.Running)
                throw new BehaviourAPIException($"Cannot pause FSM: {_name} if is not Running.");
            
            _status = Status.Paused;
        }

        public void Resume()
        {
            if (_status != Status.Paused)
                throw new BehaviourAPIException($"Cannot resume FSM: {_name} if is not Paused.");
            
            _status = Status.Running;
            
            if (!_mustRestartToInitialNode) return;
            
            _onCurrentNodeDoesTransition?.Invoke(_currentNode, _initialNode);
            _currentNode = _initialNode;
        }
        #endregion

        #region --------------------------- Creation Methods ---------------------------
        public void RegisterNode(INode node)
        {
            if (_nodes.Contains(node))
                throw new BehaviourAPIException($"Cannot register one node more than one time in FSM: {_name}; node: {node.Name}.");
            
            _nodes.Add(node);
        }
        public void RegisterTransition(ITransition transition)
        {
            if (_transitions.Contains(transition))
                throw new BehaviourAPIException($"Cannot register transition more than one time in FSM: {_name}; transition: {transition.Name}.");
            if (transition.OriginNode == null && transition.DestinationNode == null)
                throw new BehaviourAPIException($"Cannot register a transition with nodes that are null in FSM: {_name}; transition: {transition.Name}.");
            
            transition.OriginNode.OutputNodes.Add(transition);
            transition.DestinationNode.InputNodes.Add(transition);
            _transitions.Add(transition);
        }
        #endregion

        public bool IsNodeInGraph(INode node)
        {
            return _nodes.Contains(node);
        }
        public bool IsTransitionInGraph(ITransition transition)
        {
            return _transitions.Contains(transition);
        }
    }
}