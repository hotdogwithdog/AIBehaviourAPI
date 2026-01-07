using AIBehaviourAPI.Bt;
using AIBehaviourAPI.Bt.Composites;
using AIBehaviourAPI.Bt.Decorators;
using AIBehaviourAPI.Fsm;
using UnityEngine;

namespace AIBehaviourAPI.Demos
{
    public class CubeChangeFSMWithBT : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;

        private FSM _fsm;
        
        private float _timerTimeMax = 3.0f;
        private float _currentTime = 0.0f;
        
        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();

            _fsm = new FSM("Cube Change FSM", true, 2, 2);
            
            // --------------------------------------------------- small FSM ---------------------------------------------------
            FSM smallFSM =  new FSM("Small FSM", true, 2, 2);
            
            NodeFSM yellowNode = new NodeFSM("Yellow Node", ChangeYellow);
            NodeFSM greenNode = new NodeFSM("Green Node", ChangeGreen);

            TransitionFSM yellowToGreenTransition = new TransitionFSM(yellowNode, greenNode, "Yellow to green", TimerFunc, () => _currentTime = 0.0f);
            TransitionFSM greenToYellowTransition = new TransitionFSM(greenNode, yellowNode, "Yellow to green", TimerFunc, () => _currentTime = 0.0f);
            
            smallFSM.RegisterNode(yellowNode);
            smallFSM.RegisterNode(greenNode);
            
            smallFSM.RegisterTransition(yellowToGreenTransition);
            smallFSM.RegisterTransition(greenToYellowTransition);
            
            smallFSM.Init(yellowNode);
            smallFSM.Pause();
            
            NodeHFSM smallFSMNode = new NodeHFSM("small FSM Node", _fsm, smallFSM);

            // --------------------------------------------------- BT ---------------------------------------------------
            BT bt = new BT("BT", 6);
            
            LeafNode isRPressedLNC = new LeafNode("isRPressedLNC", () => UnityEngine.Input.GetKey(KeyCode.R));
            LeafNode changeRedLN = new LeafNode("changeRedLN", ChangeRed, true);
            LeafNode changeBlueLN = new LeafNode("changeBlueLN", ChangeBlue, true);
            
            SequenceNode redSequenceNode = new SequenceNode("redSequenceNode", new ANodeBT[] {isRPressedLNC, changeRedLN});
            SelectorNode mainSelectorNode = new SelectorNode("mainSelectorNode", new ANodeBT[] {redSequenceNode, changeBlueLN}, SelectorMode.Order);

            LoopInfiniteNode mainLoop = new LoopInfiniteNode("main Loop", mainSelectorNode);
            
            bt.RegisterNode(isRPressedLNC);
            bt.RegisterNode(changeRedLN);
            bt.RegisterNode(changeBlueLN);
            
            bt.RegisterNode(redSequenceNode);
            bt.RegisterNode(mainSelectorNode);
            
            bt.RegisterNode(mainLoop);
            
            bt.Init(mainLoop);
            bt.Pause();
            
            NodeHFSM BTNode = new NodeHFSM("BTNode", _fsm, bt);
            
            // --------------------------------------------------- FSM ---------------------------------------------------
            TransitionFSM smallFSMToBT = new TransitionFSM(smallFSMNode, BTNode, "fsm to bt", () => UnityEngine.Input.GetKeyDown(KeyCode.C), () => _currentTime = 0.0f);
            TransitionFSM BTToSmallFSM = new TransitionFSM(BTNode, smallFSMNode, "bt to fsm", () => UnityEngine.Input.GetKeyDown(KeyCode.C), () => _currentTime = 0.0f);
            
            _fsm.RegisterNode(smallFSMNode);
            _fsm.RegisterNode(BTNode);
            
            _fsm.RegisterTransition(smallFSMToBT);
            _fsm.RegisterTransition(BTToSmallFSM);
            
            _fsm.Init(smallFSMNode);
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

        private void ChangeYellow()
        {
            Debug.Log("Change Yellow");
            _spriteRenderer.color = Color.yellow;
        }
        #endregion
    }
}