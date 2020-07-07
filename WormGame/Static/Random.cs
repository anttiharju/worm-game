using Otter.Graphics;
using Otter.Utility.MonoGame;
using WormGame.Manager;

namespace WormGame.Static
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
        /// Returns a random Color from an array.
        /// </summary>
        /// <returns>Random color</returns>
        public static Color Color()
        {
            return colors[Range(0, colors.Length)];
        }

        /// <summary>
        /// Returns a random directions.
        /// </summary>
        /// <returns>Random color</returns>
        public static Vector2 Direction()
        {
            return Help.directions[Range(0, Help.directions.Length)];
        }

        /// <summary>
        /// Returns a random valid directions.
        /// </summary>
        /// <returns>Random color</returns>
        public static Vector2 ValidDirection(Collision field, Vector2 target, int size)
        {
            int direction = Range(0, Help.directions.Length);
            for (int i = 0; i < 3; i++)
            {
                if (Help.ValidateDirection(field, target, Help.directions[direction] * size))
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
