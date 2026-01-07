using AIBehaviourAPI.Fsm;
using UnityEngine;

namespace AIBehaviourAPI.Demos
{
    public class HierarchicalFSM : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;

        private FSM _fsm;

        private float _timerTimeMax = 3.0f;
        private float _currentTime = 0.0f;
        
        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();

            _fsm = new FSM("Cube Change Hierarchical");
            
            NodeFSM redNodeFsm = new NodeFSM("Red", ChangeRed);
            
            FSM smallFsm = new FSM("Small", false, 2, 2);
            
            NodeFSM greenNodeFsm = new NodeFSM("Green", ChangeGreen);
            NodeFSM blueNodeFsm = new NodeFSM("Blue", ChangeBlue);

            TransitionFSM greenToBlue = new TransitionFSM(greenNodeFsm, blueNodeFsm, "greenToBlue", TimerFunc, () => _currentTime = 0.0f);
            TransitionFSM blueToGreen = new TransitionFSM(blueNodeFsm, greenNodeFsm, "blueToGreen", TimerFunc, () => _currentTime = 0.0f);
            
            smallFsm.RegisterNode(greenNodeFsm);
            smallFsm.RegisterNode(blueNodeFsm);
            smallFsm.RegisterTransition(greenToBlue);
            smallFsm.RegisterTransition(blueToGreen);
            smallFsm.Init(greenNodeFsm);
            smallFsm.Pause();
            
            NodeHFSM nodeH = new NodeHFSM("Sub",_fsm, smallFsm);
            
            TransitionFSM bigToSmall = new TransitionFSM(redNodeFsm, nodeH, "bigToSmall", () => UnityEngine.Input.GetKeyDown(KeyCode.C));
            TransitionFSM smallToBig = new TransitionFSM(nodeH, redNodeFsm, "smallToBig", () => UnityEngine.Input.GetKeyDown(KeyCode.C), () => _currentTime = 0.0f);
            
            _fsm.RegisterNode(redNodeFsm);
            _fsm.RegisterNode(nodeH);
            _fsm.RegisterTransition(bigToSmall);
            _fsm.RegisterTransition(smallToBig);
            
            _fsm.Init(redNodeFsm);
        }

        private void Update()
        {
            _fsm.Update();
        }

        #region Functions for agent

        private bool TimerFunc()
        {
            Debug.Log($"currentTime: {_currentTime}");
            _currentTime += Time.deltaTime;
            if (_currentTime >= _timerTimeMax) return true;
            return false;
        }

        private void ChangeRed()
        {
            _spriteRenderer.color = Color.red;
        }

        private void ChangeGreen()
        {
            _spriteRenderer.color = Color.green;
        }

        private void ChangeBlue()
        {
            _spriteRenderer.color = Color.blue;
        }
        #endregion
    }
}