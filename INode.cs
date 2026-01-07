using System.Collections.Generic;


namespace AIBehaviourAPI
{
    public interface INode
    {
        public System.Action Action { get; }
        
        public System.Func<bool> Condition { get; }
        
        public List<INode> InputNodes { get; }
        
        public List<INode> OutputNodes { get; }
        
        public string Name { get; }
    }
}