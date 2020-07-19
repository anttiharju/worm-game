using Otter.Graphics;
using Otter.Utility.MonoGame;
using WormGame.Core;

namespace WormGame.Static
{
    /// @author Antti Harju
    /// @version 18.07.2020
    /// <summary>
    /// Class for generating random stuff.
    /// </summary>
    public static class Random
    {
        private static readonly System.Random randomGenerator = new System.Random();
        private static readonly Color[] colors = { Color.Red, Color.Blue, Color.Green, Color.Yellow, Color.Magenta, Color.Orange, Color.Cyan };


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
        /// Returns a random number between min and max (exclusive).
        /// </summary>
        /// <param name="min">Minimum value</param>
        /// <param name="max">Maximum value, exclusive</param>
        /// <returns>Random number between min and max</returns>
        public static int Range(int min, int max)
        {
            return randomGenerator.Next(min, max);
        }


        /// <summary>
        /// Generates a random valid position.
        /// </summary>
        /// <param name="field">Collision</param>
        /// <param name="width">Field width</param>
        /// <param name="height">Field height</param>
        /// <param name="validity">0 out of bounds, 1 worm, 2 brick, 3 fruit, 4 free</param>
        /// <returns>Random valid position</returns>
        public static Vector2 ValidPosition(Collision field, int width, int height, int validity)
        {
            int randomX = Range(0, width);
            int randomY = Range(0, height);
            if (field.Check(randomX, randomY) < validity)
            {
                for (int y = randomY; y < height; y++)
                    for (int x = randomX; x < width; x++)
                        if (field.Check(x, y) >= validity)
                        {
                            randomX = x;
                            randomY = y;
                            goto End;
                        }
                for (int y = randomY; y >= 0; y--)
                    for (int x = randomX; x >= 0; x--)
                        if (field.Check(x, y) >= validity)
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
            for (int i = 0; i < Help.directions.Length; i++)
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
