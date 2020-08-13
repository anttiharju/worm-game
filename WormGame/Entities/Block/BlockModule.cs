using WormGame.Core;
using WormGame.Pooling;

namespace WormGame.Entities
{
    /// @author Antti Harju
    /// @version 06.08.2020
    /// <summary>
    /// BlockModule. Scaled as needed by Block.
    /// </summary>
    public class BlockModule : Poolable
    {
        private readonly BlockManager blockManager;
        private readonly Collision collision;

        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public BlockModule Next { get; set; }


        public BlockModule(Config config, BlockManager blockManager)
        {
            this.blockManager = blockManager;
            collision = config.collision;
        }

        public BlockModule Spawn(int x, int y)
        {
            X = x;
            Y = y;
            return this;
        }

        public void Set(Block parent)
        {
            blockManager.Set(parent.Color, X, Y, Width, Height);
        }

        public override void Disable(bool recursive = true)
        {
            base.Disable();
            if (recursive && Next != null)
                Next.Disable();
            Next = null;
            collision.Set(null, X, Y, Width, Height);
            blockManager.Clear(X, Y, Width, Height);
            Width = 1;
            Height = 1;
        }
    }
}
