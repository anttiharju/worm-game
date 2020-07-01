using Otter;
using WormGame.Help;
using WormGame.Other;

namespace WormGame.GameObject
{
    /// @author Antti Harju
    /// @version 01.07.2020
    /// <summary>
    /// The worm class. Technically it's just the head entity but it manages the entire worm.
    /// </summary>
    class Worm : WormTail
    {
        private readonly WormTail[] worm;
        private readonly int size;

        private PlayArea playArea;
        private Collision collision;
        private Vector2 newTarget;

        public override Color Color { get { return Graphic.Color ?? null; } set { SetColor(value); } }
        public string Direction { private get; set; }
        public bool Posessed { get; set; }
        public bool Noclip { private get; set; }
        public int Length { get; private set; }

        /// <summary>
        /// Head constructor. Calls Body constructor.
        /// </summary>
        /// <param name="size">Diameter of the circle graphic</param>
        public Worm(int size) : base(size)
        {
            this.size = size;
            Direction = "UP";
            worm = new WormTail[Config.maxWormLength];
            newTarget = new Vector2();
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
        public Worm Spawn(Pooler<WormTail> tails, Collision collision, PlayArea playArea, int x, int y, int length, Color color, string direction)
        {
            // Tail
            int tailCount = length - 1; // minus one because head already counts as 1
            WormTail current = this;
            for (int i = 0; i < tailCount; i++)
            {
                worm[i] = current;
                WormTail next = tails.Enable();
                if (next == null) return null;
                next.X = x;
                next.Y = y;
                next.target = next.Position;
                current.Next = next;
                current = next;
            }
            worm[tailCount] = current;

            // Head
            this.collision = collision;
            this.playArea = playArea;
            X = x;
            Y = y;
            Length = length;
            target = Position;
            Direction = direction;
            playArea.Update(target, this);
            Color = color;

            return this;
        }


        /// <summary>
        /// Activates the worm to move
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
            newTarget.X = target.X + deltaX; newTarget.Y = target.Y + deltaY;

            if (collision.WormCheck(this, newTarget, Noclip))
                Move(deltaX, deltaY);
            else if (!Posessed)
                Direction = Random.Direction();
        }


        /// <summary>
        /// Disables the worm
        /// </summary>
        public override void Disable()
        {
            for (int i = 0; i < worm.Length; i++)
            {
                playArea.Update(target, null);
                worm[i].Enabled = false;
            }
        }


        /// <summary>
        /// Sets worms color
        /// </summary>
        /// <param name="color"></param>
        public void SetColor(Color color)
        {
            for (int i = 0; i < worm.Length; i++)
                worm[i].Graphic.Color = color;
        }


#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
        /// <summary>
        /// Indexer for the worm
        /// </summary>
        /// <param name="i">index</param>
        /// <returns>Worms body part at index</returns>
        public WormTail this[int i]
        {
            get => worm[i];
            set => worm[i] = value;
        }
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
    }
}
