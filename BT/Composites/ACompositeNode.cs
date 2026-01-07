using System;
using System.Collections.Generic;
using System.Xml;

namespace AIBehaviourAPI.Bt.Composites
{
    public abstract class ACompositeNode : ANodeBT
    {
        public ACompositeNode(string name, List<ANodeBT> childNodes) : base(name)
        {
            _outputNodes = childNodes;
            _condition = NodeAction;

            foreach (ANodeBT childNode in childNodes)
            {
                childNode.SetParent(this);
            }
        }

        protected abstract bool NodeAction();
    }
}