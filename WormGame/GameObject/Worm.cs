using Otter;
using System;
using System.Collections;
using WormGame.Other;

namespace WormGame.GameObject
{
    /// @author Antti Harju
    /// @version 22.06.2020
    /// <summary>
    /// The worm class. Technically it's just the head entity but it manages the entire worm.
    /// </summary>
    class Worm : Tail
    {
        public override Color Color { get { return Graphic.Color ?? null; } set { SetColor(value); } }
        public string Direction { private get; set; }
        public int Length { get; private set; }

        private Collision collision;
        private PlayArea playArea;
        private readonly int size;
        private Tail[] wholeWorm;
        public bool Noclip { private get; set; }

        /// <summary>
        /// Head constructor. Calls Body constructor.
        /// </summary>
        /// <param name="size">Diameter of the circle graphic</param>
        public Worm(int size) : base(size)
        {
            this.size = size;
            Direction = "UP";
        }


        /// <summary>
        /// Spawns the worm
        /// </summary>
        /// <param name="wormGame">Manager</param>
        /// <param name="x">Horizontal position</param>
        /// <param name="y">Vertical position</param>
        /// <param name="length">Worms length</param>
        /// <param name="color">Worms color</param>
        /// <param name="directions">Movement instructions for the worm</param>
        /// <returns>The spawned worm</returns>
        public Worm Spawn(Pooler<Tail> tails, Collision collision, PlayArea playArea, int x, int y, int length, Color color, string direction)
        {
            this.collision = collision;
            this.playArea = playArea;
            Length = length;
            Direction = direction;
            Position = new Vector2(x, y);
            Target = Position;
            Graphic.Color = color;
            playArea.Update(Target, this);

            int bodyCount = length - 1; // - 1 because head already counts as 1
            Tail currentBody = this;
            for (int i = 0; i < bodyCount; i++)
            {
                Tail tmpBody = tails.Enable();
                if (tmpBody == null) return null;
                tmpBody.Position = new Vector2(x, y);
                tmpBody.Target = tmpBody.Position;
                tmpBody.Graphic.Color = color;
                currentBody.NextBody = tmpBody;
                currentBody = tmpBody;
            }
            wholeWorm = GetWorm();
            return this;
        }


        /// <summary>
        /// Moves the worm
        /// </summary>
        public void Move()
        {
            switch (Direction)
            {
                case "UP":
                    CheckCollision(0, -size);
                    break;
                case "LEFT":
                    CheckCollision(-size, 0);
                    break;
                case "DOWN":
                    CheckCollision(0, size);
                    break;
                case "RIGHT":
                    CheckCollision(size, 0);
                    break;
            }
        }


        /// <summary>
        /// Moves the worm the desired amount
        /// </summary>
        /// <param name="deltaX">Horizontal movement</param>
        /// <param name="deltaY">Vertical movement</param>
        private void CheckCollision(int deltaX, int deltaY)
        {
            if (collision.WormCheck(this, Target, deltaX, deltaY, Noclip))
                Move(deltaX, deltaY);
        }

        public override void Disable()
        {
            if (NextBody != null)
                NextBody.Disable(playArea);
            Enabled = false;
            playArea.Update(Target, null);
        }

        public Tail[] GetWorm()
        {
            Tail[] tmp = new Tail[Length];
            if (NextBody != null)
                NextBody.GetWorm(ref tmp, 1);
            tmp[0] = this;
            return tmp;
        }


#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
        /// <summary>
        /// Indexer for the worm
        /// </summary>
        /// <param name="i">index</param>
        /// <returns>Worms body part at index</returns>
        public Tail this[int i]
        {
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
            get => wholeWorm[i];
            set => wholeWorm[i] = value;
        }
    }
}
