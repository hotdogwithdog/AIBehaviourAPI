using System;
using System.Collections.Generic;

namespace AIBehaviourAPI.Bt.Decorators
{
    public abstract class ADecoratorNode : ANodeBT
    {
        public ANodeBT Child => ChildNodes.Count > 0 ? ChildNodes[0] : null;
        
        public ADecoratorNode(string name, ANodeBT child) : base(name)
        {
            _outputNodes.Capacity = 1;
            _outputNodes.Add(child);
            _condition = NodeAction;
            
            child.SetParent(this);
        }

        protected abstract bool NodeAction();
    }
}