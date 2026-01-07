using System;
using System.Collections.Generic;

namespace AIBehaviourAPI.Bt
{
    public class ConditionsAndActionsLeafNode : ANodeBT
    {
        /// <summary>
        /// Node that correspond to one composite node of conditional leaf nodes and later the action node
        /// </summary>
        /// <param name="name"> name of the node </param>
        /// <param name="conditions"> array of conditions </param>
        /// <param name="action"> action of the action node </param>
        /// <param name="returnValueConst"> const result of the action node </param>
        public ConditionsAndActionsLeafNode(string name, Func<bool>[] conditions, Action action, bool returnValueConst = true) : base(name)
        {
            _condition = () =>
            {
                foreach (Func<bool> condition in conditions)
                {
                    if (!condition()) return false;
                }

                action();
                return returnValueConst;
            };
        }

        /// <summary>
        /// Node that correspond to one sequence composite of one conditional leaf node and later the action node
        /// </summary>
        /// <param name="name"> name of the node </param>
        /// <param name="condition"> condition to check for do the action </param>
        /// <param name="action"> action of the action node </param>
        /// <param name="returnValueConst"> const result of the action node </param>
        public ConditionsAndActionsLeafNode(string name, Func<bool> condition, Action action, bool returnValueConst = true) : base(name)
        {
            _condition = () =>
            {
                if (!condition()) return false;
                
                action();
                return returnValueConst;
            };
        }

        /// <summary>
        /// Node that correspond to one composite node of conditional leaf nodes and later the action node
        /// </summary>
        /// <param name="name"> name of the node </param>
        /// <param name="conditions"> array of conditions </param>
        /// <param name="actionWithReturn"> action of the action node with dynamic return value for const return value in the action see other overloads of the constructor </param>
        public ConditionsAndActionsLeafNode(string name, Func<bool>[] conditions, Func<bool> actionWithReturn) : base(name)
        {
            _condition = () =>
            {
                foreach (Func<bool> condition in conditions)
                {
                    if (!condition()) return false;
                }

                return actionWithReturn();
            };
        }

        /// <summary>
        /// Node that correspond to one composite node of conditional leaf nodes and later the action node
        /// </summary>
        /// <param name="name"> name of the node </param>
        /// <param name="condition"> condition to check for do the action </param>
        /// <param name="actionWithResult"> action of the action node with dynamic return value for const return value in the action see other overloads of the constructor </param>
        public ConditionsAndActionsLeafNode(string name, Func<bool> condition, Func<bool> actionWithResult) : base(name)
        {
            _condition = () =>
            {
                if (!condition()) return false;
                
                return actionWithResult();
            };
        }
    }
}