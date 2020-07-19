using Otter.Graphics;
using Otter.Graphics.Drawables;
using Otter.Utility.MonoGame;
using WormGame.Core;
using WormGame.Pooling;

namespace WormGame.GameObject
{
    public class BrickModule : PoolableObject
    {
        private Collision field;

        public BrickModule Next { get; set; }

        public Image Graphic { get; private set; }

        public Vector2 Position { get { return position; } set { position = value; } }
        private Vector2 position;

        public override bool Enabled { get { return enabled; } set { enabled = value; Graphic.Visible = value; } }
        private bool enabled;


        public BrickModule(Config config)
        {
            field = config.field;
            Graphic = Image.CreateRectangle(config.imageSize);
            Graphic.Scale = (float)config.size / config.imageSize;
            Graphic.CenterOrigin();
        }

        public bool CopyWorm(Worm worm, WormModule wormModule, Brick brick, Pooler<BrickModule> brickModules, bool disable = false)
        {
            Next = brickModules.Enable();
            Next.Graphic.X = wormModule.Target.X - worm.Position.X;
            Next.Graphic.Y = wormModule.Target.Y - worm.Position.Y;
            Next.Position = wormModule.Target;
            brick.AddGraphic(Next.Graphic);
            field.Set(brick, wormModule.Target);
            if (!disable)
                disable = CheckNeighbours(brick, worm.Color, field.X(wormModule.Target.X), field.Y(wormModule.Target.Y));
            if (wormModule.Next != null)
                return Next.CopyWorm(worm, wormModule.Next, brick, brickModules, disable);
            return disable;
        }

        private bool CheckNeighbours(Brick parent, Color color, int positionX, int positionY)
        {
            for (int x = -1; x <= 1; x++)
                for (int y = -1; y <= 1; y++)
                {
                    int checkX = positionX + x;
                    int checkY = positionY + y;
                    if (field.Check(checkX, checkY) == 2)
                    {
                        Brick brick = (Brick)field.Get(checkX, checkY);
                        if (brick.Id != parent.Id && ColorCheck(color, brick.Color))
                        {
                            brick.Disable();
                            return true;
                        }
                    }
                }
            return false;
        }

        private bool ColorCheck(Color a, Color b)
        {
            if (a.R == b.R && a.G == b.G && a.B == b.B)
                return true;
            return false;
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

        public void Disable(Vector2 parentPosition)
        {
            field.Set(null, parentPosition.X + Graphic.X, parentPosition.Y + Graphic.Y);
            Graphic.Visible = false;
            Enabled = false;
            if (Next != null)
                Next.Disable(parentPosition);
        }
    }
}
