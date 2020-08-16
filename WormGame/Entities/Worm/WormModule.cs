using Otter.Graphics;
using Otter.Graphics.Drawables;
using Otter.Utility.MonoGame;
using WormGame.Core;
using WormGame.Pooling;
using WormGame.Static;

namespace WormGame.Entities
{
    public class WormModule : PoolableEntity
    {
        private readonly float size;
        private readonly float halfSize;
        private readonly float step;

        private Vector2 direction;

        public WormModule Next { get; set; }
        public float Scale { get { if (direction.X == 0) return Graphic.ScaleY; else return Graphic.ScaleX; } }


        public WormModule(Config config)
        {
            Graphic = Image.CreateRectangle(config.size);
            Graphic.CenterOrigin();
            step = config.wormStep;
            size = config.size;
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


        public override void Update()
        {
        }


        public override void Disable(bool recursive = true)
        {
            base.Disable();
            Graphic.Scale = 1;
            X = 0;
            Y = 0;
        }

        private int Normalize(float number)
        {
            if (number < 0) return -1;
            return 1;
        }
    }
}