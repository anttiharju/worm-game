using Otter.Graphics;
using WormGame.Core;
using WormGame.Static;
using WormGame.Pooling;

namespace WormGame.Entities
{
    /// @author Antti Harju
    /// @version 28.07.2020
    /// <summary>
    /// Block class.
    /// </summary>
    public class Block : PoolableEntity
    {
#if DEBUG
        private readonly bool disableBlocks;
#endif
        private readonly Collision collision;

        private BlockModule firstModule;
        private BlockModule lastModule;
        private int top;
        private int left;
        private int bottom;
        private int right;
        private int width;
        private int height;

        /// <summary>
        /// Block color.
        /// </summary>
        public Color Color { get; private set; }


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="config">Configuration</param>
        public Block(Config config)
        {
            collision = config.collision;
            width = config.width;
            height = config.height;
#if DEBUG
            disableBlocks = config.disableBlocks;
#endif
        }


        /// <summary>
        /// Spawns the block and optimizes module usage.
        /// </summary>
        /// <param name="worm">Worm</param>
        /// <param name="blockModules">Module pooler</param>
        /// <returns>Block</returns>
        public Block Spawn(Worm worm, Pooler<BlockModule> blockModules)
        {
            SetPosition(worm.firstModule.Target);
            Color = worm.Color;
            top = 0;
            left = width;
            bottom = height;
            right = 0;
            if (SetBlockBuffer(worm))
            {
                SetBlockBuffer(worm, 0, false);
                Disable();
                return null;
            }
            for (int y = top; y >= bottom; y--)
                for (int x = left; x <= right; x++)
                    if (collision.blockBuffer[x, y] == 1)
                    {
                        if (firstModule == null)
                        {
                            lastModule = blockModules.Enable().Spawn(this, collision.EntityX(x) - X, collision.EntityY(y) - Y);
                            firstModule = lastModule;
                        }
                        else
                        {
                            lastModule.Next = blockModules.Enable().Spawn(this, collision.EntityX(x) - X, collision.EntityY(y) - Y);
                            lastModule = lastModule.Next;
                        }
                        collision.blockBuffer[x, y] = 2;
                        ExpandBothX(x, y);
                        ExpandX(x, y);
                        ExpandY(x, y);
                    }
            SetBlockBuffer(worm, 0, false, true);
            return this;
        }


        /// <summary>
        /// Scales module recursively in both directions (horizontal part).
        /// </summary>
        /// <param name="x">Horizontal field position</param>
        /// <param name="y">Vertical field position</param>
        private void ExpandBothX(int x, int y)
        {
            if (ScaleX(x, y))
                ExpandBothY(x, y);
        }


        /// <summary>
        /// Scales module recursively in both directions (vertical part).
        /// </summary>
        /// <param name="x">Horizontal field position</param>
        /// <param name="y">Vertical field position</param>
        private void ExpandBothY(int x, int y)
        {
            if (ScaleY(x, y))
                ExpandBothX(x, y);
        }


        /// <summary>
        /// Scales module recursively in the horizontal direction.
        /// </summary>
        /// <param name="x">Horizontal field position</param>
        /// <param name="y">Vertical field position</param>
        private void ExpandX(int x, int y)
        {
            if (ScaleX(x, y))
                ExpandX(x, y);
        }


        /// <summary>
        /// Scales module recursively in the vertical direction.
        /// </summary>
        /// <param name="x">Horizontal field position</param>
        /// <param name="y">Vertical field position</param>
        private void ExpandY(int x, int y)
        {
            if (ScaleY(x, y))
                ExpandY(x, y);
        }


