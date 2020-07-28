using Otter.Graphics;
using WormGame.Core;
using WormGame.Static;
using WormGame.Pooling;

namespace WormGame.GameObject
{
    /// @author Antti Harju
    /// @version 28.07.2020
    /// <summary>
    /// Block class.
    /// </summary>
    public class Block : PoolableEntity
    {
        private readonly Collision collision;

        private BlockModule lastModule;
        private int top;
        private int left;
        private int bottom;
        private int right;


        /// <summary>
        /// Block color.
        /// </summary>
        public new Color Color { get; private set; }


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="config">Configuration</param>
        public Block(Config config)
        {
            collision = config.collision;
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
            left = collision.Width;
            bottom = collision.Height;
            right = 0;

            SetBlockfield(worm, 1, true);

            for (int y = bottom; y <= top; y++)
                for (int x = left; x <= right; x++)
                    if (collision.blockField[x, y] == 1)
                    {
                        lastModule = blockModules.Enable().Spawn(this, collision.EntityX(x) - X, collision.EntityY(y) - Y);
                        collision.blockField[x, y] = 2;
                        ExpandBothX(x, y);
                        ExpandX(x, y);
                        ExpandY(x, y);
                    }
            bool disable = false;
            if (disable)
                Disable();
            SetBlockfield(worm, 0, false);
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
            int yScale = Mathf.FastRound(lastModule.Graphic.ScaleY);
            int xPos = x + Mathf.FastRound(lastModule.Graphic.ScaleX);
            if (xPos >= collision.Width) return false;
            for (int yPos = y; yPos < y + yScale; yPos++)
                if (collision.blockField[xPos, yPos] != 1)
                    return false;
            for (int yPos = y; yPos < y + yScale; yPos++)
                collision.blockField[xPos, yPos] = 2;
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
            int xScale = Mathf.FastRound(lastModule.Graphic.ScaleX);
            int yPos = y + Mathf.FastRound(lastModule.Graphic.ScaleY);
            if (yPos >= collision.Height) return false;
            for (int xPos = x; xPos < x + xScale; xPos++)
                if (collision.blockField[xPos, yPos] != 1)
                    return false;
            for (int xPos = x; xPos < x + xScale; xPos++)
                collision.blockField[xPos, yPos] = 2;
            lastModule.Graphic.ScaleY++;
            return true;
        }


        /// <summary>
        /// Copies worm position data to a buffer which is used to optimize blockModule usage.
        /// </summary>
        /// <param name="worm">Worm</param>
        /// <param name="n">1, used to keep track wheter or not that part of worm has been handled</param>
        /// <param name="getBorders">Get the borders for the area which we loop through</param>
        private void SetBlockfield(Worm worm, int n, bool getBorders)
        {
            WormModule wormModule = worm.firstModule;
            for (int i = 0; i < worm.Length; i++)
            {
                int x = collision.X(wormModule.Target.X);
                int y = collision.Y(wormModule.Target.Y);
                if (getBorders)
                {
                    collision.Set(this, x, y);
                    if (x < left) left = x;
                    if (y < bottom) bottom = y;
                    if (x > right) right = x;
                    if (y > top) top = y;
                }
                collision.blockField[x, y] = n;
                wormModule = wormModule.Next;
            }
        }


        /*private bool CheckNeighbours(Color color, int x, int y)
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
        }*/


        /// <summary>
        /// Disable entity.
        /// </summary>
        public override void Disable()
        {
            if (lastModule != null)
                lastModule.Disable();
            ClearGraphics();
            Enabled = false;
        }
    }
}
