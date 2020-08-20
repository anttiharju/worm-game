using Otter.Graphics;
using Otter.Graphics.Drawables;
using Otter.Utility.MonoGame;
using WormGame.Core;
using WormGame.Static;

namespace WormGame.Entities
{
    /// @author Antti Harju
    /// @version v0.5
    /// <summary>
    /// Fruit system.
    /// </summary>
    public class Fruits
    {
        private readonly Collision collision;
        private readonly Tilemap tilemap;
        private readonly Color fruitColor;
        private readonly int width;
        private readonly int height;


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="settings">Settings</param>
        public Fruits(Settings settings)
        {
            collision = settings.collision;
            tilemap = settings.tilemap;
            fruitColor = Colors.foreground;
            width = settings.width;
            height = settings.height;
        }


        /// <summary>
        /// Spawn a fruit to a random position.
        /// </summary>
        /// <returns>Success</returns>
        public bool Spawn()
        {
            Vector2 random = Random.ValidPosition(collision, width, height, collision.empty);
            if (random.X == -1)
                return false;
            int x = collision.X(random.X);
            int y = collision.Y(random.Y);
            tilemap.SetTile(x, y, fruitColor, "");
            collision.Set(this, x, y);
            return true;
        }


        /// <summary>
        /// Disable a fruit. Collision doesn't need to be cleared as it's overridden by worm.
        /// </summary>
        /// <param name="x">Horizontal position</param>
        /// <param name="y">Vertical position</param>
        public void Disable(int x, int y)
        {
            tilemap.ClearTile(x, y);
        }
    }
}
