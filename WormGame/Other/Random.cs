using Otter;

namespace WormGame.Other
{
    /// @author anvemaha
    /// @version 01.07.2020
    /// <summary>
    /// Random class for randomizing things.
    /// </summary>
    static class Random
    {
        private static readonly System.Random randomNumber = new System.Random();
        private static readonly Color[] colors = { Otter.Color.Blue, Otter.Color.Cyan, Otter.Color.Green, Otter.Color.Magenta, Otter.Color.Orange, Otter.Color.Yellow };
        private static readonly string[] directions = { "UP", "LEFT", "DOWN", "RIGHT" };


        /// <summary>
        /// Returns a random number between a and b, b exclusive.
        /// </summary>
        /// <param name="a">Minimum value</param>
        /// <param name="b">Maximum value</param>
        /// <returns>Random number between a and b</returns>
        public static int Range(int a, int b)
        {
            return randomNumber.Next(a, b);
        }


        /// <summary>
        /// Returns a random Color from a predetermined array.
        /// </summary>
        /// <returns>Random color</returns>
        public static Color Color()
        {
            return colors[Range(0, colors.Length)];
        }


        /// <summary>
        /// Returns a random direction.
        /// </summary>
        /// <returns>Random color</returns>
        public static string Direction()
        {
            return directions[Range(0, colors.Length)];
        }
    }
}
