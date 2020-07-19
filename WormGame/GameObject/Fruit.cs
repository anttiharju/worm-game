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
    /// TODO: What if there's no free positions?
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
            Image image = Image.CreateRectangle(config.size / 4, config.size / 2, Color.White);
            Image image2 = Image.CreateRectangle(config.size / 2, config.size / 4, Color.White);
            image.CenterOrigin();
            image2.CenterOrigin();
            AddGraphic(image);
            AddGraphic(image2);
        }


        /// <summary>
        /// Spawns fruit to a free position.
        /// </summary>
        /// <returns>Fruit</returns>
        public Fruit Spawn()
        {
            Vector2 random = Random.ValidPosition(field, width, height, 4);
            if (field.Get(random) != null)
                Disable();
            X = random.X;
            Y = random.Y;
            field.Set(this, Position);
            return this;
        }
    }
}
