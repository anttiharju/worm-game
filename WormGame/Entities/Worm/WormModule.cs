using Otter.Graphics;
using Otter.Graphics.Drawables;
using Otter.Utility.MonoGame;
using WormGame.Core;
using WormGame.Pooling;

namespace WormGame.Entities
{
    public class WormModule : PoolableEntity
    {
        private readonly float halfSize;

        private Vector2 direction;
        public WormModule Next { get; set; }
        public float Scale { get { if (direction.X == 0) return Graphic.ScaleY; else return Graphic.ScaleX; } }


        public WormModule(Config config)
        {
            Graphic = Image.CreateRectangle(config.size);
            Graphic.CenterOrigin();
            halfSize = config.halfSize;
        }

        public WormModule Initialize(Vector2 position, Vector2 direction, Color color)
        {
            SetPosition(position);
            this.direction = direction;
            Graphic.Color = color;
            return this;
        }


        public void Grow()
        {
            if (direction.X == 0)
                Graphic.ScaleY++;
            else
                Graphic.ScaleX++;
            Move();
        }


        public void Move()
        {
            Position += direction * halfSize;
        }


        public Vector2 GetEnd()
        {
            return Position - halfSize * direction - Scale * halfSize * direction;
        }


        public void Shrink()
        {
            if (direction.X == 0)
                Graphic.ScaleY--;
            else
                Graphic.ScaleX--;
            Move();
        }


        public override void Disable(bool recursive = true)
        {
            base.Disable();
            Graphic.ScaleX = 1;
            Graphic.ScaleY = 1;
            direction.X = 0;
            direction.Y = 0;
            Next = null;
        }
    }
}