using System;
using Otter;
using WBGame.Other;

namespace WBGame.GameObject
{
    /// <summary>
    /// Bunch class. A manager for blocks but also a block. Very much work in progress.
    /// </summary>
    class Bunch : Block
    {
        private readonly int size;
        private int count;
        private float floorHeight;
        public override Color Color { get { return Graphic.Color ?? null; } set { SetColor(value); } }

        public Bunch(int size) : base(size)
        {
            this.size = size;
        }

        public void Spawn(Vector2 position, Color color, int count, float floorHeight)
        {
            this.count = count;
            this.floorHeight = floorHeight;
            Spawn(position, color);
        }

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
                    anchorToCurrent = VectorCW(anchorToCurrent);
                else
                    anchorToCurrent = VectorCCW(anchorToCurrent);
                current.Position = anchor.Position + anchorToCurrent;
            }
        }

        public override void SoftDrop(int up = 1)
        {
            if (Lowest(Y) >= floorHeight - size)
                return;
            if (NextBlock != null)
                NextBlock.SoftDrop(up);
            Y += size * up;
        }


        public override void HardDrop()
        {
            int softDrops = (int)(floorHeight - Lowest(Y)) / size;
            SoftDrop(softDrops);
        }


        private Vector2 VectorCW(Vector2 v)
        {
            return new Vector2(v.Y, -v.X);
        }

        private Vector2 VectorCCW(Vector2 v)
        {
            return new Vector2(-v.Y, v.X);
        }
    }
}
