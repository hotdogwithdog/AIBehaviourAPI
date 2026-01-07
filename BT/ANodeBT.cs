using System;
using System.Collections.Generic;
using System.Text;
using AIBehaviourAPI.Fsm;

namespace AIBehaviourAPI.Bt
{
    public abstract class ANodeBT : INode
    {
        protected Func<bool> _condition;
        protected List<ANodeBT> _inputNodes;
        protected List<ANodeBT> _outputNodes;
        protected string _name;
        
        public Action Action => null;
        public Func<bool> Condition => _condition;
        public List<INode> InputNodes =>  _inputNodes.ConvertAll<INode>(node => node as INode);
        public List<INode> OutputNodes =>  _outputNodes.ConvertAll<INode>(node => node as INode);
        public string Name =>  _name;
        
        public ANodeBT ParentNode => _inputNodes.Count > 0 ? _inputNodes[0] : null;
        public List<ANodeBT> ChildNodes => _outputNodes;

        protected ANodeBT(string name, Func<bool> condition = null)
        {
            _name = name;
            _condition = condition;
            _inputNodes = new List<ANodeBT>();
            _outputNodes = new List<ANodeBT>();
        }
        
        internal void SetParent(ANodeBT parent)
        {
            if (ParentNode != null)
                throw new BehaviourAPIException($"The BT Node: {_name} can have just one Parent Node");
            
            _inputNodes.Add(parent);
        }
        
        public override string ToString()
        {
            return $"N: {_name} ParentNode: [{(ParentNode != null? ParentNode.Name : "None")}]; Childs: [{NodesListToString(OutputNodes)}]";
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