using Otter;
using WormGame.Other;

namespace WormGame.GameObject
{
    /// <summary>
    /// Bunch class, work in progress. A manager for blocks but also a block.
    /// </summary>
    class Bunch : Block
    {
        private readonly int size;
        private int count;
        private float bottom;
        public override Color Color { get { return Graphic.Color ?? null; } set { SetColor(value); } }

        /// <summary>
        /// Constructor. Nothing special.
        /// </summary>
        /// <param name="size"></param>
        public Bunch(int size) : base(size)
        {
            this.size = size;
        }


        /// <summary>
        /// Spawns the bunch block.
        /// </summary>
        /// <param name="position">Position to spawn the bunch block</param>
        /// <param name="color">Bunch block color</param>
        /// <param name="count">How many blocks does the bunch contain</param>
        /// <param name="bottom">Play area bottom</param>
        public void Spawn(Vector2 position, Color color, int count, float bottom)
        {
            this.count = count;
            this.bottom = bottom;
            Spawn(position, color);
        }


        /// <summary>
        /// Rotates the bunch by 90 degrees.
        /// </summary>
        /// <param name="clockwise">rotation direction</param>
        public void Rotate(bool clockwise = false)
        {
            int anchorIndex = count / 2;
            Block anchor = GetBlock(anchorIndex);
            for (int i = 0; i < count; i++)
            {
                if (i == anchorIndex)
                    i++;
                Block current = GetBlock(i);
                Vector2 anchorToCurrent = current.Position - anchor.Position;
                if (clockwise)
                    anchorToCurrent = Mathf.RotateCW(anchorToCurrent);
                else
                    anchorToCurrent = Mathf.RotateCCW(anchorToCurrent);
                current.Position = anchor.Position + anchorToCurrent;
            }
        }


        /// <summary>
        /// Drops the bunch by one.
        /// </summary>
        /// <param name="up"></param>
        public override void SoftDrop(int up = 1)
        {
            if (Lowest(Y) >= bottom - size)
                return;
            if (NextBlock != null)
                NextBlock.SoftDrop(up);
            Y += size * up;
        }


        /// <summary>
        /// Drops the bunch to the bottom.
        /// </summary>
        public void HardDrop()
        {
            int softDrops = (int)(bottom - Lowest(Y)) / size;
            SoftDrop(softDrops);
        }
    }
}
