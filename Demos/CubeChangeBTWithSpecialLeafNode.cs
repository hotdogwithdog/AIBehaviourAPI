using AIBehaviourAPI.Bt;
using AIBehaviourAPI.Bt.Composites;
using AIBehaviourAPI.Bt.Decorators;
using UnityEngine;

namespace AIBehaviourAPI.Demos
{
    public class CubeChangeBTWithSpecialLeafNode : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;

        private BT _bt;
        
        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();

            _bt = new BT("Cube Change BT", 6);
            
            ConditionsAndActionsLeafNode changeRedLNCA = new ConditionsAndActionsLeafNode("changeRedLN", () => UnityEngine.Input.GetKey(KeyCode.R), ChangeRed, true);
            LeafNode changeBlueLN = new LeafNode("changeBlueLN", ChangeBlue, true);
            
            SelectorNode mainSelectorNode = new SelectorNode("mainSelectorNode", new ANodeBT[] {changeRedLNCA, changeBlueLN}, SelectorMode.Order);

            LoopInfiniteNode mainLoop = new LoopInfiniteNode("main Loop", mainSelectorNode);
            
            _bt.RegisterNode(changeRedLNCA);
            _bt.RegisterNode(changeBlueLN);
            
            _bt.RegisterNode(mainSelectorNode);
            
            _bt.RegisterNode(mainLoop);
            
            _bt.Init(mainLoop);
        }

        private void Update()
        {
            _bt.Update();
        }

        #region Functions for agent

        private void ChangeRed()
        {
            //Debug.Log("Change Red");
            _spriteRenderer.color = Color.red;
        }

        private void ChangeGreen()
        {
            //Debug.Log("Change Green");
            _spriteRenderer.color = Color.green;
        }

        private void ChangeBlue()
        {
            //Debug.Log("Change Blue");
            _spriteRenderer.color = Color.blue;
        }
        #endregion
    }
}