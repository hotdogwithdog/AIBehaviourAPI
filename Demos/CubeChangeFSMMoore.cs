
using AIBehaviourAPI.Fsm;
using UnityEngine;

namespace AIBehaviourAPI.Demos
{
    public class CubeChangeFSMMoore : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;

        private FSM _fsm;
        
        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();

            _fsm = new FSM("Cube Change", false, 3, 3);
            
            NodeFSM redNodeFsm = new NodeFSM("Red", ChangeRed);
            NodeFSM greenNodeFsm = new NodeFSM("Green", ChangeGreen);
            NodeFSM blueNodeFsm = new NodeFSM("Blue",  ChangeBlue);
            
            TransitionFSM redToGreen = new TransitionFSM(redNodeFsm, greenNodeFsm, "redToGreen", () => UnityEngine.Input.GetKey(KeyCode.G), () => Debug.Log("Red -> Green"));
            TransitionFSM greenToBlue = new TransitionFSM(greenNodeFsm, blueNodeFsm, "greenToBlue", () => UnityEngine.Input.GetKey(KeyCode.B), () => Debug.Log("Green -> Blue"));
            TransitionFSM blueToRed = new TransitionFSM(blueNodeFsm, redNodeFsm, "blueToRed", () => UnityEngine.Input.GetKey(KeyCode.R), () => Debug.Log("Blue -> Red"));
            
            _fsm.RegisterNode(redNodeFsm);
            _fsm.RegisterNode(greenNodeFsm);
            _fsm.RegisterNode(blueNodeFsm);
            _fsm.RegisterTransition(redToGreen);
            _fsm.RegisterTransition(greenToBlue);
            _fsm.RegisterTransition(blueToRed);
            
            _fsm.Init(redNodeFsm);
        }

        private void Update()
        {
            _fsm.Update();
        }

        #region Functions for agent
        private void ChangeRed()
        {
            Debug.Log("Change Red");
            _spriteRenderer.color = Color.red;
        }

        private void ChangeGreen()
        {
            Debug.Log("Change Green");
            _spriteRenderer.color = Color.green;
        }

        private void ChangeBlue()
        {
            Debug.Log("Change Blue");
            _spriteRenderer.color = Color.blue;
        }
        #endregion
    }
}

