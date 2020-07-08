using Otter.Graphics;
using Otter.Utility.MonoGame;
using WormGame.Core;

namespace WormGame.Static
{
    /// @author Antti Harju
    /// @version 08.07.2020
    /// <summary>
    /// Class for generating random stuff.
    /// </summary>
    static class Random
    {
        private static readonly System.Random randomGenerator = new System.Random();
        private static readonly Color[] colors = { Otter.Graphics.Color.Blue, Otter.Graphics.Color.Cyan, Otter.Graphics.Color.Green, Otter.Graphics.Color.Magenta, Otter.Graphics.Color.Orange, Otter.Graphics.Color.Yellow };

        /// <summary>
        /// Returns a random number between a and b, b exclusive.
        /// </summary>
        /// <param name="min">Minimum value</param>
        /// <param name="max">Maximum value</param>
        /// <returns>Random number between a and b</returns>
        public static int Range(int min, int max)
        {
            return randomGenerator.Next(min, max);
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
        public static Vector2 Direction()
        {
            return Help.directions[Range(0, Help.directions.Length)];
        }


        /// <summary>
        /// Generates a random valid direction.
        /// </summary>
        /// <param name="field">Collision</param>
        /// <param name="position">Worm target</param>
        /// <param name="size">Entity size</param>
        /// <returns>Random valid direction</returns>
        public static Vector2 ValidDirection(Collision field, Vector2 position, int size)
        {
            int direction = Range(0, Help.directions.Length);
            for (int i = 0; i < 3; i++)
            {
                if (Help.ValidateDirection(field, position, size, Help.directions[direction]))
                    break;
                else
                {
                    direction--;
                    if (direction < 0)
                        direction = 3;
                }
            }
            return Help.directions[direction];
        }
    }
}
