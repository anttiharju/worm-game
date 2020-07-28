using Otter.Graphics;
using Otter.Graphics.Drawables;
using Otter.Utility.MonoGame;
using WormGame.Core;
using WormGame.Static;
using WormGame.Pooling;

namespace WormGame.GameObject
{
    public class BlockModule : PoolableObject
    {
        private readonly Collision collision;
        private readonly int size;

        private bool initialized;

        public Image Graphic { get; private set; }

        public BlockModule Next { get; set; }

        public override bool Enabled { get { return enabled; } set { enabled = value; Graphic.Visible = value; } }
        private bool enabled;


        public BlockModule(Config config)
        {
            collision = config.collision;
            size = config.size;
            Graphic = Image.CreateRectangle(config.size);
            Graphic.Scale = 0;
        }


        public BlockModule Spawn(Vector2 position)
        {
            Graphic.SetPosition(position);
            return this;
        }

        public void Scale(Vector2 direction, int wormLength)
        {
            direction.X = Mathf.Normalize(direction.X);
            direction.Y = Mathf.Normalize(direction.Y);
            if (!initialized)
            {
                if (direction.Y == 0)
                {
                    Graphic.OriginX = 0;
                    Graphic.OriginY = size / 2 * direction.Y + size / 2;
                    Graphic.X += -size / 2 * direction.X;
                    Graphic.Y += 0;
                    Graphic.ScaleX = direction.X;
                    Graphic.ScaleY = 1;
                }
                else
                {
                    Graphic.OriginX = size / 2 * direction.X + size / 2;
                    Graphic.OriginY = 0;
                    Graphic.X += 0;
                    Graphic.Y += -size / 2 * direction.Y;
                    Graphic.ScaleX = 1;
                    Graphic.ScaleY = direction.Y;
                }
                initialized = true;
            }
            if (wormLength > 1)
            {
                Graphic.ScaleX += direction.X;
                Graphic.ScaleY += direction.Y;
            }
        }

        public void SetCollision(Vector2 parentPosition)
        {
            int currentX = collision.X(parentPosition.X);
            int currentY = collision.Y(parentPosition.Y);
        }

        public override void Disable()
        {
            if (Next != null)
                Next.Disable();
            Enabled = false;
            initialized = false;
            Graphic.Scale = 0;
            Graphic.SetPosition(0, 0);
        }
    }
}
