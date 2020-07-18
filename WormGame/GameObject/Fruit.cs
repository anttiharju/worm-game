using Otter.Graphics;
using Otter.Graphics.Drawables;
using Otter.Utility.MonoGame;
using WormGame.Core;
using WormGame.Static;
using WormGame.Pooling;

namespace WormGame.GameObject
{
    public class Fruit : PoolableEntity
    {
        private readonly Collision field;
        private readonly int width;
        private readonly int height;


        public Fruit(Config config) : base()
        {
            field = config.field;
            width = config.width;
            height = config.height;
            Image image = Image.CreateRectangle(config.size / 4, config.size / 2, Color.Red);
            Image image2 = Image.CreateRectangle(config.size / 2, config.size / 4, Color.Red);
            image.CenterOrigin();
            image2.CenterOrigin();
            AddGraphic(image);
            AddGraphic(image2);
        }

        public Fruit Spawn()
        {
            Vector2 random = Random.ValidPosition(field, width, height);
            X = random.X;
            Y = random.Y;
            field.Set(this, Position);
            return this;
        }
    }
}
