using Otter.Utility.MonoGame;
using WormGame.Core;

namespace WormGame.Static
{
    /// <summary>
    /// Class for static things that don't belong anywhere else.
    /// </summary>
    static class Help
    {
        public static readonly Vector2[] directions = { new Vector2(0, -1), new Vector2(-1, 0), new Vector2(0, 1), new Vector2(1, 0) };

        /// <summary>
        /// Validates directions
        /// </summary>
        /// <param name="field"></param>
        /// <param name="target"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static bool ValidateDirection(Collision field, Vector2 target, Vector2 direction)
        {
            target += direction;
            return field.Check(field.X(target.X), field.Y(target.Y));
        }
    }
}
