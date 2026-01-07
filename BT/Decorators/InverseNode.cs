using System;

namespace AIBehaviourAPI.Bt.Decorators
{
    public class InverseNode : ADecoratorNode
    {
        public InverseNode(string name, ANodeBT child) : base(name, child) { }

        protected override bool NodeAction()
        {
            return !Child.Condition();
        }
    }
}