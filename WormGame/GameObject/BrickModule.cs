using Otter.Graphics;
using Otter.Graphics.Drawables;
using Otter.Utility.MonoGame;
using WormGame.Core;
using WormGame.Pooling;

namespace WormGame.GameObject
{
    public class BrickModule : PoolableObject
    {
        public BrickModule Next { get; set; }

        public Image Graphic { get; private set; }

        public Vector2 Position { get { return position; } set { position = value; } }
        private Vector2 position;

        public override bool Enabled { get { return enabled; } set { enabled = value; Graphic.Visible = value; } }
        private bool enabled;


        public BrickModule(Config config)
        {
            Graphic = Image.CreateRectangle(config.imageSize);
            Graphic.Scale = (float)config.size / config.imageSize;
            Graphic.CenterOrigin();
        }

        public void CopyWorm(Worm worm, WormModule wormModule, Brick brick, Pooler<BrickModule> brickModules, Collision field)
        {
            Next = brickModules.Enable();
            Next.Graphic.X = wormModule.Target.X - worm.Position.X;
            Next.Graphic.Y = wormModule.Target.Y - worm.Position.Y;
            Next.Position = wormModule.Target;
            brick.AddGraphic(Next.Graphic);
            field.Set(brick, wormModule.Target);
            if (wormModule.Next != null)
                Next.CopyWorm(worm, wormModule.Next, brick, brickModules, field);
        }

        public Vector2 GetPosition(int n, int i = 0)
        {
            if (n == i)
                return Position;
            return Next.GetPosition(n, ++i);
        }

        public ref Vector2 GetPosition()
        {
            return ref position;
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
    }
}
