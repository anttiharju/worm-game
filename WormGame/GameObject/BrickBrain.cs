using Otter.Graphics;
using Otter.Utility.MonoGame;
using WormGame.Core;
using WormGame.Static;
using WormGame.Pooling;

namespace WormGame.GameObject
{
    public class BrickBrain : BasicPoolable
    {
        private readonly Brick[] bricks;
        private readonly Vector2[] next;
        private readonly int size;

        private Collision field;
        private int anchorIndex;

        public Vector2 Position { get { return bricks[0].Position; } }
        public Color Color { get { return bricks[0].Color ?? null; } set { SetColor(value); } }
        public bool Posessed { get; set; }
        public int Count { get; private set; }

        public BrickBrain(Config config)
        {
            size = config.size;
            bricks = new Brick[config.maxWormLength];
            next = new Vector2[config.maxWormLength];
        }


        public BrickBrain Spawn(Pool<Brick> baseBricks, Collision field, Worm worm)
        {
            this.field = field;
            Count = worm.Length;
            for (int i = 0; i < Count; i++)
                SetBrick(baseBricks.Enable(), worm, i);
            anchorIndex = Count / 2;
            Color = worm.Color;
            return this;
        }


        public void SetBrick(Brick brick, Worm worm, int i)
        {
            bricks[i] = brick;
            bricks[i].Position = worm.GetTarget(i);
            field.Set(bricks[i]);
        }


        public void Rotate(bool clockwise = false)
        {
            SetNull();

            Brick anchor = bricks[anchorIndex];
            for (int i = 0; i < Count; i++)
            {
                // I don't know why, but without the next line rotating sometimes fucks up when moving horizontally.
                next[i] = bricks[i].Position;
                if (i == anchorIndex) i++;
                Vector2 rotationVector = bricks[i].Position - anchor.Position;
                rotationVector = clockwise ? Mathf.RotateCW(rotationVector) : Mathf.RotateCCW(rotationVector);
                next[i] = anchor.Position + rotationVector;
            }

            for (int i = 0; i < Count; i++)
            {
                if (!field.Check(next[i]))
                {
                    Reset(); return;
                }
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
            for (int i = 0; i < Count; i++)
            {
                next[i] = bricks[i].Position;
                next[i].Y += size * HardDropAmount();
                if (!field.Check(next[i]))
                {
                    Reset(); return;
                }
            }
            Set();
        }

        public int HardDropAmount()
        {
            int startX = Leftmost();
            int endX = Rightmost();
            int startY = field.Y(Position.Y) - 1;
            for (int x = startX; x < endX; x++)
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
                    Reset(); return;
            }
            Set();
        }

        public override void Disable()
        {
            for (int i = 0; i < Count; i++)
            {
                field.Set(null, bricks[i].Position);
                bricks[i].Enabled = false;
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

        private float Lowest()
        {
            float biggest = 0;
            for (int i = 0; i < Count; i++)
                biggest = Mathf.Bigger(bricks[i].Y, biggest);
            return biggest;
        }


        public void SetColor(Color color)
        {
            for (int i = 0; i < Count; i++)
                bricks[i].Graphic.Color = color;
        }
    }
}
