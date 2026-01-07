using System;
using System.Collections.Generic;
using System.Linq;

namespace AIBehaviourAPI.Bt.Composites
{
    public enum SelectorMode
    {
        None = 0,
        Order,
        Random
    }
    
    public class SelectorNode : ACompositeNode
    {
        private readonly SelectorMode _mode;

        public SelectorNode(string name, ANodeBT[] childNodes, SelectorMode mode = SelectorMode.Order) : base(name, childNodes.ToList())
        {
            _mode = mode;
        }
        
        protected override bool NodeAction()
        {
            switch (_mode)
            {
                case SelectorMode.Order:
                    return OrderExecute();
                case SelectorMode.Random:
                    return RandomExecute();
                default:
                    throw new BehaviourAPIException($"The Selector Mode of Node: {_name} is Invalid");
            }
        }

        private bool OrderExecute()
        {
            foreach (ANodeBT child in ChildNodes)
            {
                if (child.Condition()) return true;
            }
            return false;
        }

        private bool RandomExecute()
        {
            int[] indices = Enumerable.Range(0, ChildNodes.Count).ToArray();
            Random random = new Random();
            for (int i = indices.Length - 1; i >= 0; i--)
            {
                int j = random.Next(0, i);
                (indices[j], indices[i]) = (indices[i], indices[j]);
            }
            
            foreach (int i in indices)
            {
                if (ChildNodes[i].Condition()) return true;
            }
            return false;
        }
    }
}