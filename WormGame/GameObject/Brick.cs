﻿using Otter.Graphics;
using Otter.Graphics.Drawables;
using Otter.Utility.MonoGame;
using WormGame.Core;
using WormGame.Static;
using WormGame.Pooling;

namespace WormGame.GameObject
{
    public class Brick : PoolableEntity
    {
        private readonly Collision field;
        private readonly Vector2[] positions;
        private readonly Vector2[] next;
        private readonly Image[] graphics;
        private readonly int kickLimit = 2;
        private readonly int maxLength;
        private readonly int size;

        private int anchorIndex;
        private int kickCounter;

        public int Count { get; private set; }
        public Player Player { get; set; }
        public override Color Color { get { return graphics[0].Color ?? null; } set { SetColor(value); } }


        public Brick(Config config)
        {
            size = config.size;
            field = config.field;
            maxLength = config.minWormLength;
            positions = new Vector2[maxLength];
            graphics = new Image[maxLength];
            next = new Vector2[maxLength];
            for (int i = 0; i < maxLength; i++)
            {
                Image tmp = Image.CreateRectangle(config.imageSize);
                tmp.Scale = (float)config.size / config.imageSize;
                tmp.Visible = false;
                tmp.CenterOrigin();
                graphics[i] = tmp;
                AddGraphic(tmp);
            }
        }


        public Brick Spawn(Worm worm)
        {
            Position = worm.GetTarget(0);
            Count = worm.Length;
            Color = worm.Color;
            anchorIndex = (int)Count / 2;
            kickCounter = 0;
            for (int i = 0; i < Count; i++)
            {
                field.Set(this, worm.GetTarget(i));
                positions[i] = worm.GetTarget(i) - worm.GetTarget(0);
                graphics[i].X = positions[i].X;
                graphics[i].Y = positions[i].Y;
                graphics[i].Visible = true;
            }
            return this;
        }

        public void Right(int amount = 1)
        {
            SetNull();
            for (int i = 0; i < Count; i++)
            {
                next[i].X = graphics[i].X;
                next[i].Y = graphics[i].Y;
                next[i].X += size * amount;
                if (field.Check(next[i] + Position) == 2)
                {
                    Reset(); return;
                }
            }
            Set();
        }

        public void Left()
        {
            Right(-1);
        }

        public void SoftDrop()
        {
            SetNull();
            for (int i = 0; i < Count; i++)
            {
                next[i].X = graphics[i].X;
                next[i].Y = graphics[i].Y;
                next[i].Y += size;
                if (field.Check(next[i] + Position) == 2)
                {
                    if (Player != null)
                    {
                        kickCounter++;
                        if (kickCounter >= kickLimit)
                        {
                            Player.LeaveBrick();
                        }
                    }
                    Reset();
                    return;
                }
            }
            kickCounter = 0;
            Set();
        }

        public void HardDrop()
        {
            SetNull();
            int dropAmount = HardDropAmount();
            for (int i = 0; i < Count; i++)
            {
                next[i].X = graphics[i].X;
                next[i].Y = graphics[i].Y;
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
                    if (field.Check(x, y) == 2)
                        return startY - y;
            return startY + 1;
        }

        public void Rotate(bool clockwise = false)
        {
            SetNull();
            next[anchorIndex].X = graphics[anchorIndex].X;
            next[anchorIndex].Y = graphics[anchorIndex].Y;
            for (int i = 0; i < Count; i++)
            {
                if (i == anchorIndex) i++;
                next[i].X = graphics[i].X;
                next[i].Y = graphics[i].Y;
                Vector2 rotationVector = next[i] - next[anchorIndex];
                rotationVector = clockwise ? Mathf.RotateCW(rotationVector) : Mathf.RotateCCW(rotationVector);
                next[i] = next[anchorIndex] + rotationVector;
            }
            for (int i = 0; i < Count; i++)
                if (field.Check(Position + next[i]) == 2)
                {
                    Reset();
                    return;
                }
            Set();
        }

        private void SetNull()
        {
            for (int i = 0; i < Count; i++)
                field.Set(null, X + graphics[i].X, Y + graphics[i].Y);
        }


        private void Reset()
        {
            for (int i = 0; i < Count; i++)
                field.Set(this, X + graphics[i].X, Y + graphics[i].Y);
        }


        private void Set()
        {
            graphics[0].X = 0;
            graphics[0].Y = 0;
            Position += next[0];
            field.Set(this, Position);
            for (int i = 1; i < Count; i++)
            {
                field.Set(this, Position + next[i] - next[0]);
                graphics[i].X = next[i].X - next[0].X;
                graphics[i].Y = next[i].Y - next[0].Y;
            }
        }

        private int Leftmost()
        {
            float leftmost = float.MaxValue;
            for (int i = 0; i < Count; i++)
                leftmost = Mathf.Smaller(graphics[i].X, leftmost);
            return field.X(X + leftmost);
        }


        private int Rightmost()
        {
            float rightmost = float.MinValue;
            for (int i = 0; i < Count; i++)
                rightmost = Mathf.Bigger(graphics[i].X, rightmost);
            return field.X(X + rightmost);
        }


        private int Lowest()
        {
            float lowest = float.MinValue;
            for (int i = 0; i < Count; i++)
                lowest = Mathf.Bigger(graphics[i].Y, lowest);
            return field.Y(Y + lowest);
        }


        public void SetColor(Color color)
        {
            for (int i = 0; i < Count; i++)
                graphics[i].Color = color;
        }
    }
}
