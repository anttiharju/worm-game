using Otter.Utility.MonoGame;
using Otter.Graphics.Drawables;
using WormGame.Pooling;
using WormGame.Core;
using WormGame.Static;

namespace WormGame.GameObject
{
    public class WormEntity : Poolable
    {
        private readonly int size;
        private readonly float step;
        private readonly Collision field;
        private Vector2 direction = new Vector2(0, 0);

        public Vector2 target;
        public WormEntity Next { get; set; }
        public Vector2 Direction { get { return direction; } set { if (Help.ValidateDirection(field, target, value * size)) direction = value; } }
        public bool Stuck { get; set; }

        public WormEntity(Config config) : base()
        {
            size = config.size;
            step = config.wormStep;
            field = config.field;

            Image image = Image.CreateCircle(size / 2);
            AddGraphic(image);
            image.CenterOrigin();
        }

        public void DirectionFollow(Vector2 newDirection)
        {
            if (Next != null)
                Next.DirectionFollow(direction);
            direction = newDirection;
        }

        public void TargetFollow(Vector2 newTarget)
        {
            if (Next != null)
                Next.TargetFollow(target);
            target = newTarget;
        }

        public void Step()
        {
            if (!Stuck)
                Position += Direction * step;
        }
    }
}
