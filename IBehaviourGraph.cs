using System.Collections.Generic;

namespace AIBehaviourAPI
{
    public interface IBehaviourGraph : IBehaviourEngine
    {
        public void RegisterNode(INode node);
        
        public List<INode> Nodes { get; }
        
        public bool IsNodeInGraph(INode node);
    }
}