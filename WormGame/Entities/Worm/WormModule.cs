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

        private bool horizontal;
        private Vector2 direction;
        private Vector2 target;

        public Color Color { get { return Graphic.Color; } set { Graphic.Color = value; } }

        public WormModule(Config config)
        {
            Graphic = Image.CreateRectangle(config.size);
            step = config.wormStep;
            size = config.size;
            halfSize = config.halfSize;
        }

        public WormModule Initialize(Vector2 direction, Vector2 position, Color color)
        {
            this.direction = direction;
            SetPosition(position);
            Graphic.Scale = 1;
            Color = color;
            if (direction.Y != 0) /// UGLY
            {

                if (direction.Y > 0)
                {
                    Graphic.SetOrigin(halfSize, size);
                    Y += halfSize;
                }
                else
                {
                    Graphic.SetOrigin(halfSize, 0);
                    Y -= halfSize;
                }
            }
            else
            {
                if (direction.X > 0)
                {
                    Graphic.SetOrigin(size, halfSize);
                    X += halfSize;
                }
                else
                {
                    Graphic.SetOrigin(0, halfSize);
                    X -= halfSize;
                }
                horizontal = true;
            }
            return this;
        }


        public Vector2 GetEnd()
        {
            Vector2 end = Position;
            if (horizontal)
                end.X -= halfSize;
            else
                end.Y -= halfSize;
            return end;
        }


        public void Grow()
        {
            Position += direction * size;
            if (horizontal)
                Graphic.ScaleX++;
            else
                Graphic.ScaleY++;
        }


        public void Move()
        {

        }


        public void Flip()
        {

        }

        public void Shrink()
        {

        }


        public override void Update()
        {
        }


        public override void Disable(bool recursive = true)
        {
            base.Disable();
            Graphic.SetOrigin(0, 0);
            horizontal = false;
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