using Otter;

namespace WormGame.Help
{
    /// @author anvemaha
    /// @version 01.07.2020
    /// <summary>
    /// Class for miscellaneous mathematical functions.
    /// </summary>
    public static class Mathf
    {
        /// <summary>
        /// Lightweight version of Math.Abs()
        /// </summary>
        /// <param name="number">Number</param>
        /// <returns>Numbers absolute value</returns>
        /// <example>
        /// <pre name="test">
        ///  FastAbs(1) ~~~ 1;
        ///  FastAbs(0) ~~~ 0;
        ///  FastAbs(-1) ~~~ 1;
        /// </pre>
        /// </example>
        public static float FastAbs(float number)
        {
            if (number < 0) return -number;
            return number;
        }


        /// <summary>
        /// Rotates vector counter-clockwise
        /// </summary>
        /// <param name="v">Vector to rotate</param>
        /// <returns>Rotated vector</returns>
        /// <example>
        /// <pre name="test">
        /// Vector2 v = new Vector2(-1, 0);          // Down
        ///  v = RotateCW(v) === new Vector2(0, -1); // Left
        ///  v = RotateCW(v) === new Vector2(1, 0);  // Up
        ///  v = RotateCW(v) === new Vector2(0, 1);  // Right
        ///  v = RotateCW(v) === new Vector2(-1, 0); // Down
        /// </pre>
        /// </example>
        public static Vector2 RotateCW(Vector2 v)
        {
            float x = v.X;
            v.X = -v.Y;
            v.Y = x;
            return v;
        }


        /// <summary>
        /// Rotates vector clockwise
        /// </summary>
        /// <param name="v">Vector to rotate</param>
        /// <returns>Rotated vector</returns>
        /// <example>
        /// <pre name="test">
        /// Vector2 v = new Vector2(-1, 0);           // Down
        ///  v = RotateCCW(v) === new Vector2(0, 1);  // Right
        ///  v = RotateCCW(v) === new Vector2(1, 0);  // Up
        ///  v = RotateCCW(v) === new Vector2(0, -1); // Left
        ///  v = RotateCCW(v) === new Vector2(-1, 0); // Down
        /// </pre>
        /// </example>
        public static Vector2 RotateCCW(Vector2 v)
        {
            float x = v.X;
            v.X = v.Y;
            v.Y = -x;
            return v;
        }


        /// <summary>
        /// Returns the smaller value
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
        /// Returns the smaller value
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
        public static float Smaller(float a, float b)
        {
            if (a < b) return a;
            return b;
        }


        /// <summary>
        /// Returns the bigger value
        /// </summary>
        /// <param name="a">First value</param>
        /// <param name="b">Second value</param>
        /// <returns>Bigger value</returns>
        /// <example>
        /// <pre name="test">
        ///  Bigger(1, 2) === 2;
        ///  Bigger(2, 1) === 2;
        ///  Bigger(-1, 1) === 1;
        ///  Bigger(0, 0) === 0;
        /// </pre>
        /// </example>
        public static float Bigger(float a, float b)
        {
            if (b < a) return a;
            return b;
        }
    }
}
