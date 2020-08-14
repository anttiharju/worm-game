using Otter.Graphics;
using WormGame.Core;
using WormGame.Static;
using System;

namespace WormGame.Entities
{
    /// @author Antti Harju
    /// @version 14.08.2020
    /// <summary>
    /// Manages block spawning.
    /// </summary>
    public class BlockSpawner
    {
#if DEBUG
        private readonly bool disableBlocks;
        private readonly bool visualize;
#endif
        private readonly Collision collision;
        private readonly bool[,] optimizationBuffer;
        private readonly int width;
        private readonly int height;

        private BlockModule firstModule;
        private BlockModule lastModule;
        private Color color;
        private int top;
        private int left;
        private int bottom;
        private int right;


        /// <summary>
        /// Initialize spawner.
        /// </summary>
        /// <param name="config">Configuration</param>
        public BlockSpawner(Config config)
        {
            collision = config.collision;
            width = config.width;
            height = config.height;
            optimizationBuffer = new bool[width, height];
#if DEBUG
            disableBlocks = config.disableBlocks;
            visualize = config.visualizeBlockifying;
#endif
        }


        /// <summary>
        /// Spawns a block by spawning as many block modules as needed.
        /// </summary>
        /// <param name="worm">Worm</param>
        /// <param name="manager">Module pooler</param>
        /// <returns>Block</returns>
        public BlockModule Spawn(Worm worm, BlockManager manager)
        {
            color = worm.Color;
            bottom = height;
            left = width;
            top = 0;
            right = 0;
            if (InitBuffer(worm.firstModule, worm.Length))
            {
                ClearBuffer(worm.firstModule, worm.Length);
                return null;
            }
            for (int y = bottom; y <= top; y++)
                for (int x = left; x <= right; x++)
                    if (optimizationBuffer[x, y] == true)
                    {
                        optimizationBuffer[x, y] = false;
                        if (firstModule == null)
                        {
                            lastModule = manager.Enable().Initialize(color, x, y);
                            firstModule = lastModule;
                        }
                        else
                        {
                            lastModule.Next = manager.Enable().Initialize(color, x, y);
                            lastModule = lastModule.Next;
                        }
                        lastModule.First = firstModule;
                        if (ExpandBothX(x, y))
                            ExpandY(x, y);
                        else
                            ExpandX(x, y);
                        lastModule.Add();
                    }
            firstModule = null;
#if DEBUG
            if (visualize)
                Visualize();
#endif
            return firstModule;
        }


        /// <summary>
        /// Scales current block module recursively in both directions (horizontal part).
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
        /// Scales current block module recursively in both directions (vertical part).
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
        /// Scales current block module recursively in the horizontal direction.
        /// </summary>
        /// <param name="x">Horizontal field position</param>
        /// <param name="y">Vertical field position</param>
        private void ExpandX(int x, int y)
        {
            if (ScaleX(x, y))
                ExpandX(x, y);
        }


        /// <summary>
        /// Scales current block module recursively in the vertical direction.
        /// </summary>
        /// <param name="x">Horizontal field position</param>
        /// <param name="y">Vertical field position</param>
        private void ExpandY(int x, int y)
        {
            if (ScaleY(x, y))
                ExpandY(x, y);
        }


        /// <summary>
        /// Scales current block module horizontally.
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
                if (optimizationBuffer[xPos, yPos] != true)
                    return false;
            for (int yPos = y; yPos < y + yScale; yPos++)
                optimizationBuffer[xPos, yPos] = false;
            lastModule.Width++;
            return true;
        }


        /// <summary>
        /// Scales current block module vertically.
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
                if (optimizationBuffer[xPos, yPos] != true)
                    return false;
            for (int xPos = x; xPos < x + xScale; xPos++)
                optimizationBuffer[xPos, yPos] = false;
            lastModule.Height++;
            return true;
        }


        /// <summary>
        /// Initializes optimization buffer and checks for neighbours of the same color.
        /// </summary>
        /// <param name="wormModule">Worm.firstModule</param>
        /// <param name="wormLength">Worms length</param>
        /// <returns>Abort spawn process</returns>
        private bool InitBuffer(WormModule wormModule, int wormLength)
        {
            for (int i = 0; i < wormLength; i++)
            {
                int x = collision.X(wormModule.Target.X);
                int y = collision.Y(wormModule.Target.Y);
                optimizationBuffer[x, y] = true;
                if (CheckNeighbours(x, y)) return true;
                if (x < left) left = x;
                if (y < bottom) bottom = y;
                if (x > right) right = x;
                if (y > top) top = y;
                wormModule = wormModule.Next;
            }
            return false;
        }


        /// <summary>
        /// Clears optimization buffer.
        /// </summary>
        /// <param name="wormModule">Worm.firstModule</param>
        /// <param name="wormLength">Worms length</param>
        private void ClearBuffer(WormModule wormModule, int wormLength)
        {
            for (int i = 0; i < wormLength; i++)
            {
                optimizationBuffer[collision.X(wormModule.Target.X), collision.Y(wormModule.Target.Y)] = false;
                wormModule = wormModule.Next;
            }
        }


        /// <summary>
        /// Check adjacent neighbours. (not diagonal ones)
        /// </summary>
        /// <param name="x">Horizontal field position</param>
        /// <param name="y">Vertical field position</param>
        /// <returns>Stop spawn process</returns>
        private bool CheckNeighbours(int x, int y)
        {
            int[] xPositions = { -1, 1, 0, 0 };
            int[] yPositions = { 0, 0, -1, 1 };

            bool stop = false;
            for (int i = 0; i < xPositions.Length; i++)
            {
                int currentX = x + xPositions[i];
                int currentY = y + yPositions[i];
                if (currentX >= 0 &&
                    currentY >= 0 &&
                    currentX < width &&
                    currentY < height)
                    if (CheckNeighbour(currentX, currentY))
                        stop = true;
            }
            return stop;
        }


        /// <summary>
        /// Compares neighbouring blocks' color to current blocks' color
        /// </summary>
        /// <param name="x">Horizontal field position</param>
        /// <param name="y">Vertical field position</param>
        /// <returns>Is neighbour the same color</returns>
        private bool CheckNeighbour(int x, int y)
        {
#if DEBUG
            if (!disableBlocks)
                return false;
#endif
            Object cell = collision.Get(x, y);
            if (cell is BlockModule module)
                if (Help.Equal(module.Color, color))
                {
                    module.Disable();
                    return true;
                }
            return false;
        }
#if DEBUG
        /// <summary>
        /// Visualizes optimization buffer.
        /// </summary>
        public void Visualize()
        {
            for (int y = 0; y < height; y++)
            {
                Console.CursorTop = y + 1;
                System.Text.StringBuilder line = new System.Text.StringBuilder(width);
                for (int x = 0; x < width; x++)
                {
                    if (optimizationBuffer[x, y])
                        line.Append('x');
                    else
                        line.Append('.');
                }
                Console.WriteLine(line.ToString());
            }
            Console.CursorLeft = 0;
            Console.CursorTop = height + 1;
        }
#endif
    }
}
