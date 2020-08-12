using Otter.Graphics;
using Otter.Utility.MonoGame;
using WormGame.Core;

namespace WormGame.Static
{
    /// @author Antti Harju
    /// @version 12.08.2020
    /// <summary>
    /// Class for static things that don't belong anywhere else.
    /// </summary>
    public static class Help
    {
        public static readonly Color[] colors = { Color.Red, Color.Blue, Color.Green, Color.Yellow, Color.Magenta, Color.Cyan, Color.Orange };
        public static readonly Vector2[] directions = { new Vector2(0, -1), new Vector2(-1, 0), new Vector2(0, 1), new Vector2(1, 0) };


        /// <summary>
        /// Check if two colors are the same. Otter doesn't have one so I made this.
        /// </summary>
        /// <param name="a">Color a</param>
        /// <param name="b">Color b</param>
        /// <returns>Are the two colors the same (ignores alpha)</returns>
        /// TODO: Contribute to Otter? But in a way that == operator works.
        public static bool Equal(Color a, Color b)
        {
            if (a.R == b.R && a.G == b.G && a.B == b.B)
                return true;
            return false;
        }


        /// <summary>
        /// Validates directions
        /// </summary>
        /// <param name="collision">Collision</param>
        /// <param name="target">Target</param>
        /// <param name="direction">Direction to check</param>
        /// <returns>Is direction valid or not</returns>
        public static bool ValidateDirection(Collision collision, Vector2 target, int size, Vector2 direction)
        {
            target += direction * size;
            return collision.Check(collision.X(target.X), collision.Y(target.Y)) >= collision.fruit;
        }
    }
}
