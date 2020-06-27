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
        private readonly Controls controls = new Controls();

        public override Color Color { get { return Graphic.Color ?? null; } set { Graphic.Color = value; SetColor(value); } }

        public int Length { get; private set; }

        /// <summary>
        /// Head constructor. Calls Body constructor.
        /// </summary>
        /// <param name="size">Diameter of the circle graphic</param>
        public Worm(int size) : base(size)
        {
            this.size = size;
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
        public Worm Spawn(Manager manager, float x, float y, int length, Color color, char[] directions = null)
        {
            this.manager = manager;
            Length = length;
            Position = new Vector2(x, y);
            SetTarget(x, y);
            Graphic.Color = color;
            if (directions != null)
                controls.AddMultiple(directions);
            return this;
        }


        /// <summary>
        /// Returns the movement queue of the worm so player can posess the worm
        /// </summary>
        /// <returns>Movement queue of the worm</returns>
        public Controls GrabControls()
        {
            return controls;
        }


        /// <summary>
        /// Interprets the movement queue
        /// </summary>
        public void Move()
        {
            switch (controls.Next())
            {
                case 'W': // up
                    Move(0, -size);
                    break;
                case 'A': // left
                    Move(-size, 0);
                    break;
                case 'S': // down
                    Move(0, size);
                    break;
                case 'D': // right
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
            if (manager.WormCollision(GetTarget() + new Vector2(x, y)))
                MoveWorm(x, y);
        }
    }
}
