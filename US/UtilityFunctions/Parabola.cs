using System;

namespace AIBehaviourAPI.US.UtilityFunctions
{
    public class Parabola : IUtilityFunction
    {
        private readonly float _a, _b, _c;
        
       
        /// <summary>
        /// Creates a custom parabola of the formula a*x^2 + b*x + c
        /// </summary>
        public Parabola(float a, float b, float c)
        {
            _a = a;
            _b = b;
            _c = c;
        }

        /// <summary>
        /// Creates a parabola between 0 and 1 in both axis with this formula a*x^2 + b*x + c / a, b, c are set with based on the bool parameter
        /// </summary>
        /// <param name="isConvex"> if is true the parabola is open up with return 1 for 0 and 1 and return 0 for 0.5, otherwise is open down with return 0 for 0 and 1 and return 1 for 0.5
        /// true => {a = 4, b = -4, c = 1} and false => {a = -4, b = 4, c = 0} </param>
        public Parabola(bool isConvex)
        {
            if (isConvex)
            {
                _a = 4.0f;
                _b = -4.0f;
                _c = 1.0f;
                return;
            }
            
            _a = -4.0f;
            _b = 4.0f;
            _c = 0;
        }
        
        public float GetValue(float t)
        {
            return _a*t*t + _b*t + _c;
        }
    }
}