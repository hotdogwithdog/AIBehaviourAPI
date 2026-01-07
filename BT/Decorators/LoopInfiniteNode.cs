using System;

namespace AIBehaviourAPI.Bt.Decorators
{
    public class LoopInfiniteNode : ADecoratorNode
    {
        public LoopInfiniteNode(string name, ANodeBT child) : base(name, child) { }

        protected override bool NodeAction()
        {
            Child.Condition();
            return true;
        }
    }
}