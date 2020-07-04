using Otter;
using System;
using WormGame.Help;
using WormGame.Other;

namespace WormGame.GameObject
{
    /// @author Antti Harju
    /// @version 01.07.2020
    /// <summary>
    /// The BrickBrain class. Technically it's just the head entity but it manages the entire BrickBrain.
    /// </summary>
    class Bunch : Brick
    {
        private readonly Brick[] allBricks;
        private readonly Vector2[] next;
        private readonly int size;

        private PlayArea playArea;
        private Collision collision;
        private float bottom;

        public override Color Color { get { return Graphic.Color ?? null; } set { SetColor(value); } }
        public bool Posessed { get; set; }
        public int Count { get; private set; }

        /// <summary>
        /// Head constructor. Calls Body constructor.
        /// </summary>
        /// <param name="size">Diameter of the circle graphic</param>
        public Bunch(int size) : base(size)
        {
            this.size = size;
            allBricks = new Brick[Config.maxWormLength];
            next = new Vector2[Config.maxWormLength];
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
        public Bunch Spawn(Pooler<Brick> bricks, Collision collision, PlayArea playArea, Worm worm)
        {
            this.collision = collision;
            this.playArea = playArea;
            bottom = playArea.EntityX(0);

            Count = worm.Length;
            for (int i = 0; i < Count; i++)
            {
                allBricks[i] = bricks.Enable();
                allBricks[i].X = worm[i].target.X;
                allBricks[i].Y = worm[i].target.Y;
                playArea.Update(allBricks[i]);
            }
            Color = worm.Color;

            return this;
        }


        /// <summary>
        /// Moves the BrickBrain the desired amount
        /// </summary>
        /// <param name="deltaX">Horizontal movement</param>
        /// <param name="deltaY">Vertical movement</param>
        private void CheckCollision(int deltaX, int deltaY)
        {
            /*
            nextPos.X = Position.X + deltaX; nextPos.Y = Position.Y + deltaY;

            if (collision.BrickCheck(this, nextPos))
                Move(deltaX, deltaY);*/
        }


        /// <summary>
        /// Rotates the bunch by 90 degrees.
        /// </summary>
        /// <param name="clockwise">rotation direction</param>
        public void Rotate(bool clockwise = false)
        {
            for (int i = 0; i < Count; i++)
                next[i] = allBricks[i].Position;

            int anchorIndex = Count / 2;
            Brick anchor = allBricks[anchorIndex];
            for (int i = 0; i < Count; i++)
            {
                if (i == anchorIndex) i++;
                Vector2 rotationVector = next[i] - anchor.Position;
                rotationVector = clockwise ? Mathf.RotateCW(rotationVector) : Mathf.RotateCCW(rotationVector);
                next[i] = anchor.Position + rotationVector;
            }

            for (int j = 0; j < Count; j++)
                playArea.Update(allBricks[j].Position, null);

            for (int i = 0; i < Count; i++)
                if (!collision.Check(next[i]))
                {
                    for (int j = 0; j < Count; j++)
                        playArea.Update(allBricks[j]);
                    return;
                }

            for (int i = 0; i < Count; i++)
            {
                allBricks[i].Position = next[i];
                playArea.Update(allBricks[i]);
            }
        }


        /// <summary>
        /// Drops the bunch by one.
        /// </summary>
        /// <param name="amount"></param>
        public void Drop(int amount = 1)
        {
            for (int i = 0; i < Count; i++)
            {
                next[i] = allBricks[i].Position;
                next[i].Y += size * amount;
                if (!collision.Check(next[i]))
                    return;
            }
            for (int i = 0; i < Count; i++)
            {
                playArea.Update(allBricks[i].Position, null);
                allBricks[i].Position = next[i];
                playArea.Update(allBricks[i]);
            }
        }


        /// <summary>
        /// Disables the BrickBrain
        /// </summary>
        public override void Disable()
        {
            for (int i = 0; i < Count; i++)
            {
                playArea.Update(allBricks[i].Position, null);
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
