using Otter.Graphics;
using WormGame.Core;
using WormGame.Static;
using WormGame.Pooling;
using System;

namespace WormGame.Entities
{
    /// @author Antti Harju
    /// @version 28.07.2020
    /// <summary>
    /// Block class.
    /// </summary>
    public class Block : Poolable
    {
#if DEBUG
        private readonly bool disableBlocks;
#endif
        private readonly Collision collision;
        private readonly BlockManager manager;
        private readonly int width;
        private readonly int height;

        private BlockModule firstModule;
        private BlockModule lastModule;
        private int top;
        private int left;
        private int bottom;
        private int right;

        /// <summary>
        /// Block color.
        /// </summary>
        /// TODO: use firstmodule or worm.color?
        public Color Color { get; private set; }


        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="config">Configuration</param>
        public Block(Config config, BlockManager manager)
        {
            collision = config.collision;
            this.manager = manager;
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
        /// <param name="manager">Module pooler</param>
        /// <returns>Block</returns>
        public Block Spawn(Worm worm, BlockManager manager)
        {
            Color = worm.Color;
            bottom = height;
            left = width;
            top = 0;
            right = 0;
            if (SetBlockBuffer(worm))
            {
                SetBlockBuffer(worm, 0, false);
                Disable();
                return null;
            }
            for (int y = bottom; y <= top; y++)
                for (int x = left; x <= right; x++)
                    if (manager.blockBuffer[x, y] == 1)
                    {
                        if (firstModule == null)
                        {
                            lastModule = manager.modules.Enable().Spawn(x, y);
                            firstModule = lastModule;
                        }
                        else
                        {
                            lastModule.Next = manager.modules.Enable().Spawn(x, y);
                            lastModule = lastModule.Next;
                        }
                        if (ExpandBothX(x, y))
                            ExpandY(x, y);
                        else
                            ExpandX(x, y);
                        lastModule.Set(this);
                    }
            SetBlockBuffer(worm, 0, false, true);
            return this;
        }


        /// <summary>
        /// Scales module recursively in both directions (horizontal part).
        /// </summary>
        /// <param name="x">Horizontal field position</param>
        /// <param name="y">Vertical field position</param>
        /// <returns>ScaleX failure</returns>
        private bool ExpandBothX(int x, int y)
        {
            if (ScaleX(x, y))
                return ExpandBothY(x, y);
            return true;
        }


        /// <summary>
        /// Scales module recursively in both directions (vertical part).
        /// </summary>
        /// <param name="x">Horizontal field position</param>
        /// <param name="y">Vertical field position</param>
        /// <returns>ScaleX failure</returns>
        private bool ExpandBothY(int x, int y)
        {
            if (ScaleY(x, y))
                return ExpandBothX(x, y);
            return false;
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
            int yScale = lastModule.Height;
            int xPos = x + lastModule.Width;
            if (xPos >= width) return false;
            for (int yPos = y; yPos < y + yScale; yPos++)
                if (manager.blockBuffer[xPos, yPos] != 1)
                    return false;
            for (int yPos = y; yPos < y + yScale; yPos++)
                manager.blockBuffer[xPos, yPos] = 2;
            lastModule.Width++;
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
            int xScale = lastModule.Width;
            int yPos = y + lastModule.Height;
            if (yPos >= height) return false;
            for (int xPos = x; xPos < x + xScale; xPos++)
                if (manager.blockBuffer[xPos, yPos] != 1)
                    return false;
            for (int xPos = x; xPos < x + xScale; xPos++)
                manager.blockBuffer[xPos, yPos] = 2;
            lastModule.Height++;
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
                manager.blockBuffer[x, y] = n;
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
            Object cell = collision.Get(x, y);
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
        }
    }
}
