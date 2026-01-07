namespace AIBehaviourAPI
{
    public enum Status
    {
        None = 0,
        Running = 1,
        Paused = (1 << 1),
        Finished = (1 << 2),
    }
}