using System;
using Otter.Core;
using Otter.Graphics;
using Otter.Graphics.Drawables;
using WormGame.Core;
using WormGame.Static;
using WormGame.Pooling;

namespace WormGame.Entities
{
    /// @author Antti Harju
    /// @version v0.5
    /// <summary>
    /// Custom pooler that manages everything related to blocks. Uses a tilemap for rendering.
    /// </summary>
    public class Blocks : Pooler<BlockModule>
    {
        public readonly BlockSpawner blockSpawner;

        private readonly Eraser eraser;
        private readonly Tilemap tilemap;


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="settings">Settings</param>
        public Blocks(Settings settings, Game game, WormScene scene) : base(settings.moduleAmount)
        {
            tilemap = settings.tilemap;
            eraser = new Eraser(settings, game, scene);
            blockSpawner = new BlockSpawner(settings, eraser);

            for (int i = 0; i < settings.moduleAmount; i++)
            {
                BlockModule current = new BlockModule(settings, this);
                current.Disable(false);
                pool[i] = current;
            }
        }


        /// <summary>
        /// Add module to tilemap.
        /// </summary>
        /// <param name="module">Module</param>
        public void Add(BlockModule module)
        {
            // Replace module.Color with Color.Random to see how blocks are formed.
            tilemap.SetRect(module.X, module.Y, module.Width, module.Height, module.Color, "");
        }


        /// <summary>
        /// Clear module from tilemap.
        /// </summary>
        /// <param name="module">Module</param>
        public void Clear(BlockModule module)
        {
            tilemap.ClearRect(module.X, module.Y, module.Width, module.Height);
        }


        /// <summary>
        /// Clear tilemap and reset eraser and block module pooler.
        /// </summary>
        public override void Reset()
        {
            tilemap.ClearAll();
            eraser.Reset();
            base.Reset();
        }


        /// <summary>
        /// Turn a worm into a block.
        /// </summary>
        /// <param name="worm">Worm to transform</param>
        /// <returns>Block</returns>
        public BlockModule SpawnBLock(Worm worm)
        {
            return blockSpawner.Spawn(worm, this);
        }
    }


    /// @author Antti Harju
    /// @version v0.5
    /// <summary>
    /// Manage block spawning.
    /// </summary>
    public class BlockSpawner
    {
        private readonly Collision collision;
        private readonly Eraser eraser;
        private readonly int width;
        private readonly int height;
        private readonly bool[,] optimizationBuffer;
        private readonly bool disableBlocks;
#if DEBUG
        private readonly bool visualize;
#endif

        private BlockModule firstModule;
        private BlockModule lastModule;
        private Color color;
        private int top;
        private int left;
        private int bottom;
        private int right;


        /// <summary>
        /// Constructor. Initializes spawner with correct values.
        /// </summary>
        /// <param name="settings">Settings</param>
        public BlockSpawner(Settings settings, Eraser eraser)
        {
            this.eraser = eraser;
            collision = settings.collision;
            width = settings.width;
            height = settings.height;
            optimizationBuffer = new bool[width, height];
            disableBlocks = settings.disableBlocks;
#if DEBUG
            visualize = settings.visualizeBlockSpawner;
#endif
        }


        /// <summary>
        /// Create a block by spawning multiple block modules.
        /// </summary>
        /// <param name="worm">Worm</param>
        /// <param name="manager">Block manager</param>
        /// <returns>Module</returns>
        public BlockModule Spawn(Worm worm, Blocks manager)
        {
            color = worm.Color;
            bottom = height;
            left = width;
            top = 0;
            right = 0;
            bool spawn = InitBuffer(worm.firstModule, worm.Length);
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
                        eraser.Erase(lastModule);
                        if (spawn)
                            lastModule.Spawn();
                    }
            if (!spawn)
                firstModule.Disable();
            firstModule = null;
#if DEBUG
            if (visualize)
                Visualize();
#endif
            return firstModule;
        }


        /// <summary>
        /// Scale current block module recursively in both directions. This is the horizontal part of the algorithm.
        /// </summary>
        /// <param name="x">Horizontal field position</param>
        /// <param name="y">Vertical field position</param>
        /// <returns>Horizontal scaling success</returns>
        private bool ExpandBothX(int x, int y)
        {
            if (ScaleX(x, y))
                return ExpandBothY(x, y);
            return true;
        }


        /// <summary>
        /// Scale current block module recursively in both directions. This is the vertical part of the algorithm.
        /// </summary>
        /// <param name="x">Horizontal field position</param>
        /// <param name="y">Vertical field position</param>
        /// <returns>Horizontal scaling success</returns>
        private bool ExpandBothY(int x, int y)
        {
            if (ScaleY(x, y))
                return ExpandBothX(x, y);
            return false;
        }


        /// <summary>
        /// Recursively scale current block module horizontally.
        /// </summary>
        /// <param name="x">Horizontal field position</param>
        /// <param name="y">Vertical field position</param>
        private void ExpandX(int x, int y)
        {
            if (ScaleX(x, y))
                ExpandX(x, y);
        }


