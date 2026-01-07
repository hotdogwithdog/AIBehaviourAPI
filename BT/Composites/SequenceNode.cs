using System;
using System.Collections.Generic;
using System.Linq;

namespace AIBehaviourAPI.Bt.Composites
{
    public class SequenceNode : ACompositeNode
    {
        public SequenceNode(string name, ANodeBT[] childNodes) : base(name, childNodes.ToList()) { }

        protected override bool NodeAction()
        {
            foreach (ANodeBT child in ChildNodes)
            {
                if (!child.Condition()) return false;
            }
            return true;
        }
    }
}