using System;
using System.Collections.Generic;
using System.Text;

namespace AIBehaviourAPI.Fsm
{
    public class NodeFSM : INode
    {
        protected Action _action;
        protected Func<bool> _condition;
        protected List<INode> _inputNodes;
        protected List<INode> _outputNodes;
        protected string _name;
        
        public Action Action => _action;
        public Func<bool> Condition => _condition;
        public List<INode> InputNodes => _inputNodes;
        public List<INode> OutputNodes =>  _outputNodes;
        public string Name => _name;
        public NodeFSM(string name, Action action = null)
        {
            _condition = null;
            _action = action;
            _name = name;
            _inputNodes = new List<INode>();
            _outputNodes = new List<INode>();
        }
        public override string ToString()
        {
            return $"N: {_name} IT: [{NodesListToString(_inputNodes)}]; OT: [{NodesListToString(_outputNodes)}]";
        }

        private string NodesListToString(List<INode> nodes)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < nodes.Count - 1; ++i)
            {
                builder.Append($"{nodes[i].Name}, ");
            }
            builder.Append($"{nodes[nodes.Count - 1].Name}");
            return builder.ToString();
        }
    }
}