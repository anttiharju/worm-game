namespace WormGame.Static
{
    /// @author Antti Harju
    /// @version v0.5
    /// <summary>
    /// Simple math library.
    /// </summary>
    public static class SimpleMath
    {
        /// <summary>
        /// Simple version of Math.Abs().
        /// </summary>
        /// <param name="number">Number</param>
        /// <returns>Absolute value of number</returns>
        /// <example>
        /// <pre name="test">
        ///  Abs(1)  ~~~ 1;
        ///  Abs(0)  ~~~ 0;
        ///  Abs(-1) ~~~ 1;
        /// </pre>
        /// </example>
        public static float Abs(float number)
        {
            if (number < 0) return -number;
            return number;
        }


        /// <summary>
        /// Function to convert whole number floats to integers. Simple version of Math.Round().
        /// </summary>
        /// <param name="number">float</param>
        /// <returns>int</returns>
        /// <example>
        /// <pre name="test">
        ///  Round(2.0000000000000000000000001f)  === 2;
        ///  Round(0.9999999999999999999999999f)  === 1;
        ///  Round(0.0000000000000000000000001f)  === 0;
        ///  Round(-0.9999999999999999999999999f) === -1;
        ///  Round(-2.0000000000000000000000001f) === -2;
        /// </pre>
        /// </example>
        public static int Round(float number, float accuracy = 0.5f)
        {
            if (number < 0)
                return (int)(number - accuracy);
            return (int)(number + accuracy);
        }
    }
}
