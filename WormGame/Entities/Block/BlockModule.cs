using Otter.Graphics;
using WormGame.Core;
using WormGame.Pooling;

namespace WormGame.Entities
{
    /// @author Antti Harju
    /// @version 14.08.2020
    /// <summary>
    /// Block module. BlockSpawner forms blocks out of these.
    /// </summary>
    public class BlockModule : Poolable
    {
        private readonly BlockManager manager;
        private readonly Collision collision;


        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="config">Configuration</param>
        /// <param name="manager">Manager</param>
        public BlockModule(Config config, BlockManager manager)
        {
            this.manager = manager;
            collision = config.collision;
        }


        /// <summary>
        /// Adds block to tilemap and collision.
        /// </summary>
        public void Add()
        {
            manager.Add(this);
            collision.Add(First, X, Y, Width, Height);
        }


        /// <summary>
        /// Disables block.
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
            collision.Add(null, X, Y, Width, Height);
            Width = 1;
            Height = 1;
        }


        /// <summary>
        /// Initializes module. Spawning is done by BlockSpawner.
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
        /// Module color.
        /// </summary>
        public Color Color { get; private set; }


        /// <summary>
        /// First module of the block.
        /// </summary>
        public BlockModule First { get; set; }


        /// <summary>
        /// Next module in the block.
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
        /// Width (ScaleX)
        /// </summary>
        public int Width { get; set; }


        /// <summary>
        /// Height (ScaleY)
        /// </summary>
        public int Height { get; set; }
    }
}
