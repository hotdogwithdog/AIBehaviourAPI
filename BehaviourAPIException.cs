using System;

namespace AIBehaviourAPI
{
    public class BehaviourAPIException : Exception
    {
        public BehaviourAPIException(string message) : base(message) { }
    }
}