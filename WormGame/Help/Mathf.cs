using Otter;

namespace WormGame.Help
{
    /// @author anvemaha
    /// @version 01.07.2020
    /// <summary>
    /// Class for miscellaneous mathematical functions.
    /// </summary>
    static class Mathf
    {
        /// <summary>
        /// Lightweight version of Math.Abs()
        /// </summary>
        /// <param name="number">Number</param>
        /// <returns>Numbers absolute value</returns>
        public static float FastAbs(float number)
        {
            if (number < 0) return -number;
            return number;
        }


        /// <summary>
        /// Rotates vector clockwise
        /// </summary>
        /// <param name="v">Vector to rotate</param>
        /// <returns>Rotated vector</returns>
        public static Vector2 RotateCW(Vector2 v)
        {
            return new Vector2(v.Y, -v.X);
        }


        /// <summary>
        /// Rotates vector counter-clockwise
        /// </summary>
        /// <param name="v">Vector to rotate</param>
        /// <returns>Rotated vector</returns>
        public static Vector2 RotateCCW(Vector2 v)
        {
            return new Vector2(-v.Y, v.X);
        }


        /// <summary>
        /// Area two Vector2s roughly equal (collision)
        /// </summary>
        /// <param name="a">First Vector2</param>
        /// <param name="b">Second Vector2</param>
        /// <param name="errorMargin">Accuracy</param>
        /// <returns></returns>
        public static bool RoughlyEquals(Vector2 a, Vector2 b, float errorMargin)
        {
            if (RoughlyEquals(a.X, b.X, errorMargin))
                if (RoughlyEquals(a.Y, b.Y, errorMargin))
                    return true;
            return false;
        }


        /// <summary>
        /// Are two floats roughly equal
        /// </summary>
        /// <param name="a">First float</param>
        /// <param name="b">Second float</param>
        /// <param name="errorMargin">Accuracy</param>
        /// <returns></returns>
        public static bool RoughlyEquals(float a, float b, float errorMargin)
        {
            if (b - errorMargin < a && b + errorMargin > a)
                return true;
            return false;
        }


        /// <summary>
        /// Returns the smaller value
        /// </summary>
        /// <param name="a">First value</param>
        /// <param name="b">Second value</param>
        /// <returns>Smaller value</returns>
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
        public static float Bigger(float a, float b)
        {
            if (b < a) return a;
            return b;
        }
    }
}
