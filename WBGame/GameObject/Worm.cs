using Otter;
using System;
using WBGame.Other;

namespace WBGame.GameObject
{
    /// @author Antti Harju
    /// @version 20.06.2020
    /// <summary>
    /// The worm class
    /// </summary>
    class Worm : Tail
    {
        private Manager manager;
        private int wormLength;
        private readonly int size;
        private readonly Queue queue;

        /// <summary>
        /// Head constructor. Calls Body constructor.
        /// </summary>
        /// <param name="size">Diameter of the circle graphic</param>
        public Worm(int size) : base(size)
        {
            this.size = size;
            queue = new Queue();
        }


        /// <summary>
        /// Spawns the head
        /// </summary>
        /// <param name="manager">Manager</param>
        /// <param name="x">Horizontal position</param>
        /// <param name="y">Vertical position</param>
        /// <param name="length">Worms length</param>
        /// <param name="color">Worms color</param>
        /// <returns></returns>
        public Worm Spawn(Manager manager, float x, float y, int length, Color color)
        {
            this.manager = manager;
            Enable();
            wormLength = length;
            Position = new Vector2(x, y);
            SetTarget(x, y);
            Color = color;
            return this;
        }

        public Queue GetQueue()
        {
            return queue;
        }

        internal void EatQueue()
        {
            switch (queue.Get())
            {
                case 1:
                    Move(0, -size);
                    break;
                case 2:
                    Move(-size, 0);
                    break;
                case 3:
                    Move(0, size);
                    break;
                case 4:
                    Move(size, 0);
                    break;
            }
        }

        /// <summary>
        /// Moves the worm when a key is pressed and changes color of the worms head
        /// </summary>
        /// <param name="key">Key</param>
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
        public int GetLength()
        {
            return wormLength;
        }
    }
}
