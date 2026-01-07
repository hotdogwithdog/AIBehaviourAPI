
namespace AIBehaviourAPI.US.UtilityFunctions
{
    public class Sigmoid : IUtilityFunction
    {
        private readonly float _a, _b;
        
        /// <summary>
        /// Construct a sigmoid function
        /// </summary>
        /// <param name="a"> Is the strength/slope </param>
        /// <param name="b"> Is the value of t that gives the 0,5 value</param>
        public Sigmoid(float a, float b)
        {
            _a = a;
            _b = b;
        }
        
        public float GetValue(float t)
        {
            return 1.0f / (1.0f + (float)System.Math.Exp(-_a * (t - _b)));
        }
    }
}