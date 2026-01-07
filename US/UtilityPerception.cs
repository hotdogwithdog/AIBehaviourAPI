using System;

namespace AIBehaviourAPI.US
{
    public class UtilityPerception : ANodeUS
    {
        private Func<float> _perceptionFunction;
        public UtilityPerception(string name, Func<float> perceptionFunction) : base(name)
        {
            _perceptionFunction = perceptionFunction;
        }

        public override float GetUtilityValue()
        {
            if (_isCachedUtilityValueValid) return _cachedUtilityValue;
            
            _cachedUtilityValue = _perceptionFunction();
            _isCachedUtilityValueValid = true;
            return _cachedUtilityValue;
        }
    }
}