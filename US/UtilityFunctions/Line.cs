namespace AIBehaviourAPI.US.UtilityFunctions
{
    public class Line : IUtilityFunction
    {
        private readonly float _x1, _x2;
        
        /// <summary>
        /// Create a linear function that returns the value between 0 and 1 when the t value is between x1 and x2
        /// </summary>
        /// <param name="x1"> The lower value when t = x1 it will return 0 if x1 is lower than x2 and 1 if not </param>
        /// <param name="x2"> The upper value when t = x2 it will return 1 if x2 is lower than x1 and 0 if not </param>
        public Line(float x1, float x2)
        {
            _x1 = x1;
            _x2 = x2;
        }
        
        public float GetValue(float t)
        {
            return (t - _x1) / (_x2 - _x1);
        }
    }
}