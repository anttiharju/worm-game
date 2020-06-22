using Otter;
using System;

namespace WBGame.Other
{
    /// @author Antti Harju
    /// @version 21.06.2020
    /// <summary>
    /// Contains miscallenous static helper subroutines.
    /// </summary>
    static class Helper
    {
        private static readonly Random rand = new Random();
        private static readonly Color[] colors = { Color.Blue, Color.Cyan, Color.Green, Color.Magenta, Color.Orange, Color.Yellow };


        /// <summary>
        /// Return a random number between a and b, both inclusive
        /// </summary>
        /// <param name="a">Minimum value</param>
        /// <param name="b">Maximum value</param>
        /// <returns>Random number between a and b</returns>
        public static int RandomRange(int a, int b)
        {
            return rand.Next(a, b + 1);
        }


        /// <summary>
        /// Returns a random Color from a predetermined array
        /// </summary>
        /// <returns>Random color</returns>
        public static Color RandomColor()
        {
            return colors[RandomRange(0, colors.Length - 1)];
        }


        /// <summary>
        /// Generates set of random directions.
        /// </summary>
        /// <param name="length">How many directions</param>
        /// <returns>Set of random directions</returns>
        public static char[] GenerateDirections(int length)
        {
            char[] directions = new char[length - 1];
            char[] chars = { 'W', 'A', 'S', 'D' };

            int pos = RandomRange(0, 3);
            for (int i = 0; i < directions.Length; i++)
            {
                int dir = RandomRange(0, 2); // Returns 0, 1 or 2
                if (dir == 1)
                    pos++;
                if (dir == 2)
                    pos--;
                if (pos < 0)
                    pos = 3;
                if (pos > 3)
                    pos = 0;
                directions[i] = chars[pos];
            }
            return directions;
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
    }
}
