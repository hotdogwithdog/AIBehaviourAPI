namespace AIBehaviourAPI.US.DecisionFactors
{
    public class FusionFactorMin : ANodeUS
    {
        public FusionFactorMin(string name, ANodeUS[] parents) : base(name)
        {
            _inputNodes.AddRange(parents);
            foreach (ANodeUS parentNode in parents)
            {
                parentNode.OutputNodes.Add(this);
            }
        }

        public override float GetUtilityValue()
        {
            if (_isCachedUtilityValueValid) return _cachedUtilityValue;
            
            float min = float.MaxValue;
            foreach (ANodeUS parentNode in _inputNodes)
            {
                float value = parentNode.GetUtilityValue();
                if (value < min) min = value;
            }

            _cachedUtilityValue = min;
            _isCachedUtilityValueValid = true;
            return _cachedUtilityValue;
        }
    }
}