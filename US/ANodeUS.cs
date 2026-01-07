using System;
using System.Collections.Generic;
using System.Text;

namespace AIBehaviourAPI.US
{
    public abstract class ANodeUS : INode
    {
        protected Action _action;
        protected Func<bool> _condition;
        protected List<ANodeUS> _inputNodes;
        protected List<ANodeUS> _outputNodes;
        protected string _name;

        protected float _cachedUtilityValue;
        protected bool _isCachedUtilityValueValid;
        
        public Action Action => _action;
        public Func<bool> Condition => _condition;
        public List<INode> InputNodes => _inputNodes.ConvertAll<INode>(node => node as INode);
        public List<INode> OutputNodes => _outputNodes.ConvertAll<INode>(node => node as INode);
        public string Name => _name;

        public abstract float GetUtilityValue();
        
        public ANodeUS(string name)
        {
            _name = name;
            _isCachedUtilityValueValid = false;
            
            _inputNodes = new List<ANodeUS>();
            _outputNodes = new List<ANodeUS>();
        }
        
        public void ClearCacheValue()
        {
            _isCachedUtilityValueValid = false;
        }
        
        public override string ToString()
        {
            return $"N: {_name}; CUV: {(_isCachedUtilityValueValid? _cachedUtilityValue : "Not valid")} IN: [{NodesListToString(InputNodes)}]; ON: [{NodesListToString(OutputNodes)}]";
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