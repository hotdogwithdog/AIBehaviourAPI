using System;
using System.Collections.Generic;

namespace AIBehaviourAPI.Fsm
{
    public class TransitionFSM : ITransition
    {
        protected List<INode> _originNode;
        protected List<INode> _destinationNode;
        protected Action _action;
        protected Func<bool> _condition;
        protected string _name;
        
        public INode OriginNode => _originNode.Count > 0? _originNode[0] : null;
        public INode DestinationNode => _destinationNode.Count > 0? _destinationNode[0] : null;
        public Action Action => _action;
        public Func<bool> Condition => _condition;
        public List<INode> InputNodes => _originNode;
        public List<INode> OutputNodes => _destinationNode;
        public string Name => _name;
        
        public TransitionFSM(INode originNode, INode destinationNode, string name, Func<bool> condition, Action action = null)
        {
            _originNode = new List<INode>();
            _destinationNode =  new List<INode>();
            _originNode.Add(originNode);
            _destinationNode.Add(destinationNode);
            
            _name = name;
            _condition = condition;
            _action = action;
        }

        public override string ToString()
        {
            return $"O: {(_originNode.Count > 0? _originNode[0].Name : "None")} -> N: {_name} -> D: {(_destinationNode.Count > 0? _destinationNode[0].Name : "None")}";
        }
    }
}