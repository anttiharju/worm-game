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

        public Image Graphic { get; private set; }

        public BlockModule Next { get; set; }

        public override bool Enabled { get { return enabled; } set { enabled = value; Graphic.Visible = value; } }
        private bool enabled;


        public BlockModule(Config config)
        {
            collision = config.collision;
            size = config.size;
            Graphic = Image.CreateRectangle(config.size);
        }


        public BlockModule Spawn(Vector2 position, Vector2 direction, int first = 1)
        {
            Graphic.SetPosition(position);
            if (direction.Y == 0)
            {
                Graphic.OriginX = 0;
                Graphic.OriginY = size / 2 * Mathf.Normalize(direction.Y) + size / 2;
                Graphic.X += size / 2 * Mathf.Normalize(direction.X) * first;
                Graphic.Y += 0;
                Graphic.ScaleX = Mathf.Normalize(direction.X);
                Graphic.ScaleY = 1;
            }
            else
            {
                Graphic.OriginX = size / 2 * Mathf.Normalize(direction.X) + size / 2;
                Graphic.OriginY = 0;
                Graphic.X += 0;
                Graphic.Y += size / 2 * Mathf.Normalize(direction.Y) * first;
                Graphic.ScaleX = 1;
                Graphic.ScaleY = Mathf.Normalize(direction.Y);
            }
            return this;
        }


        public void SetOrigin(int originX, int originY, int offsetX, int offsetY)
        {
            Graphic.SetOrigin(originX, originY);
            Graphic.SetPosition(offsetX, offsetY);
        }


        public void SetColor(Color color)
        {
            if (Next != null)
                Next.SetColor(color);
            Graphic.Color = color;
        }

        public void Disable(Vector2 parentPosition)
        {
            collision.Set(null, parentPosition.X + Graphic.X, parentPosition.Y + Graphic.Y);
            if (Next != null)
                Next.Disable(parentPosition);
            Disable();
        }

        public override void Disable()
        {
            Enabled = false;
            Graphic.X = 0;
            Graphic.Y = 0;
            //Graphic.Scale = 0;
        }
    }
}
