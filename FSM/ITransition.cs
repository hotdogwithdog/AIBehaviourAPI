namespace AIBehaviourAPI.Fsm
{
    public interface ITransition : INode
    {
        public INode OriginNode { get; }
        
        public INode DestinationNode { get; }
    }
}