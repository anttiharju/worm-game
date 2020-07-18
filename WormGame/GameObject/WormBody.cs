using Otter.Graphics;
using Otter.Graphics.Drawables;
using Otter.Utility.MonoGame;
using WormGame.Core;
using WormGame.Pooling;

namespace WormGame.GameObject
{
    public class WormBody : PoolableObject
    {
        public WormBody Next { get; set; }
        public Image Graphic { get; set; }
        public Vector2 Target { get; set; }
        public Vector2 Direction { get; set; }

        private bool enabled;
        public override bool Enabled { get { return enabled; } set { enabled = value; Graphic.Visible = value; } }


        public WormBody(Config config)
        {
            Graphic = Image.CreateCircle(config.imageSize / 2);
            Graphic.Scale = (float)config.size / config.imageSize;
            Graphic.CenterOrigin();
        }

        public Vector2 GetTarget(int wantedIndex, int i = 0)
        {
            if (i == wantedIndex)
                return Target;
            return Next.GetTarget(wantedIndex, ++i);
        }

        public void TargetFollow(Vector2 newTarget)
        {
            if (Next != null)
                Next.TargetFollow(Target);
            Target = newTarget;
        }

        public void DirectionFollow(Vector2 newDirection)
        {
            if (Next != null)
                Next.DirectionFollow(Direction);
            Direction = newDirection;
        }

        public void SetColor(Color color)
        {
            if (Next != null)
                Next.SetColor(color);
            Graphic.Color = color;
        }

        public override void Disable()
        {
            if (Next != null)
                Next.Disable();
            Graphic.Visible = false;
            Enabled = false;
        }

        public void Follow(Vector2 positionDelta, float step)
        {
            Vector2 delta = Direction * step - positionDelta;
            Graphic.X += delta.X;
            Graphic.Y += delta.Y;
            if (Next != null)
                Next.Follow(positionDelta, step);
        }
    }
}
