using System;

namespace AIBehaviourAPI.US
{
    public class UtilityAction : ANodeUS
    {
        private ANodeUS _parentNode => _inputNodes.Count > 0? _inputNodes[0] : null;
        
        public UtilityAction(string name, Action action, ANodeUS parent) : base(name)
        {
            _action = action;
            _inputNodes.Add(parent);
            parent.OutputNodes.Add(this);
        }

        public override float GetUtilityValue()
        {
            if (_isCachedUtilityValueValid) return _cachedUtilityValue;

            _cachedUtilityValue = _parentNode.GetUtilityValue();
            _isCachedUtilityValueValid = true;
            return _cachedUtilityValue;
        }
    }
}