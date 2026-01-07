using System;

namespace AIBehaviourAPI.Bt.Decorators
{
    public class LoopUntilFailNode : ADecoratorNode
    {
        public LoopUntilFailNode(string name, ANodeBT child) : base(name, child) { }

        protected override bool NodeAction()
        {
            return Child.Condition();
        }
    }
}