        /// <summary>
        /// Recursively scale current block module vertically.
        /// </summary>
        /// <param name="x">Horizontal field position</param>
        /// <param name="y">Vertical field position</param>
        private void ExpandY(int x, int y)
        {
            if (ScaleY(x, y))
                ExpandY(x, y);
        }


        /// <summary>
        /// Scale current block module horizontally.
        /// </summary>
        /// <param name="x">Horizontal field position</param>
        /// <param name="y">Vertical field position</param>
        /// <returns>Success</returns>
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
        /// Scale current block module vertically.
        /// </summary>
        /// <param name="x">Horizontal field position</param>
        /// <param name="y">Vertical field position</param>
        /// <returns>Success</returns>
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
        /// Initialize optimization buffer and check if any of the neighbours has the same color.
        /// </summary>
        /// <param name="wormModule">Worm.firstModule</param>
        /// <param name="wormLength">Worm length</param>
        /// <returns>Spawn current block</returns>
        private bool InitBuffer(WormModule wormModule, int wormLength)
        {
            bool spawn = true;
            for (int i = 0; i < wormLength; i++)
            {
                int x = collision.X(wormModule.Target.X);
                int y = collision.Y(wormModule.Target.Y);
                optimizationBuffer[x, y] = true;
                if (FindNeighbors(x, y)) spawn = false;
                if (x < left) left = x;
                if (y < bottom) bottom = y;
                if (x > right) right = x;
                if (y > top) top = y;
                wormModule = wormModule.Next;
            }
            return spawn;
        }


        /// <summary>
        /// Find adjacent neighbors (not diagonally).
        /// </summary>
        /// <param name="x">Horizontal field position</param>
        /// <param name="y">Vertical field position</param>
        /// <returns>Stop spawn process</returns>
        private bool FindNeighbors(int x, int y)
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
                    if (CompareColors(currentX, currentY))
                        stop = true;
            }
            return stop;
        }


        /// <summary>
        /// Compare neighbouring module colors to current module color.
        /// </summary>
        /// <param name="x">Horizontal field position</param>
        /// <param name="y">Vertical field position</param>
        /// <returns>Is neighbour the same color</returns>
        private bool CompareColors(int x, int y)
        {
            if (!disableBlocks)
                return false;
            object cell = collision.Get(x, y);
            if (cell is BlockModule module)
                if (Colors.Equal(module.Color, color))
                {
                    module.Disable();
                    return true;
                }
            return false;
        }
#if DEBUG
        /// <summary>
        /// Visualize optimization buffer.
        /// </summary>
        public void Visualize()
        {
            System.Text.StringBuilder visualization = new System.Text.StringBuilder((width + 1) * height);
            for (int y = 0; y < height; y++)
            {
                visualization.Append("\n");
                for (int x = 0; x < width; x++)
                {
                    if (optimizationBuffer[x, y])
                        visualization.Append('x');
                    else
                        visualization.Append('.');
                }
            }
            Console.CursorTop = 0;
            Console.WriteLine(visualization.ToString());
        }
#endif
    }


    /// @author Antti Harju
    /// @version v0.5
    /// <summary>
    /// Block module. BlockSpawner forms blocks out of these.
    /// </summary>
    public class BlockModule : Poolable
    {
        private readonly Collision collision;
        private readonly Blocks manager;


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="settings">Settings</param>
        /// <param name="manager">Manager</param>
        public BlockModule(Settings settings, Blocks manager)
        {
            collision = settings.collision;
            this.manager = manager;
        }


        /// <summary>
        /// Spawn block module.
        /// </summary>
        public void Spawn()
        {
            collision.Set(First, X, Y, Width, Height);
            manager.Add(this);
        }


        /// <summary>
        /// Disable block.
        /// </summary>
        /// <param name="recursive">Disable recursively</param>
        public override void Disable(bool recursive = true)
        {
            base.Disable();
            if (recursive && Next != null)
                Next.Disable();
            Next = null;
            First = null;
            manager.Clear(this);
            collision.Set(null, X, Y, Width, Height);
            Width = 1;
            Height = 1;
        }


        /// <summary>
        /// Initialize block module. Scale is set from BlockSpawner.
        /// </summary>
        /// <param name="color">Block color</param>
        /// <param name="x">Horizontal position</param>
        /// <param name="y">Vertical position</param>
        /// <returns>Itself</returns>
        public BlockModule Initialize(Color color, int x, int y)
        {
            Color = color;
            X = x;
            Y = y;
            return this;
        }


        /// <summary>
        /// Block module color.
        /// </summary>
        public Color Color { get; private set; }


        /// <summary>
        /// First module of the block.
        /// </summary>
        public BlockModule First { get; set; }


        /// <summary>
        /// Next block module.
        /// </summary>
        public BlockModule Next { get; set; }


        /// <summary>
        /// Horizontal position.
        /// </summary>
        public int X { get; set; }


        /// <summary>
        /// Vertical position.
        /// </summary>
        public int Y { get; set; }


        /// <summary>
        /// Horizontal scale.
        /// </summary>
        public int Width { get; set; }


        /// <summary>
        /// Vertical scale.
        /// </summary>
        public int Height { get; set; }
    }
}
