using System;
using AIBehaviourAPI.Fsm;
using AIBehaviourAPI.US;
using AIBehaviourAPI.US.DecisionFactors;
using AIBehaviourAPI.US.UtilityFunctions;
using UnityEngine;

namespace AIBehaviourAPI.Demos
{
    public class CubeChangeFSMWithUS : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;

        private FSM _fsm;

        private float _currentTime = 0.0f;
        private float _maxTime = 3.0f;

        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();

            _fsm = new FSM("Main fsm");
            
            NodeFSM greenNode = new NodeFSM("green node", ChangeGreen);
            
            // US
            UtilitySystem us = new UtilitySystem("Cube change US", USMode.Max, 10, 3);
            
            UtilityPerception pressRPerception = new UtilityPerception("press R perception", () => UnityEngine.Input.GetKey(KeyCode.R)? 0.5f : 0.0f);
            UtilityPerception pressYPerception = new UtilityPerception("press Y perception", () => UnityEngine.Input.GetKey(KeyCode.Y)? 0.5f : 0.0f);
            UtilityPerception timerPerception = new UtilityPerception("timer perception", () => _currentTime);
            
            DecisionFactor rDecisionFactor = new DecisionFactor("r decision factor", new Line(0.0f, 1.0f), pressRPerception);
            DecisionFactor yDecisionFactor = new  DecisionFactor("y decision factor", new Line(0.0f, 1.0f), pressYPerception);
            DecisionFactor timerDecisionFactor = new DecisionFactor("timer decision factor", new Line(0.0f, _maxTime), timerPerception);

            FusionFactorWeightedSum fusionFactor = new FusionFactorWeightedSum("fusion factor", 
                new ANodeUS[] { rDecisionFactor, yDecisionFactor, timerDecisionFactor },
                new float[] { 0.25f, 0.25f, 0.5f });
            
            UtilityAction redAction = new UtilityAction("red action", ChangeRed, rDecisionFactor);
            UtilityAction yellowAction = new UtilityAction("yellow action", ChangeYellow, yDecisionFactor);
            UtilityAction blueAction = new UtilityAction("blue action", ChangeBlue, fusionFactor);
            
            us.RegisterNode(pressRPerception);
            us.RegisterNode(pressYPerception);
            us.RegisterNode(timerPerception);
            
            us.RegisterNode(rDecisionFactor);
            us.RegisterNode(yDecisionFactor);
            us.RegisterNode(timerDecisionFactor);
            
            us.RegisterNode(fusionFactor);
            
            us.RegisterAction(redAction);
            us.RegisterAction(yellowAction);
            us.RegisterAction(blueAction);
            
            us.Init();
            us.Pause();
            
            NodeHFSM utilitySystemNode = new NodeHFSM("utility system node", _fsm, us);

            TransitionFSM greenToUSTransition = new TransitionFSM(greenNode, utilitySystemNode, "green to US", () => UnityEngine.Input.GetKeyDown(KeyCode.C), () => _currentTime = 0.0f);
            TransitionFSM usToGreenTransition = new TransitionFSM(utilitySystemNode, greenNode, "US to green", () => UnityEngine.Input.GetKeyDown(KeyCode.C), () => _currentTime = 0.0f);
            
            _fsm.RegisterNode(greenNode);
            _fsm.RegisterNode(utilitySystemNode);
            
            _fsm.RegisterTransition(greenToUSTransition);
            _fsm.RegisterTransition(usToGreenTransition);
            
            _fsm.Init(greenNode);
        }

        private void Update()
        {
            UpdateTime();
            
            _fsm.Update();
        }

        private void UpdateTime()
        {
            _currentTime += Time.deltaTime;
            if (_currentTime >= _maxTime) _currentTime = 0.0f;
        }

        #region Functions for agent

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
        
        private void ChangeYellow()
        {
            _spriteRenderer.color = Color.yellow;
        }
        #endregion
    }
}