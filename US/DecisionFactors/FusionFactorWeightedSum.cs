namespace AIBehaviourAPI.US.DecisionFactors
{
    public class FusionFactorWeightedSum : ANodeUS
    {
        private float[] _weights;
        
        public FusionFactorWeightedSum(string name, ANodeUS[] parents, float[] weights) : base(name)
        {
            _inputNodes.AddRange(parents);
            foreach (ANodeUS parentNode in parents)
            {
                parentNode.OutputNodes.Add(this);
            }
            
            _weights = weights;

            if (weights.Length != parents.Length)
                throw new BehaviourAPIException("FusionFactorWeightedSum::FusionFactorWeightedSum: The Weights array must be the same size that the nodes added.");
        }

        public override float GetUtilityValue()
        {
            if (_isCachedUtilityValueValid) return _cachedUtilityValue;

            float sumValue = 0;
            
            for (int i = 0; i < _inputNodes.Count; ++i)
            {
                sumValue += _inputNodes[i].GetUtilityValue() * _weights[i];
            }

            _cachedUtilityValue = sumValue;
            _isCachedUtilityValueValid = true;
            return _cachedUtilityValue;
        }
    }
}