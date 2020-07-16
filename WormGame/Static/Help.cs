using Otter.Utility.MonoGame;
using WormGame.Core;

namespace WormGame.Static
{
    /// <summary>
    /// Class for static things that don't belong anywhere else.
    /// </summary>
    public static class Help
    {
        public static readonly Vector2[] directions = { new Vector2(0, -1), new Vector2(-1, 0), new Vector2(0, 1), new Vector2(1, 0) };

        /// <summary>
        /// Validates directions
        /// </summary>
        /// <param name="field"></param>
        /// <param name="target"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static bool ValidateDirection(Collision field, Vector2 target, int size, Vector2 direction)
        {
            target += direction * size;
            return field.Check(field.X(target.X), field.Y(target.Y)) != 2;
        }
    }
}
