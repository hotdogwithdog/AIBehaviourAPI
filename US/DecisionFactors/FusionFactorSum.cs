namespace AIBehaviourAPI.US.DecisionFactors
{
    public class FusionFactorSum : ANodeUS
    {
        public FusionFactorSum(string name, ANodeUS[] parents) : base(name)
        {
            _inputNodes.AddRange(parents);
            foreach (ANodeUS parentNode in parents)
            {
                parentNode.OutputNodes.Add(this);
            }
        }

        public override float GetUtilityValue()
        {
            float sumValue = 0;
            foreach (ANodeUS parentNode in _inputNodes)
            {
                sumValue += parentNode.GetUtilityValue();
            }
            
            _cachedUtilityValue = sumValue / (float)_inputNodes.Count;
            _isCachedUtilityValueValid = true;
            return _cachedUtilityValue;
        }
    }
}