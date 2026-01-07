using AIBehaviourAPI.Fsm;
using UnityEngine;

namespace AIBehaviourAPI.Demos
{
    public class HierarchicalFSMWithoutSugarNodes : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;

        private FSM _fsm;
        private FSM _smallFsm;

        private float _timerTimeMax = 3.0f;
        private float _currentTime = 0.0f;
        
        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();

            _fsm = new FSM("Cube Change Hierarchical");
            
            NodeFSM redNodeFsm = new NodeFSM("Red", ChangeRed);
            
            _smallFsm = new FSM("Small", true, 2, 2);
            
            NodeFSM greenNodeFsm = new NodeFSM("Green", ChangeGreen);
            NodeFSM blueNodeFsm = new NodeFSM("Blue", ChangeBlue);

            TransitionFSM greenToBlue = new TransitionFSM(greenNodeFsm, blueNodeFsm, "greenToBlue", TimerFunc, () => _currentTime = 0.0f);
            TransitionFSM blueToGreen = new TransitionFSM(blueNodeFsm, greenNodeFsm, "blueToGreen", TimerFunc, () => _currentTime = 0.0f);
            
            _smallFsm.RegisterNode(greenNodeFsm);
            _smallFsm.RegisterNode(blueNodeFsm);
            _smallFsm.RegisterTransition(greenToBlue);
            _smallFsm.RegisterTransition(blueToGreen);
            _smallFsm.Init(greenNodeFsm);
            _smallFsm.Pause();
            
            NodeFSM subActivation = new NodeFSM("Sub", () => _smallFsm.Update());
            
            TransitionFSM bigToSmall = new TransitionFSM(redNodeFsm, subActivation, "bigToSmall", () => UnityEngine.Input.GetKeyDown(KeyCode.C), () => _smallFsm.Resume());
            TransitionFSM smallToBig = new TransitionFSM(subActivation, redNodeFsm, "smallToBig", () => UnityEngine.Input.GetKeyDown(KeyCode.C), () =>
            {
                _currentTime = 0.0f;
                _smallFsm.Pause();
            });
            
            _fsm.RegisterNode(redNodeFsm);
            _fsm.RegisterNode(subActivation);
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