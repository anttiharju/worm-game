using Otter;
using WBGame.Other;

namespace WBGame.GameObject
{
    /// @author Antti Harju
    /// @version 22.06.2020
    /// <summary>
    /// The worm class. Technically it's just the head entity but it does manage the entire worm.
    /// </summary>
    class Worm : Tail
    {
        private Manager manager;
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
        /// <param name="manager">Manager</param>
        /// <param name="x">Horizontal position</param>
        /// <param name="y">Vertical position</param>
        /// <param name="length">Worms length</param>
        /// <param name="color">Worms color</param>
        /// <param name="directions">Movement instructions for the worm</param>
        /// <returns>The spawned worm</returns>
        public Worm Spawn(Manager manager, float x, float y, int length, Color color)
        {
            this.manager = manager;
            Length = length;
            Position = new Vector2(x, y);
            Target = Position;
            Graphic.Color = color;
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
                    Move(0, -size);
                    break;
                case "LEFT":
                    Move(-size, 0);
                    break;
                case "DOWN":
                    Move(0, size);
                    break;
                case "RIGHT":
                    Move(size, 0);
                    break;
            }
        }


        /// <summary>
        /// Moves the worm the desired amount
        /// </summary>
        /// <param name="x">Horizontal movement</param>
        /// <param name="y">Vertical movement</param>
        private void Move(float x, float y)
        {
            if (manager.WormCollision(Target + new Vector2(x, y)))
                MoveWorm(x, y);
        }
    }
}
