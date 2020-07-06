using Otter.Graphics;

namespace WormGame.Help
{
    /// @author Antti Harju
    /// @version 01.07.2020
    /// <summary>
    /// Random class for randomizing things.
    /// </summary>
    static class Random
    {
        private static readonly System.Random randomGenerator = new System.Random();
        private static readonly Color[] colors = { Otter.Graphics.Color.Blue, Otter.Graphics.Color.Cyan, Otter.Graphics.Color.Green, Otter.Graphics.Color.Magenta, Otter.Graphics.Color.Orange, Otter.Graphics.Color.Yellow };

        /// <summary>
        /// Returns a random number between a and b, b exclusive.
        /// </summary>
        /// <param name="a">Minimum value</param>
        /// <param name="b">Maximum value</param>
        /// <returns>Random number between a and b</returns>
        public static int Range(int a, int b)
        {
            return randomGenerator.Next(a, b);
        }


        /// <summary>
        /// Returns a random Color from a predetermined array.
        /// </summary>
        /// <returns>Random color</returns>
        public static Color Color()
        {
            return colors[Range(0, colors.Length)];
        }
    }
}
