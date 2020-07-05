using Otter.Graphics;
using Otter.Utility.MonoGame;
using WBGame.Other;
using WormGame.Help;
using WormGame.Other;

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
        public Controls controls;

        private Collision field;
        private int anchorIndex;

        private readonly BrickBase[] allBricks;
        private readonly Vector2[] next;
        private readonly int size;


        /// <summary>
        /// Head constructor. Calls Body constructor.
        /// </summary>
        /// <param name="size">Diameter of the circle graphic</param>
        public Brick(int size) : base(size)
        {
            this.size = size;
            allBricks = new BrickBase[Config.maxWormLength];
            next = new Vector2[Config.maxWormLength];
            controls = new Controls();
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
        public Brick Spawn(Pooler<BrickBase> bricks, Collision field, Worm worm)
        {
            this.field = field;
            anchorIndex = Count / 2;

            Count = worm.Length;
            for (int i = 0; i < Count; i++)
            {
                allBricks[i] = bricks.Enable();
                allBricks[i].X = worm[i].target.X;
                allBricks[i].Y = worm[i].target.Y;
                field.Update(allBricks[i]);
            }
            Color = worm.Color;

            return this;
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

        public override void Update()
        {
            switch (controls.Next())
            {
                case 'W':
                    HardDrop();
                    break;
                case 'A':
                    Left();
                    break;
                case 'S':
                    SoftDrop();
                    break;
                case 'D':
                    Right();
                    break;
                case 'E':
                    Rotate(true);
                    break;
                case 'Q':
                    Rotate();
                    break;
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
        /// Sets BrickBrains color
        /// </summary>
        /// <param name="color"></param>
        public void SetColor(Color color)
        {
            for (int i = 0; i < Count; i++)
                allBricks[i].Graphic.Color = color;
        }
    }
}
