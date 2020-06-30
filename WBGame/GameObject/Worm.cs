using Otter;
using System;
using WormGame.Other;

namespace WormGame.GameObject
{
    /// @author Antti Harju
    /// @version 22.06.2020
    /// <summary>
    /// The worm class. Technically it's just the head entity but it does manage the entire worm.
    /// </summary>
    class Worm : Tail
    {
        private Collision collision;
        private readonly int size;

        public int Length { get; private set; }
        public string Direction { private get; set; }
        public override Color Color { get { return Graphic.Color ?? null; } set { SetColor(value); } }

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
        public Worm Spawn(Collision collision, int x, int y, int length, Color color, string direction)
        {
            this.collision = collision;
            Length = length;
            Direction = direction;
            Position = new Vector2(collision.X(x), collision.Y(y));
            Console.WriteLine(x + " " + Position.X + " " + collision.ReverseX((int)Position.X) + " " + collision.X(collision.ReverseX((int)Position.X)));
            Console.WriteLine(y + " " + Position.Y + " " + collision.ReverseY((int)Position.Y) + " " + collision.Y(collision.ReverseY((int)Position.Y)));
            Console.WriteLine(" ");
            Target = Position;
            Graphic.Color = color;
            collision.Set(this);
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
            if (collision.WormCheck(this, Position, deltaX, deltaY))
                Move(deltaX, deltaY);
        }


        public Tail[] GetWorm()
        {
            Tail[] tmp = new Tail[Length];
            if (NextBody != null)
                NextBody.GetWorm(ref tmp, 1);
            tmp[0] = this;
            return tmp;
        }
    }
}
