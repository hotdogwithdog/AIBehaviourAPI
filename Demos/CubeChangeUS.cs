using System;
using AIBehaviourAPI.US;
using AIBehaviourAPI.US.DecisionFactors;
using AIBehaviourAPI.US.UtilityFunctions;
using UnityEngine;

namespace AIBehaviourAPI.Demos
{
    public class CubeChangeUS : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;

        private UtilitySystem _US;

        private float _currentTime = 0.0f;
        private float _maxTime = 3.0f;

        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();

            _US = new UtilitySystem("Cube change US", USMode.Max, 10, 3);
            
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
            
            _US.RegisterNode(pressRPerception);
            _US.RegisterNode(pressYPerception);
            _US.RegisterNode(timerPerception);
            
            _US.RegisterNode(rDecisionFactor);
            _US.RegisterNode(yDecisionFactor);
            _US.RegisterNode(timerDecisionFactor);
            
            _US.RegisterNode(fusionFactor);
            
            _US.RegisterAction(redAction);
            _US.RegisterAction(yellowAction);
            _US.RegisterAction(blueAction);
            
            _US.Init();
        }

        private void Update()
        {
            UpdateTime();
            
            _US.Update();
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