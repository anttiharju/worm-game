using Otter.Graphics;
using Otter.Utility.MonoGame;
using WormGame.Static;
using WormGame.Manager;

namespace WormGame.GameObject
{
    /// @author Antti Harju
    /// @version 01.07.2020
    /// <summary>
    /// Manages a collection of bricks.
    /// </summary>
    class Brick : BrickBase
    {
        public override Color Color { get { return Graphic.Color ?? null; } set { SetColor(value); } }
        public bool Posessed { get; set; }
        public int Count { get; private set; }

        private Collision field;
        private int anchorIndex;

        private readonly BrickBase[] allBricks;
        private readonly Vector2[] next;
        private readonly int size;


        /// <summary>
        /// Head constructor. Calls Body constructor.
        /// </summary>
        /// <param name="size">Diameter of the circle graphic</param>
        public Brick(int size, Config config) : base(size, config)
        {
            this.size = size;
            allBricks = new BrickBase[config.maxWormLength];
            next = new Vector2[config.maxWormLength];
        }


        /// <summary>
        /// Spawns the BrickBrain
        /// </summary>
        /// <param name="BrickBrainGame">Manager</param>
        /// <param name="x">Horizontal position</param>
        /// <param name="y">Vertical position</param>
        /// <param name="count">BrickBrains length</param>
        /// <param name="color">BrickBrains color</param>
        /// <param name="directions">Movement instructions for the BrickBrain</param>
        /// <returns>The spawned BrickBrain</returns>
        public Brick Spawn(Pooler<BrickBase> baseBricks, Collision field, Worm worm)
        {
            this.field = field;
            Count = worm.Length;
            SetBrick(this, worm, 0);
            for (int i = 1; i < Count; i++)
                SetBrick(baseBricks.Enable(), worm, i);
            anchorIndex = Count / 2;
            Color = worm.Color;
            return this;
        }

        public void SetBrick(BrickBase brick, Worm worm, int i)
        {
            allBricks[i] = brick;
            allBricks[i].X = worm[i].target.X;
            allBricks[i].Y = worm[i].target.Y;
            field.Update(allBricks[i]);
        }


        /// <summary>
        /// Rotates the bunch by 90 degrees.
        /// </summary>
        /// <param name="clockwise">rotation direction</param>
        public void Rotate(bool clockwise = false)
        {
            SetNull();

            BrickBase anchor = allBricks[anchorIndex];
            for (int i = 0; i < Count; i++)
            {
                // I don't know why, but without the next line rotating sometimes fucks up when moving horizontally.
                next[i] = allBricks[i].Position; 
                if (i == anchorIndex) i++;
                Vector2 rotationVector = allBricks[i].Position - anchor.Position;
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
                field.Update(allBricks[j].Position, null);
        }


        private void Reset()
        {
            for (int i = 0; i < Count; i++)
                field.Update(allBricks[i]);
        }


        private void Set()
        {
            for (int i = 0; i < Count; i++)
            {
                allBricks[i].Position = next[i];
                field.Update(allBricks[i]);
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
                next[i] = allBricks[i].Position;
                next[i].X += size * amount;
                if (!field.Check(next[i]))
                {
                    Reset(); return;
                }
            }
            Set();
        }


        /// <summary>
        /// Drops the bunch by one.
        /// </summary>
        /// <param name="amount"></param>
        public void HardDrop(int amount = 1)
        {
            SoftDrop();
        }


        /// <summary>
        /// Drops the bunch by one.
        /// </summary>
        /// <param name="amount"></param>
        public void SoftDrop(int amount = 1)
        {
            SetNull();
            for (int i = 0; i < Count; i++)
            {
                next[i] = allBricks[i].Position;
                next[i].Y += size * amount;
                if (!field.Check(next[i]))
                {
                    Reset(); return;
                }
            }
            Set();
        }


        /// <summary>
        /// Disables the BrickBrain
        /// </summary>
        public override void Disable()
        {
            for (int i = 0; i < Count; i++)
            {
                field.Update(allBricks[i].Position, null);
                allBricks[i].Enabled = false;
            }
        }

        public float Lowest(Vector2[] next)
        {
            float biggest = 0;
            for (int i = 0; i < Count; i++)
                biggest = Mathf.Bigger(next[i].Y, biggest);
            return biggest;
        }


        /// <summary>
        /// Sets Brick color
        /// </summary>
        /// <param name="color"></param>
        public void SetColor(Color color)
        {
            for (int i = 0; i < Count; i++)
                allBricks[i].Graphic.Color = color;
        }
    }
}
