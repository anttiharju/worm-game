using Otter.Core;
using Otter.Graphics;
using Otter.Graphics.Drawables;
using WormGame.Core;
using WormGame.Pooling;

namespace WormGame.Entities
{
    /// <summary>
    /// Contains everything related to blocks.
    /// </summary>
    public class BlockManager : Pooler<Block>
    {
        public readonly Pooler<BlockModule> modules;
        public readonly int[,] blockBuffer;

        private readonly Tilemap tilemap;


        /// <summary>
        /// Initializes blockBuffer, tilemap and block poolers.
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="config"></param>
        /// <param name="capacity"></param>
        public BlockManager(Scene scene, Config config, int capacity)
        {
            blockBuffer = new int[config.width, config.height];
            tilemap = new Tilemap(config.width * config.size, config.height * config.size, config.size, config.size)
            {
                X = config.leftBorder - config.halfSize,
                Y = config.topBorder - config.halfSize
            };
            scene.AddGraphic(tilemap);

            modules = new BlockModulePooler(this, config, capacity);

            pool = new Block[capacity];
            endIndex = capacity - 1;
            for (int i = 0; i < capacity; i++)
            {
                Block currentPoolable = new Block(config, this);
                currentPoolable.Disable(false);
                pool[i] = currentPoolable;
            }
        }


        /// <summary>
        /// Clears a rectangular area out of tilemap.
        /// </summary>
        /// <param name="x">Horizontal position</param>
        /// <param name="y">Vertical position</param>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        public void Set(Color color, int x, int y, int width, int height)
        {
            tilemap.SetRect(x, y, width, height, color, "");
        }


        /// <summary>
        /// Clears a rectangular area out of tilemap.
        /// </summary>
        /// <param name="x">Horizontal position</param>
        /// <param name="y">Vertical position</param>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        public void Clear(int x, int y, int width, int height)
        {
            tilemap.ClearRect(x, y, width, height);
        }


        /// <summary>
        /// Spawns block.
        /// </summary>
        /// <param name="worm">Worm whose shape to copy</param>
        /// <returns>Spawned block or null</returns>
        public Block SpawnBlock(Worm worm)
        {
            Block block = Enable();
            if (block == null)
                return null;
            block = block.Spawn(worm, this);
            return block;
        }


        /// <summary>
        /// Clears tilemap and resets block poolers.
        /// </summary>
        public override void Reset()
        {
            tilemap.ClearAll();
            modules.Reset();
            for (int i = EnableIndex; i >= 0; i--)
                if (pool[i].Active)
                    pool[i].Disable(false);
            EnableIndex = 0;
        }
    }


    /// <summary>
    /// Custom Pooler. Required to pass manager as a parameter to modules.
    /// </summary>
    public class BlockModulePooler : Pooler<BlockModule>
    {
        /// <summary>
        /// Constructor that passes manager to all modules.
        /// </summary>
        /// <param name="manager">Manager</param>
        /// <param name="config">Configuration</param>
        /// <param name="capacity">Pool size</param>
        public BlockModulePooler(BlockManager manager, Config config, int capacity)
        {
            pool = new BlockModule[capacity];
            endIndex = capacity - 1;
            for (int i = 0; i < capacity; i++)
            {
                BlockModule currentPoolable = new BlockModule(config, manager);
                currentPoolable.Disable(false);
                pool[i] = currentPoolable;
            }
        }
    }
}
