using Otter.Graphics.Drawables;
using WormGame.Core;
using WormGame.Pooling;

namespace WormGame.GameObject
{
    public class BrickEntity : Poolable
    {
        private readonly Collision field;

        public Brick Parent { get; set; }

        public BrickEntity(Config config) : base()
        {
            this.field = config.field;
            Image image = Image.CreateRectangle(config.size);
            AddGraphic(image);
            image.CenterOrigin();
        }

        public override void Disable()
        {
            field.Set(null, Position);
            Enabled = false;
        }
    }
}
