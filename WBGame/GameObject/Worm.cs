using Otter;
using WBGame.Other;

namespace WBGame.GameObject
{
    /// @author Antti Harju
    /// @version 21.06.2020
    /// <summary>
    /// The worm class. Technically it's just the head entity but it does manage the entire worm.
    /// </summary>
    class Worm : Tail
    {
        private Manager manager;
        private int wormLength;
        private readonly int size;
        private readonly Controls controls;


        /// <summary>
        /// Head constructor. Calls Body constructor.
        /// </summary>
        /// <param name="size">Diameter of the circle graphic</param>
        public Worm(int size) : base(size)
        {
            this.size = size;
            controls = new Controls();
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
            Enable();
            wormLength = length;
            Position = new Vector2(x, y);
            SetTarget(x, y);
            Color = color;
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
            switch (controls.Get())
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
            if (manager.CanMove(this, Position + new Vector2(x, y)))
                MoveWorm(x, y);
        }


        /// <summary>
        /// Required for Manager.Blockify()
        /// </summary>
        /// <returns>Worms length</returns>
        /// TODO: Impelent as a field?
        public int GetLength()
        {
            return wormLength;
        }


        /// <summary>
        /// Sets the entire worms color (if you use the attribute worm.color you only set the heads color)
        /// </summary>
        /// <param name="color">Desired color</param>
        public void SetColor(Color color)
        {
            RecursiveColor(color);
        }
    }
}
