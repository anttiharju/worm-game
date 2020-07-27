using Otter.Utility.MonoGame;

namespace WormGame.Static
{
    /// @author Antti Harju
    /// @version 18.07.2020
    /// <summary>
    /// Class for miscellaneous mathematical functions.
    /// </summary>
    public static class Mathf
    {
        /// <summary>
        /// Returns the bigger value.
        /// </summary>
        /// <param name="a">First value</param>
        /// <param name="b">Second value</param>
        /// <returns>Bigger value</returns>
        /// <example>
        /// <pre name="test">
        ///  Bigger(1, 2)   ~~~ 2;
        ///  Bigger(2, 1)   ~~~ 2;
        ///  Bigger(-1, 1)  ~~~ 1;
        ///  Bigger(0, 0)   ~~~ 0;
        ///  Bigger(-1, -2) ~~~ -1;
        ///  Bigger(-2, -1) ~~~ -1;
        /// </pre>
        /// </example>
        public static float Bigger(float a, float b)
        {
            if (b < a) return a;
            return b;
        }


        /// <summary>
        /// Lite version of Math.Abs().
        /// </summary>
        /// <param name="number">Number</param>
        /// <returns>Absolute value of number</returns>
        /// <example>
        /// <pre name="test">
        ///  FastAbs(1)  ~~~ 1;
        ///  FastAbs(0)  ~~~ 0;
        ///  FastAbs(-1) ~~~ 1;
        /// </pre>
        /// </example>
        public static float FastAbs(float number)
        {
            if (number < 0) return -number;
            return number;
        }


        /// <summary>
        /// Function to convert whole number floats to integers. Lite version of Math.Round().
        /// </summary>
        /// <param name="number">float</param>
        /// <returns>int</returns>
        /// <example>
        /// <pre name="test">
        ///  FastRound(2.0000000000000000000000001f)  === 2;
        ///  FastRound(0.9999999999999999999999999f)  === 1;
        ///  FastRound(0.0000000000000000000000001f)  === 0;
        ///  FastRound(-0.9999999999999999999999999f) === -1;
        ///  FastRound(-2.0000000000000000000000001f) === -2;
        /// </pre>
        /// </example>
        public static int FastRound(float number, float accuracy = 0.1f)
        {
            if (number < 0)
                return (int)(number - accuracy);
            return (int)(number + accuracy);
        }


        /// <summary>
        /// Function to normalize floats.
        /// </summary>
        /// <param name="number">Number to normalize</param>
        /// <returns>-1, 0 or 1</returns>
        /// <example>
        /// <pre name="test">
        ///  Normalize(0.01f) ~~~ 1;
        ///  Normalize(-0.1f) ~~~ -1;
        ///  Normalize(0)     ~~~ 0;
        /// </pre>
        /// </example>
        public static int Normalize(float number)
        {
            if (number > 0) return 1;
            if (number < 0) return -1;
            return 0;
        }


        /// <summary>
        /// Rotates a Vector2 90° counter-clockwise.
        /// </summary>
        /// <param name="vector">Vector to rotate</param>
        /// <returns>Vector rotated counter-clockwise by 90°</returns>
        /// <example>
        /// <pre name="test">
        /// Vector2 v = new Vector2(-1, 0);           // Down
        ///  v = RotateCCW(v) === new Vector2(0, 1);  // Right
        ///  v = RotateCCW(v) === new Vector2(1, 0);  // Up
        ///  v = RotateCCW(v) === new Vector2(0, -1); // Left
        ///      RotateCCW(v) === new Vector2(-1, 0); // Down
        /// </pre>
        /// </example>
        public static Vector2 RotateCCW(Vector2 vector)
        {
            float x = vector.X;
            vector.X = vector.Y;
            vector.Y = -x;
            return vector;
        }


        /// <summary>
        /// Rotates a Vector2 90° clockwise.
        /// </summary>
        /// <param name="vector">Vector to rotate</param>
        /// <returns>Vector rotated clockwise by 90°</returns>
        /// <example>
        /// <pre name="test">
        /// Vector2 v = new Vector2(-1, 0);          // Down
        ///  v = RotateCW(v) === new Vector2(0, -1); // Left
        ///  v = RotateCW(v) === new Vector2(1, 0);  // Up
        ///  v = RotateCW(v) === new Vector2(0, 1);  // Right
        ///      RotateCW(v) === new Vector2(-1, 0); // Down
        /// </pre>
        /// </example>
        public static Vector2 RotateCW(Vector2 vector)
        {
            float x = vector.X;
            vector.X = -vector.Y;
            vector.Y = x;
            return vector;
        }


        /// <summary>
        /// Returns the smaller value.
        /// </summary>
        /// <param name="a">First value</param>
        /// <param name="b">Second value</param>
        /// <returns>Smaller value</returns>
        /// <example>
        /// <pre name="test">
        ///  Smaller(1, 2)  ===  1;
        ///  Smaller(2, 1)  ===  1;
        ///  Smaller(-1, 1) === -1;
        ///  Smaller(0, 0)  ===  0;
        /// </pre>
        /// </example>
        public static int Smaller(int a, int b)
        {
            if (a < b) return a;
            return b;
        }


        /// <summary>
        /// Returns the smaller value.
        /// </summary>
        /// <param name="a">First value</param>
        /// <param name="b">Second value</param>
        /// <returns>Smaller value</returns>
        /// <example>
        /// <pre name="test">
        ///  Smaller(1, 2)  ~~~  1;
        ///  Smaller(2, 1)  ~~~  1;
        ///  Smaller(-1, 1) ~~~ -1;
        ///  Smaller(0, 0)  ~~~  0;
        /// </pre>
        /// </example>
        public static float Smaller(float a, float b)
        {
            if (a < b) return a;
            return b;
        }
    }
}
