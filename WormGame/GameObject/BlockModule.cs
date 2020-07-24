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

        public BlockModule Next { get; set; }

        public Vector2 Target { get; private set; }

        public Image Graphic { get; private set; }

        public override bool Enabled { get { return enabled; } set { enabled = value; Graphic.Visible = value; } }
        private bool enabled;


        public BlockModule(Config config)
        {
            collision = config.collision;
            Graphic = Image.CreateRectangle(config.size);
            Graphic.CenterOrigin();
        }

        public void CloneWorm(Worm worm, WormModule wormModule, Block parent, Pooler<BlockModule> brickModules, int currentLength, int i = 1)
        {
            Target = wormModule.Target;
            Graphic.X = wormModule.Target.X - worm.Position.X;
            Graphic.Y = wormModule.Target.Y - worm.Position.Y;
            Graphic.Color = worm.Color;

            if (CheckNeighbours(worm.Color, parent.Id, collision.X(wormModule.Target.X), collision.Y(wormModule.Target.Y)))
            {
                parent.Disable();
                return;
            }

            parent.AddGraphic(Graphic);
            collision.Set(parent, wormModule.Target);
            if (i < currentLength)
            {
                Next = brickModules.Enable();
                Next.CloneWorm(worm, wormModule.Next, parent, brickModules, currentLength, ++i);
            }
        }

        private bool CheckNeighbours(Color color, int parentId, int x, int y)
        {
            int[] xLoop = { -1, 1, 0, 0 };
            int[] yLoop = { 0, 0, -1, 1 };

            bool disable = false;
            for (int i = 0; i < xLoop.Length; i++)
            {
                int currentX = x + xLoop[i];
                int currentY = y + yLoop[i];
                if (currentX >= 0 &&
                    currentY >= 0 &&
                    currentX < collision.Width &&
                    currentY < collision.Height)
                    if (CheckNeighbour(color, parentId, currentX, currentY))
                        disable = true;
            }
            return disable;
        }

        private bool CheckNeighbour(Color color, int parentId, int x, int y)
        {
            PoolableEntity cell = collision.Get(x, y);
            if (cell is Block block)
                if (parentId != block.Id)
                    if (Help.Equal(block.Color, color))
                    {
                        block.Disable();
                        return true;
                    }
            return false;
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
            Next = null;
            Graphic.X = 0;
            Graphic.Y = 0;
        }
    }
}
