using Otter.Graphics;
using Otter.Utility.MonoGame;
using WormGame.Core;
using WormGame.Static;
using WormGame.Pooling;
using System;

namespace WormGame.GameObject
{
    public class Brick : BasicPoolable
    {
        private readonly BrickEntity[] bricks;
        private readonly Vector2[] next;
        private readonly int size;

        private Collision field;
        private int anchorIndex;

        public WormScene Scene { get; private set; }
        public Color Color { get { return bricks[0].Color ?? null; } set { SetColor(value); } }
        public Vector2 Position { get { return bricks[0].Position; } }
        public int Count { get; private set; }
        public bool Posessed { get; set; }
        public bool Falling { get; set; }

        public Brick(Config config)
        {
            size = config.size;
            bricks = new BrickEntity[config.maxWormLength];
            next = new Vector2[config.maxWormLength];
        }


        public Brick Spawn(WormScene scene, Collision field, Pool<BrickEntity> brickEntities, Worm worm)
        {
            this.field = field;
            Count = worm.Length;
            Scene = scene;
            for (int i = 0; i < Count; i++)
            {
                BrickEntity tmp = brickEntities.Enable();
                if (tmp == null) return null;
                SetBrick(tmp, worm, i);
                tmp.Parent = this;
            }
            anchorIndex = Count / 2;
            Color = worm.Color;
            return this;
        }


        public void SetBrick(BrickEntity brick, Worm worm, int i)
        {
            bricks[i] = brick;
            bricks[i].Position = worm.GetTarget(i);
            field.Set(bricks[i]);
        }


        public void Rotate(bool clockwise = false)
        {
            SetNull();
            BrickEntity anchor = bricks[anchorIndex];
            for (int i = 0; i < Count; i++)
            {
                next[i] = bricks[i].Position; // When moving horizontally rotation fucks up sometimes without this line 
                if (i == anchorIndex) i++;
                Vector2 rotationVector = bricks[i].Position - anchor.Position;
                rotationVector = clockwise ? Mathf.RotateCW(rotationVector) : Mathf.RotateCCW(rotationVector);
                next[i] = anchor.Position + rotationVector;
            }
            for (int i = 0; i < Count; i++)
                if (!field.Check(next[i]))
                {
                    Reset();
                    return;
                }
            Set();
        }


        private void SetNull()
        {
            for (int j = 0; j < Count; j++)
                field.Set(null, bricks[j].Position);
        }


        private void Reset()
        {
            for (int i = 0; i < Count; i++)
                field.Set(bricks[i]);
        }


        private void Set()
        {
            for (int i = 0; i < Count; i++)
            {
                bricks[i].Position = next[i];
                field.Set(bricks[i]);
            }
        }


        public void Left()
        {
            Right(-1);
        }


        public void Right(int amount = 1)
        {
            SetNull();
            for (int i = 0; i < Count; i++)
            {
                next[i] = bricks[i].Position;
                next[i].X += size * amount;
                if (!field.Check(next[i]))
                {
                    Reset(); return;
                }
            }
            Set();
        }


        public void HardDrop()
        {
            SetNull();
            int dropAmount = HardDropAmount();
            for (int i = 0; i < Count; i++)
            {
                next[i] = bricks[i].Position;
                next[i].Y += size * dropAmount;
            }
            Set();
        }


        public int HardDropAmount()
        {
            int endX = Rightmost();
            int startX = Leftmost();
            int startY = Lowest() - 1;
            for (int x = startX; x <= endX; x++)
                for (int y = startY; y >= 0; y--)
                    if (!field.Check(x, y))
                        return startY - y;
            return startY + 1;
        }


        public void SoftDrop()
        {
            SetNull();
            for (int i = 0; i < Count; i++)
            {
                next[i] = bricks[i].Position;
                next[i].Y += size;
                if (!field.Check(next[i]))
                {
                    Reset();
                    return;
                }
            }
            Set();
        }


        public override void Disable()
        {
            for (int i = 0; i < Count; i++)
            {
                field.Set(null, bricks[i].Position);
                bricks[i].Enabled = false;
                bricks[i].Parent = null;
            }
        }


        private int Leftmost()
        {
            float smallest = float.MaxValue;
            for (int i = 0; i < Count; i++)
                smallest = Mathf.Smaller(bricks[i].X, smallest);
            return field.X(smallest);
        }


        private int Rightmost()
        {
            float biggest = 0;
            for (int i = 0; i < Count; i++)
                biggest = Mathf.Bigger(bricks[i].X, biggest);
            return field.X(biggest);
        }


        private int Lowest()
        {
            float biggest = 0;
            for (int i = 0; i < Count; i++)
                biggest = Mathf.Bigger(bricks[i].Y, biggest);
            return field.Y(biggest);
        }


        public void SetColor(Color color)
        {
            for (int i = 0; i < Count; i++)
                bricks[i].Graphic.Color = color;
        }
    }
}
