using Otter.Graphics.Drawables;
using WormGame.Core;
using WormGame.Pooling;

namespace WormGame.Entities
{
    /// @author Antti Harju
    /// @version 14.08.2020
    /// <summary>
    /// A custom pooler that can be called to create blocks. Uses tilemap to render objects.
    /// </summary>
    public class BlockManager : Pooler<BlockModule>
    {
        public readonly BlockSpawner blockSpawner;

        private readonly Tilemap tilemap;


        /// <summary>
        /// Initialize manager.
        /// </summary>
        /// <param name="config">Configuration</param>
        public BlockManager(Config config)
        {
            tilemap = config.tilemap;
            blockSpawner = new BlockSpawner(config);

            int capacity = config.moduleAmount;
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
