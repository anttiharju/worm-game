using Otter.Graphics;
using Otter.Graphics.Drawables;
using Otter.Utility.MonoGame;
using WormGame.Core;
using WormGame.Static;

namespace WormGame.Entities
{
    /// @author Antti Harju
    /// @version 14.08.2020
    /// <summary>
    /// Fruit manager.
    /// </summary>
    public class Fruits
    {
        private readonly Collision collision;
        private readonly Tilemap tilemap;
        private readonly Color fruitColor;
        private readonly int width;
        private readonly int height;


        /// <summary>
        /// Initialize manager.
        /// </summary>
        /// <param name="config">Configuration</param>
        public Fruits(Config config)
        {
            collision = config.collision;
            tilemap = config.tilemap;
            fruitColor = config.foregroundColor;
            width = config.width;
            height = config.height;
        }


        /// <summary>
        /// Spawns a fruit to a random position.
        /// </summary>
        /// <returns></returns>
        public bool Spawn()
        {
            Vector2 random = Random.ValidPosition(collision, width, height, collision.empty);
            if (random.X == -1)
                return false;
            int x = collision.X(random.X);
            int y = collision.Y(random.Y);
            tilemap.SetTile(x, y, fruitColor, "");
            collision.Add(this, x, y);
            return true;
        }


        /// <summary>
        /// Removes fruit from tilemap.
        /// </summary>
        /// <param name="x">Horizontal position</param>
        /// <param name="y">Vertical position</param>
        public void Remove(int x, int y)
        {
            tilemap.ClearTile(x, y);
        }
    }
}
