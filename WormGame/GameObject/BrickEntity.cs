using Otter.Graphics.Drawables;
using Otter.Utility.MonoGame;
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
            field = config.field;
            Image image = Image.CreateRectangle(config.size);
            AddGraphic(image);
            image.CenterOrigin();
        }


        public BrickEntity Spawn(Vector2 newPosition, Brick parent)
        {
            Parent = parent;
            Position = newPosition;
            field.Set(this, Position);
            return this;
        }


        public bool IsFalling()
        {
            return Parent.SoftDrop;
        }


        public override void Disable()
        {
            if (!Enabled) return;
            field.Set(null, Position);
            Parent = null;
            Enabled = false;
        }
    }
}
