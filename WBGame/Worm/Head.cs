using Otter;
using WBGame.Pooling;
using WBGame.Other;

namespace WBGame.Worm
{
    /// @author Antti Harju
    /// @version 15.06.2020
    /// <summary>
    /// The worm class
    /// </summary>
    class Head : Body
    {
        private Manager manager;
        private int length;

        /// <summary>
        /// Head constructor. The x and y don't really matter because we have Spawn()
        /// </summary>
        /// <param name="size">Diameter of the circle graphic</param>
        public Head(int size) : base(size) { }

        /// <summary>
        /// Spawns the worm with the desired configuration. Can't be done in constructor because of how I decided to implement the pooling system.
        /// </summary>
        /// <param name="posessed">Is the worm controlled by a player or not</param>
        /// <param name="x">Horizontal position</param>
        /// <param name="y">Vertical position</param>
        /// <param name="wantedLength">How long the worm should be</param>
        /// <param name="bodyPool">Required for spawning the tail and collision</param>
        /// <param name="headPool">Required for collision</param>
        /// <param name="color">Worms color</param>
        public Head Spawn(Manager manager, float x, float y, int length, Color color)
        {
            this.manager = manager;
            Enable();
            this.length = length;
            Position = new Vector2(x, y);
            SetTarget(x, y);
            SetColor(color);
            return this;
        }

        /// <summary>
        /// Worm controls are set up here
        /// </summary>
        public override void Update()
        {
            if (!Enabled()) return;
            base.Update();

            Move(Key.W, 0, -GetSize());
            Move(Key.S, 0, GetSize());
            Move(Key.A, -GetSize(), 0);
            Move(Key.D, GetSize(), 0);
        }

        /// <summary>
        /// Moves the worm when a key is pressed and changes color of the worms head
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="x">horizontal movement</param>
        /// <param name="y">vertical movement</param>
        /// <param name="color">color of the worms head when the key is pressed</param>
        private void Move(Key key, float x, float y)
        {
            if (manager.CanMove(this, Position + new Vector2(x, y)))
                if (Input.KeyPressed(key))
                    MoveWorm(x, y);
        }

        public int GetLength()
        {
            return length;
        }
    }
}
