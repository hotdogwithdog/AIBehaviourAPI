
namespace AIBehaviourAPI
{
    public interface IBehaviourEngine
    {   
        public System.Action<INode, INode> OnCurrentNodeDoesTransition { get; set; }
        public Status Status { get; }
        public void Init(INode initialNode);

        public void Update();

        public void Finish();

        public void Pause();

        public void Resume();
        
        public INode CurrentNode { get; }
        
        public bool MustRestartToInitialNode { get; }
        
        public INode InitialNode { get; }
        
        public string Name { get; }
    }
}