using Otter.Graphics;
using Otter.Graphics.Drawables;
using Otter.Utility.MonoGame;
using WormGame.Core;
using WormGame.Static;
using WormGame.Pooling;

namespace WormGame.GameObject
{
    /// <summary>
    /// Fruit class. Spawns automatically to a free position.
    /// </summary>
    public class Fruit : PoolableEntity
    {
        private readonly Collision field;
        private readonly int width;
        private readonly int height;


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="config"></param>
        public Fruit(Config config) : base()
        {
            field = config.field;
            width = config.width;
            height = config.height;
            Image image = Image.CreateCircle(config.size / 3, Color.White);
            image.OutlineColor = Color.Black;
            image.OutlineThickness = config.size / 8;
            image.CenterOrigin();
            AddGraphic(image);
        }


        /// <summary>
        /// Spawns fruit to a free position.
        /// </summary>
        /// <returns>Fruit</returns>
        public Fruit Spawn()
        {
            Vector2 random = Random.ValidPosition(field, width, height, 4);
            if (random.X == -1 || field.Get(random) != null)
            {
                Disable();
                return null;
            }
            X = random.X;
            Y = random.Y;
            field.Set(this, Position);
            return this;
        }
    }
}
