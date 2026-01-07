using System;
using System.Collections.Generic;
using AIBehaviourAPI.Fsm;

namespace AIBehaviourAPI.Bt
{
    public class BT : IBehaviourGraph
    {
        protected Action<INode, INode> _onCurrentNodeDoesTransition;
        protected Status _status;
        protected INode _currentNode;
        protected ANodeBT _rootNode;
        protected string _name;
        protected List<INode> _nodes;
        
        public Action<INode, INode> OnCurrentNodeDoesTransition
        {
            get { return _onCurrentNodeDoesTransition; }
            set { _onCurrentNodeDoesTransition = value; }
        }
        public Status Status => _status;

        public INode CurrentNode => _currentNode;
        public bool MustRestartToInitialNode => true;
        public INode InitialNode => _rootNode;
        public string Name => _name;
        
        public List<INode> Nodes => _nodes;

        /// <summary>
        /// Creates a Behaviour Tree
        /// </summary>
        /// <param name="name"> Name of the Behaviour tree just for debug and exceptions info </param>
        /// <param name="numberOfNodes"> Number of nodes that will have the BehaviourTree is just for allocate all the memory at once, so it will work fine if not set </param>
        public BT(string name, int numberOfNodes = 0)
        {
            _name = name;
            _nodes = new List<INode>(numberOfNodes);
            
            _status = Status.None;
            _currentNode = null;
        }
        
        public void Init(INode initialNode)
        {
            if (_status != Status.None)
                throw new BehaviourAPIException($"Cannot initialize BT: {_name} after it has been initialized.");
            if (!_nodes.Contains(initialNode))
                throw new BehaviourAPIException($"The initialNode is not register in the BT: {_name}.");

            _status = Status.Running;
            _rootNode = initialNode as ANodeBT;
            _onCurrentNodeDoesTransition?.Invoke(_currentNode, initialNode);
            _currentNode = initialNode;
        }
        
        /// <summary>
        ///  Updates and do all the logic of the nodes until the nodes result of the _initialNode was reach again the result
        ///  If That result is true the BT is not finish (but will not execute nothing until the next update execution)
        ///  If is false the execution of the BT will finish
        /// </summary>
        public void Update()
        {
            if (_status != Status.Running)
                throw new BehaviourAPIException($"Cannot update BT: {_name} if is not in Running state.");
            
            if (_rootNode.Condition()) return;

            _status = Status.Finished;
        }

        public void Finish()
        {
            if (_status == Status.None && _status == Status.Finished)
                throw new BehaviourAPIException($"Cannot uninitialize BT: {_name} after it has been uninitialized or before he was been initialized.");

            _status = Status.Finished;
            _currentNode = null;
        }

        public void Pause()
        {
            if (_status != Status.Running)
                throw new BehaviourAPIException($"Cannot pause BT: {_name} if is not Running.");
            
            _status = Status.Paused;
        }

        public void Resume()
        {
            if (_status != Status.Paused)
                throw new BehaviourAPIException($"Cannot resume BT: {_name} if is not Paused.");
            
            _status = Status.Running;
            
            _onCurrentNodeDoesTransition?.Invoke(_currentNode, _rootNode);
            _currentNode = _rootNode;
        }

        
        public void RegisterNode(INode node)
        {
            if (_nodes.Contains(node))
                throw new BehaviourAPIException($"Cannot register one node more than one time in BT: {_name}; node: {node.Name}.");
            
            _nodes.Add(node);
        }

        
        public bool IsNodeInGraph(INode node)
        {
            return _nodes.Contains(node);
        }
    }
}