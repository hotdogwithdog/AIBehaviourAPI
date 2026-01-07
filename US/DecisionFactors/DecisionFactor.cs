using System;
using AIBehaviourAPI.US.UtilityFunctions;

namespace AIBehaviourAPI.US.DecisionFactors
{
    public class DecisionFactor : ANodeUS
    {
        private readonly IUtilityFunction _utilityFunction;
        private ANodeUS _parentNode => _inputNodes.Count > 0? _inputNodes[0] : null;
        
        public DecisionFactor(string name, IUtilityFunction utilityFunction, ANodeUS parent) : base(name)
        {
            _utilityFunction = utilityFunction;
            _inputNodes.Add(parent);
            parent.OutputNodes.Add(this);
        }

        public override float GetUtilityValue()
        {
            if (_isCachedUtilityValueValid) return _cachedUtilityValue;

            _cachedUtilityValue = _utilityFunction.GetValue(_parentNode.GetUtilityValue());
            _isCachedUtilityValueValid = true;
            return _cachedUtilityValue;
        }
    }
}