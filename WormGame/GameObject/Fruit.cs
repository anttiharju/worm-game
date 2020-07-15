using Otter.Graphics;
using Otter.Graphics.Drawables;
using Otter.Utility.MonoGame;
using WormGame.Core;
using WormGame.Static;

namespace WormGame.GameObject
{
    public class Fruit : Poolable
    {
        private readonly Collision field;
        private readonly int width;
        private readonly int height;


        public Fruit(Config config) : base()
        {
            field = config.field;
            width = config.width;
            height = config.height;
            Image image = Image.CreateRectangle(config.size / 3, config.size / 10 * 9, Color.Red);
            Image image2 = Image.CreateRectangle(config.size / 10 * 9, config.size / 3, Color.Red);
            image.CenterOrigin();
            image2.CenterOrigin();
            AddGraphic(image);
            AddGraphic(image2);
        }


        public Fruit Spawn(int x, int y, Color color)
        {
            X = field.EntityX(x);
            Y = field.EntityY(y);
            field.Set(this, Position);
            return this;
        }


        public void Respawn()
        {
            field.Set(null, Position);
            Vector2 random = Random.ValidPosition(field, width, height);
            X = field.EntityX(Mathf.FastRound(random.X));
            Y = field.EntityY(Mathf.FastRound(random.Y));
            field.Set(this, Position);
        }
    }
}
