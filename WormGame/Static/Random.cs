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
    public static class Random
    {
        private static readonly System.Random randomGenerator = new System.Random();
        private static readonly Color[] colors = { Color.Blue, Color.Cyan, Color.Green, Color.Magenta, Color.Orange, Color.Yellow };


        /// <summary>
        /// Returns a random Color from a predetermined array.
        /// </summary>
        /// <returns>Random color</returns>
        public static Color Color
        {
            get
            {
                return colors[Range(0, colors.Length)];
            }
        }


        /// <summary>
        /// Returns a random direction.
        /// </summary>
        /// <returns>Random color</returns>
        public static Vector2 Direction
        {
            get
            {
                return Help.directions[Range(0, Help.directions.Length)];
            }
        }


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



        public static Vector2 ValidPosition(Collision field, int width, int height, int stritness = 0)
        {
            int randomX = Range(0, width);
            int randomY = Range(0, height);
            if (field.Check(randomX, randomY) > stritness)
            {
                for (int y = randomY; y < height; y++)
                    for (int x = randomX; x < width; x++)
                        if (field.Check(x, y) <= stritness)
                        {
                            randomX = x;
                            randomY = y;
                            goto End;
                        }
                for (int y = randomY; y >= 0; y--)
                    for (int x = randomX; x >= 0; x--)
                        if (field.Check(x, y) <= stritness)
                        {
                            randomX = x;
                            randomY = y;
                            goto End;
                        }
            }
        End:
            return new Vector2(field.EntityX(randomX), field.EntityY(randomY));
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
