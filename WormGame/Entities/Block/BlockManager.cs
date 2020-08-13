using Otter.Core;
using Otter.Graphics.Drawables;
using WormGame.Core;
using WormGame.Pooling;

namespace WormGame.Entities
{
    /// @author Antti Harju
    /// @version 14.08.2020
    /// <summary>
    /// A custom pooler that can be called to create blocks and also renders them using tilemap.
    /// </summary>
    public class BlockManager : Pooler<BlockModule>
    {
        public readonly BlockSpawner blockSpawner;

        private readonly Tilemap tilemap;


        /// <summary>
        /// Initializes blockBuffer, tilemap and block poolers.
        /// </summary>
        /// <param name="scene">Scene</param>
        /// <param name="config">Configuration</param>
        /// <param name="capacity">Pool length</param>
        public BlockManager(Scene scene, Config config, int capacity)
        {
            tilemap = new Tilemap(config.width * config.size, config.height * config.size, config.size, config.size)
            {
                X = config.leftBorder - config.halfSize,
                Y = config.topBorder - config.halfSize
            };
            scene.AddGraphic(tilemap);

            blockSpawner = new BlockSpawner(config);

            pool = new BlockModule[capacity];
            endIndex = capacity - 1;
            for (int i = 0; i < capacity; i++)
            {
                BlockModule current = new BlockModule(config, this);
                current.Disable(false);
                pool[i] = current;
            }
        }


        /// <summary>
        /// Add module to tilemap.
        /// </summary>
        /// <param name="module">Module to add</param>
        public void Add(BlockModule module)
        {
            // Replace block.Color with Otter.Graphics.Color.Random to see the modules that form the blocks
            tilemap.SetRect(module.X, module.Y, module.Width, module.Height, module.Color, "");
        }


        /// <summary>
        /// Clears module from tilemap.
        /// </summary>
        /// <param name="module">Module to clear</param>
        public void Clear(BlockModule module)
        {
            tilemap.ClearRect(module.X, module.Y, module.Width, module.Height);
        }


        /// <summary>
        /// Resets module pool and clears tilemap.
        /// </summary>
        public override void Reset()
        {
            base.Reset();
            tilemap.ClearAll();
        }


        /// <summary>
        /// Spawn a block.
        /// </summary>
        /// <param name="worm">Worm to blockify</param>
        /// <returns>Block or null</returns>
        public BlockModule SpawnBlock(Worm worm)
        {
            return blockSpawner.Spawn(worm, this);
        }
    }
}
