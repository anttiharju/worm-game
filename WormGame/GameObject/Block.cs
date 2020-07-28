using Otter.Graphics;
using Otter.Utility.MonoGame;
using Otter.Graphics.Drawables;
using WormGame.Core;
using WormGame.Static;
using WormGame.Pooling;

namespace WormGame.GameObject
{
    /// @author Antti Harju
    /// @version 27.07.2020
    /// <summary>
    /// Brick class. Work in progress.
    /// </summary>
    public class Block : PoolableEntity
    {
        private readonly Collision collision;
        private readonly int size;
        private BlockModule firstModule;
        private Vector2[] positions;

        public new Image Graphic { get { return firstModule.Graphic ?? null; } }
        public override Color Color { get { return firstModule.Graphic.Color ?? null; } }


        public Block(Config config)
        {
            collision = config.collision;
            size = config.size;
        }


        public Block Spawn(Worm worm, Pooler<BlockModule> blockModules) // TODO: REMOVAL FROM COLLISION ON DISABLE.
        {
            SetPosition(worm.firstModule.Target);

            WormModule wormModule = worm.firstModule;
            BlockModule blockModule = null;
            Vector2 direction;
            Vector2 previousDirection = Vector2.Zero;
            collision.Set(this, wormModule.Target);
            bool disable = false;
            if (CheckNeighbours(worm.Color, collision.X(wormModule.Target.X), collision.Y(wormModule.Target.Y))) disable = true;

            int i = 0;
            do
            {
                // Get direction
                if (i < worm.Length - 1)
                    direction = wormModule.Next.Target - wormModule.Target;
                else
                {
                    if (i == 0)
                        direction = Vector2.UnitX * size;
                    else
                        direction = previousDirection;
                }
                // Create a new module or scale the old one
                if (direction != previousDirection)
                {
                    if (i == 0)
                    {
                        blockModule = NewModule(worm, blockModules, wormModule);
                        firstModule = blockModule;
                    }
                    else
                    {
                        blockModule.Next = NewModule(worm, blockModules, wormModule);
                        blockModule = blockModule.Next;
                    }
                }
                blockModule.Scale(direction, worm.Length);
                previousDirection = direction;
                if (wormModule.Next != null)
                {
                    wormModule = wormModule.Next;
                    collision.Set(this, wormModule.Target);
                    if (CheckNeighbours(worm.Color, collision.X(wormModule.Target.X), collision.Y(wormModule.Target.Y))) disable = true;
                }
                i++;
            }
            while (i < worm.Length - 1);
            if (disable)
                Disable();
            return this;
        }

        private BlockModule NewModule(Worm worm, Pooler<BlockModule> blockModules, WormModule wormModule)
        {
            BlockModule newModule = blockModules.Enable().Spawn(wormModule.Target - worm.firstModule.Target);
            newModule.Graphic.Color = Color.Random;
            AddGraphic(newModule.Graphic);
            return newModule;
        }


        public override void Disable()
        {
            if (firstModule != null)
                firstModule.Disable();
            ClearGraphics();
            Enabled = false;
        }


        private bool CheckNeighbours(Color color, int x, int y)
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
                    if (CheckNeighbour(color, currentX, currentY))
                        disable = true;
            }
            return disable;
        }


        private bool CheckNeighbour(Color color, int x, int y)
        {
            PoolableEntity cell = collision.Get(x, y);
            if (cell is Block block)
                if (Id != block.Id)
                    if (Help.Equal(block.Color, color))
                    {
                        block.Disable();
                        return true;
                    }
            return false;
        }
    }
}
