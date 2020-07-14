﻿using Otter.Graphics;
using Otter.Graphics.Drawables;
using Otter.Utility.MonoGame;
using WormGame.Core;

namespace WormGame.GameObject
{
    public class Brick : Poolable
    {
        private readonly int size;
        private readonly Image[] graphics;
        private readonly Collision field;

        private int anchorIndex;
        private int maxLength;
        private WormScene scene;
        private Vector2[] positions;


        public int Count { get; private set; }
        public Player Player { get; set; }
        public override Color Color { get { return graphics[0].Color ?? null; } set { SetColor(value); } }


        public Brick(Config config)
        {
            scene = config.scene;
            size = config.size;
            field = config.field;
            maxLength = config.maxWormLength;
            positions = new Vector2[maxLength];
            graphics = new Image[maxLength];
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
            for (int i = 0; i < Count; i++)
            {
                positions[i] = worm.GetTarget(i) - worm.GetTarget(0);
                graphics[i].X = positions[i].X;
                graphics[i].Y = positions[i].Y;
                graphics[i].Visible = true;
            }
            return this;
        }

        public void Right()
        {
        }

        public void Left()
        {
        }

        public void SoftDrop()
        {
        }

        public void HardDrop()
        {
        }

        public void Rotate(bool ccw = false)
        {
        }

        /*
        public void Rotate(bool clockwise = false)
        {
            SetNull();
            BrickEntity anchor = graphics[anchorIndex];
            for (int i = 0; i < Count; i++)
            {
                next[i] = graphics[i].Position; // When moving horizontally rotation fucks up sometimes without this line 
                if (i == anchorIndex) i++;
                Vector2 rotationVector = graphics[i].Position - anchor.Position;
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
                field.Set(null, graphics[j].Position);
        }


        private void Reset()
        {
            for (int i = 0; i < Count; i++)
                field.Set(graphics[i]);
        }


        private void Set()
        {
            for (int i = 0; i < Count; i++)
            {
                graphics[i].Position = next[i];
                field.Set(graphics[i]);
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
                next[i] = graphics[i].Position;
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
                next[i] = graphics[i].Position;
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


        public void Fall()
        {
            SetNull();
            for (int i = 0; i < Count; i++)
            {
                next[i] = graphics[i].Position;
                next[i].Y += size;
                if (!field.Check(next[i]))
                {
                    Reset();
                    kickCounter++;
                    if (kickCounter >= kickBuffer)
                    {

                    }
                    return;
                }
            }
            Set();
        }


        public override void Disable()
        {
            for (int i = 0; i < Count; i++)
            {
                field.Set(null, graphics[i].Position);
                graphics[i].Enabled = false;
                graphics[i].Parent = null;
            }
        }


        private int Leftmost()
        {
            float smallest = float.MaxValue;
            for (int i = 0; i < Count; i++)
                smallest = Mathf.Smaller(graphics[i].X, smallest);
            return field.X(smallest);
        }


        private int Rightmost()
        {
            float biggest = 0;
            for (int i = 0; i < Count; i++)
                biggest = Mathf.Bigger(graphics[i].X, biggest);
            return field.X(biggest);
        }


        private int Lowest()
        {
            float biggest = 0;
            for (int i = 0; i < Count; i++)
                biggest = Mathf.Bigger(graphics[i].Y, biggest);
            return field.Y(biggest);
        }*/


        public void SetColor(Color color)
        {
            for (int i = 0; i < Count; i++)
                graphics[i].Color = color;
        }
    }
}
