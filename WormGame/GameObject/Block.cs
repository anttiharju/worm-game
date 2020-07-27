using Otter.Graphics;
using Otter.Utility.MonoGame;
using Otter.Graphics.Drawables;
using WormGame.Core;
using WormGame.Static;
using WormGame.Pooling;

namespace WormGame.GameObject
{
    /// @author Antti Harju
    /// @version 18.07.2020
    /// <summary>
    /// Brick class. Work in progress.
    /// </summary>
    public class Block : PoolableEntity
    {
        private readonly Collision collision;
        private readonly int size;
        private BlockModule lastModule;

        public new Image Graphic { get { return lastModule.Graphic ?? null; } }
        public override Color Color { get { return lastModule.Graphic.Color ?? null; } set { lastModule.SetColor(value); } }

        public Block(Config config)
        {
            collision = config.collision;
            size = config.size;
        }

        public Block Spawn(Worm worm, Pooler<BlockModule> blockModules)
        {
            SetPosition(worm.Target.X, worm.Target.Y);

            WormModule wormModule = worm.firstModule;
            collision.Set(this, wormModule.Target);
            Vector2 direction = wormModule.Next.Target - wormModule.Target;
            Vector2 previousDirection = direction;
            lastModule = NewModule(worm, blockModules, wormModule, direction, -1);
            for (int i = 0; i < worm.Length - 1; i++)
            {
                direction = wormModule.Next.Target - wormModule.Target;
                if (direction == previousDirection)
                {
                    if (direction.Y == 0)
                        Graphic.ScaleX += Mathf.Normalize(direction.X);
                    else
                        Graphic.ScaleY += Mathf.Normalize(direction.Y);
                }
                else
                {
                    lastModule.Next = NewModule(worm, blockModules, wormModule, direction);
                    lastModule = lastModule.Next;
                }
                previousDirection = direction;
                wormModule = wormModule.Next;
                collision.Set(this, wormModule.Target);
            }
            return this;
        }


        private BlockModule NewModule(Worm worm, Pooler<BlockModule> blockModules, WormModule wormModule, Vector2 direction, int first = 1)
        {
            BlockModule newModule = blockModules.Enable().Spawn(wormModule.Target - worm.Target, direction, first);
            newModule.SetColor(Random.Color);
            AddGraphic(newModule.Graphic);
            return newModule;
        }

        public override void Disable()
        {
            if (lastModule != null)
                lastModule.Disable(Position);
            ClearGraphics();
            Enabled = false;
        }


        /* Port to Block from BlockModule
        private bool CheckNeighbours(Color color, int parentId, int x, int y)
        {
            int[] xPositions = { -1, 1, 0, 0 };
            int[] yPositions = { 0, 0, -1, 1 };

            bool disable = false;
            for (int i = 0; i < xPositions.Length; i++)
            {
                int currentX = x + xPositions[i];
                int currentY = y + yPositions[i];
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
        }*/
    }
}
