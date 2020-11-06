using Otter.Graphics;
using Otter.Utility;
using Otter.Utility.MonoGame;
using WormGame.Core;

namespace WormGame.Static
{
    /// @author Antti Harju
    /// @version v0.5
    /// <summary>
    /// Class for generating random stuff.
    /// </summary>
    public static class Random
    {
        /// <summary>
        /// Returns a random Color from a predetermined array.
        /// </summary>
        /// <returns>Random color</returns>
        public static Color Color
        {
            get
            {
                return Colors.palette[Range(2, Colors.palette.Length)]; // This way each player has a unique color but worms don't have back- or foreground colors.
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
                return Rand.Choose(Colors.directions);
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
            return Rand.Int(min, max);
        }


        /// <summary>
        /// Generates a random valid direction.
        /// </summary>
        /// <param name="collision">Collision</param>
        /// <param name="position">Worm target</param>
        /// <param name="size">Entity size</param>
        /// <returns>Random valid direction</returns>
        public static Vector2 ValidDirection(Collision collision, Vector2 position, int size)
        {
            int direction = Range(0, Colors.directions.Length);
            for (int i = 0; i < Colors.directions.Length; i++)
            {
                if (ValidateDirection(collision, position, size, Colors.directions[direction]))
                    break;
                else
                {
                    direction--;
                    if (direction < 0)
                        direction = 3;
                }
            }
            return Colors.directions[direction];
        }


        /// <summary>
        /// Generates a random valid position.
        /// </summary>
        /// <param name="collision">Collision</param>
        /// <param name="width">Field width</param>
        /// <param name="height">Field height</param>
        /// <param name="validity">See WormGame/Core/Collision.cs for types</param>
        /// <returns>Random valid position</returns>
        public static Vector2 ValidPosition(Collision collision, int width, int height, int validity)
        {
            int randomX = Range(0, width);
            int randomY = Range(0, height);
            if (collision.GetType(randomX, randomY) < validity)
            {
                for (int y = randomY; y < height; y++)
                    for (int x = randomX; x < width; x++)
                        if (collision.GetType(x, y) >= validity)
                        {
                            randomX = x;
                            randomY = y;
                            goto End;
                        }
                for (int y = randomY; y >= 0; y--)
                    for (int x = randomX; x >= 0; x--)
                        if (collision.GetType(x, y) >= validity)
                        {
                            randomX = x;
                            randomY = y;
                            goto End;
                        }
                return -Vector2.One;
            }
        End:
            return new Vector2(collision.EntityX(randomX), collision.EntityY(randomY));
        }


        /// <summary>
        /// Validate a direction. Technically it doesn't belong in random, but it is used here.
        /// </summary>
        /// <param name="collision">Collision</param>
        /// <param name="position">Position</param>
        /// <param name="direction">Direction to check</param>
        /// <returns>Is direction valid or not</returns>
        public static bool ValidateDirection(Collision collision, Vector2 position, int size, Vector2 direction)
        {
            position += direction * size;
            return collision.GetType(collision.X(position.X), collision.Y(position.Y)) >= collision.fruit;
        }
    }
}