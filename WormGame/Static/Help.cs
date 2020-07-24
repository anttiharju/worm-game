using Otter.Graphics;
using Otter.Utility.MonoGame;
using WormGame.Core;

namespace WormGame.Static
{
    /// @author Antti Harju
    /// @version 24.07.2020
    /// <summary>
    /// Class for static things that don't belong anywhere else.
    /// </summary>
    public static class Help
    {
        public static readonly Color[] colors = { Color.Red, Color.Blue, Color.Green, Color.Yellow, Color.Magenta, Color.Orange, Color.Cyan };
        public static readonly Vector2[] directions = { new Vector2(0, -1), new Vector2(-1, 0), new Vector2(0, 1), new Vector2(1, 0) };


        /// <summary>
        /// Check if two colors are the same. Otter doesn't implement one properly so I have to use my own.
        /// </summary>
        /// <param name="a">Color a</param>
        /// <param name="b">Color b</param>
        /// <returns>Is a the same color as b? (alpha ignored)</returns>
        public static bool Equal(Color a, Color b)
        {
            if (a.R == b.R && a.G == b.G && a.B == b.B)
                return true;
            return false;
        }


        /// <summary>
        /// Validates directions
        /// </summary>
        /// <param name="field">Collision</param>
        /// <param name="target">Target</param>
        /// <param name="direction">Direction to check</param>
        /// <returns>Is direction valid or not</returns>
        public static bool ValidateDirection(Collision field, Vector2 target, int size, Vector2 direction)
        {
            target += direction * size;
            return field.Check(field.X(target.X), field.Y(target.Y)) >= 3;
        }
    }
}
