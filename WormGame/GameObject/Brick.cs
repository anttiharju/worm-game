using Otter;
using WormGame.Other;
using WormGame.Help;

namespace WormGame.GameObject
{
    /// <summary>
    /// Block class, work in progress.
    /// </summary>
    class Brick : Poolable
    {
        private readonly int size;
        public Brick NextBlock { get; set; }

        /// <summary>
        /// Constructor. Nothing special.
        /// </summary>
        /// <param name="size"></param>
        public Brick(int size) : base()
        {
            this.size = size;
            Image image = Image.CreateRectangle(size);
            AddGraphic(image);
            image.CenterOrigin();
        }


        /// <summary>
        /// Spawns the block. Nothing special.
        /// </summary>
        /// <param name="position">Position</param>
        /// <param name="color">Block color</param>
        public void Spawn(Vector2 position, Color color)
        {
            Position = position;
            Graphic.Color = color;
        }


        /// <summary>
        /// Recursive method to drop all blocks by one.
        /// </summary>
        /// <param name="up"></param>
        public virtual void SoftDrop(int up = 1)
        {
            if (NextBlock != null)
                NextBlock.SoftDrop(up);
            Y += size * up;
        }


        /// <summary>
        /// Recursive method that returns the lowest vertical block position in the bunch.
        /// </summary>
        /// <param name="biggest"></param>
        /// <returns>Lowest vertical block position in the bunch</returns>
        public float Lowest(float biggest)
        {
            if (NextBlock != null)
                biggest = NextBlock.Lowest(Mathf.Bigger(Y, biggest));
            return biggest;
        }


        /// <summary>
        /// Kind of like an indexer.
        /// </summary>
        /// <param name="i">wanted block index</param>
        /// <param name="currentI">current block index</param>
        /// <returns>block from wanted index</returns>
        public Brick GetBlock(int i, int currentI = 0)
        {
            if (i > currentI)
                return NextBlock.GetBlock(i, ++currentI);
            else
                return this;
        }


        /// <summary>
        /// Recursively changes color of all the blocks in the bunch.
        /// </summary>
        /// <param name="color">Worms new color</param>
        public void SetColor(Color color)
        {
            if (NextBlock != null)
                NextBlock.SetColor(color);
            Graphic.Color = color;
        }
    }
}