        /// <summary>
        /// Scales module horizontally.
        /// </summary>
        /// <param name="x">Horizontal field position</param>
        /// <param name="y">Vertical field position</param>
        /// <returns>Was the module scaled or not</returns>
        private bool ScaleX(int x, int y)
        {
            int yScale = FastMath.Round(lastModule.Graphic.ScaleY);
            int xPos = x + FastMath.Round(lastModule.Graphic.ScaleX);
            if (xPos >= width) return false;
            for (int yPos = y; yPos > y - yScale; yPos--)
                if (collision.blockBuffer[xPos, yPos] != 1)
                    return false;
            for (int yPos = y; yPos > y - yScale; yPos--)
                collision.blockBuffer[xPos, yPos] = 2;
            lastModule.Graphic.ScaleX++;
            return true;
        }


        /// <summary>
        /// Scales module vertically.
        /// </summary>
        /// <param name="x">Horizontal field position</param>
        /// <param name="y">Vertical field position</param>
        /// <returns>Was the module scaled or not</returns>
        private bool ScaleY(int x, int y)
        {
            int xScale = FastMath.Round(lastModule.Graphic.ScaleX);
            int yPos = y - FastMath.Round(lastModule.Graphic.ScaleY);
            if (yPos < 0) return false;
            for (int xPos = x; xPos < x + xScale; xPos++)
                if (collision.blockBuffer[xPos, yPos] != 1)
                    return false;
            for (int xPos = x; xPos < x + xScale; xPos++)
                collision.blockBuffer[xPos, yPos] = 2;
            lastModule.Graphic.ScaleY++;
            return true;
        }


        /// <summary>
        /// Copies worm position data to a buffer which is used to optimize blockModule usage.
        /// </summary>
        /// <param name="worm">Worm</param>
        /// <param name="n">Keeps track wheter or not that part of worm has been handled</param>
        /// <param name="initialize">Get for loop edges and destroy neighbours</param>
        /// <param name="setCollision">Set to collision field</param>
        /// <returns>To disable or not based on neighbours</returns>
        private bool SetBlockBuffer(Worm worm, int n = 1, bool initialize = true, bool setCollision = false)
        {
            WormModule wormModule = worm.firstModule;
            bool disable = false;
            for (int i = 0; i < worm.Length; i++)
            {
                int x = collision.X(wormModule.Target.X);
                int y = collision.Y(wormModule.Target.Y);
                collision.blockBuffer[x, y] = n;
                if (setCollision)
                    collision.Set(this, x, y);
                if (initialize)
                {
#if DEBUG
                    if (disableBlocks)
#endif
                        if (CheckNeighbours(x, y)) disable = true;
                    if (x < left) left = x;
                    if (y < bottom) bottom = y;
                    if (x > right) right = x;
                    if (y > top) top = y;
                }
                wormModule = wormModule.Next;
            }
            return disable;
        }


        /// <summary>
        /// Check neighbouring tiles (four).
        /// </summary>
        /// <param name="x">Horizontal field position</param>
        /// <param name="y">Vertical field position</param>
        /// <returns>To disable or not</returns>
        private bool CheckNeighbours(int x, int y)
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
                    currentX < width &&
                    currentY < height)
                    if (CheckNeighbour(currentX, currentY))
                        disable = true;
            }
            return disable;
        }


        /// <summary>
        /// Check a neighbouring tile.
        /// </summary>
        /// <param name="x">Horizontal field position</param>
        /// <param name="y">Vertical field position</param>
        /// <returns>Is neighbour the same color</returns>
        private bool CheckNeighbour(int x, int y)
        {
            PoolableEntity cell = collision.Get(x, y);
            if (cell is Block block)
                if (!ReferenceEquals(this, block))
                    if (Help.Equal(block.Color, Color))
                    {
                        block.Disable();
                        return true;
                    }
            return false;
        }


        /// <summary>
        /// Disable entity.
        /// </summary>
        /// <param name="recursive">Disable recursively. False only when disabling is done by pooler.</param>
        public override void Disable(bool recursive = true)
        {
            base.Disable();
            if (recursive && firstModule != null)
                firstModule.Disable();
            firstModule = null;
            ClearGraphics();
        }
    }
}
