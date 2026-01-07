using System;
using System.Collections.Generic;

namespace AIBehaviourAPI.Fsm
{
    public class NodeHFSM : NodeFSM
    {
        private IBehaviourEngine _engine;
        private IBehaviourEngine _internalEngine;
        
        /// <summary>
        /// Create a NodeFSM that count like the big node that inside have other FSM
        /// </summary>
        /// <param name="name"> Name of the node </param>
        /// <param name="engine"> The FSM that this node is part of it </param>
        /// <param name="internalEngine"> Engine that have inside, must be initialized and Paused </param>
        public NodeHFSM(string name, FSM engine, IBehaviourEngine internalEngine) : base(name)
        {
            _internalEngine = internalEngine;
            _action = () => _internalEngine.Update();
            _engine = engine;
            _engine.OnCurrentNodeDoesTransition += OnNodesChange;
        }

        private void OnNodesChange(INode lastNode, INode currentNode)
        {
            if (lastNode == currentNode) return;
            
            if (this == lastNode)
            {
                _internalEngine.Pause();
            }
            else if (this == currentNode)
            {
                _internalEngine.Resume();
            }
        }
    }
}