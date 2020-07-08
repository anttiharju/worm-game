using Otter.Utility.MonoGame;
using Otter.Graphics.Drawables;
using WormGame.Pooling;
using WormGame.Core;
using WormGame.Static;

namespace WormGame.GameObject
{
    class WormEntity : Poolable
    {
        private readonly int size;
        private readonly float step;
        private readonly Collision field;

        private Vector2 direction = new Vector2(0, 0);

        public WormEntity Next { get; set; }
        public Vector2 Direction { get { return direction; } set { if (Help.ValidateDirection(field, target, value * size)) direction = value; } }
        public Vector2 target;

        public WormEntity(Config config) : base()
        {
            size = config.size;
            step = config.wormStep;
            field = config.field;

            Image image = Image.CreateCircle(size / 2);
            AddGraphic(image);
            image.CenterOrigin();
        }

        public void DirectionFollow(Vector2 previousDirection)
        {
            if (Next != null)
                Next.DirectionFollow(direction);
            direction = previousDirection;
        }

        public void TargetFollow(Vector2 newTarget)
        {
            if (Next != null)
                Next.TargetFollow(target);
            target = newTarget;
        }

        public void Step()
        {
            Position += Direction * step;
        }
    }
}
