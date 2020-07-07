using Otter.Graphics;
using Otter.Utility.MonoGame;
using WormGame.Static;
using WormGame.Manager;

namespace WormGame.GameObject
{
    /// @author Antti Harju
    /// @version 01.07.2020
    /// <summary>
    /// The worm class. Technically it's just the head entity but it manages the entire worm.
    /// </summary>
    class Worm : WormBase
    {
        private readonly WormBase[] worm;
        private readonly int size;

        private Collision field;
        private Vector2 direction = new Vector2();
        private Vector2 tmpTarget = new Vector2();

        public bool Posessed { get; set; }
        public int Length { get; private set; }
        public override Color Color { get { return Graphic.Color ?? null; } set { SetColor(value); } }
        public Vector2 Direction { get { return direction; } set { if (Help.ValidateDirection(field, target, value * size)) direction = value; } }

        /// <summary>
        /// Head constructor. Calls Body constructor.
        /// </summary>
        /// <param name="size">Diameter of the circle graphic</param>
        public Worm(int size, Config config) : base(size, config)
        {
            this.size = size;
            worm = new WormBase[config.maxWormLength];
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
        public Worm Spawn(Pooler<WormBase> tails, Collision field, int x, int y, int length, Color color)
        {
            // Tail
            int tailCount = length - 1; // ignore head
            WormBase current = this;
            for (int i = 0; i < tailCount; i++)
            {
                worm[i] = current;
                WormBase next = tails.Enable();
                if (next == null) return null;
                next.X = x;
                next.Y = y;
                next.target = next.Position;
                current.Next = next;
                current = next;
            }
            worm[tailCount] = current;

            SetHead(this);

            // Head
            this.field = field;
            X = x;
            Y = y;
            Length = length;
            target = Position;
            Direction = Help.directions[3];
            this.field.Update(target, this);
            Color = color;

            return this;
        }


        /// <summary>
        /// Moves the worm the desired amount
        /// </summary>
        public void Move()
        {
            bool retry = false;
        Retry:
            tmpTarget = target + Direction * size;
            if (field.Check(tmpTarget))
            {
                field.Update(worm[^1].target, null);
                for (int i = worm.Length - 1; i > 0; i--)
                    field.Update(worm[i - 1].target, worm[i]);
                field.Update(tmpTarget, worm[0]);
                TailFollow(tmpTarget);
            }
            else if (!Posessed && !retry)
            {
                Direction = Random.ValidDirection(field, target, size);
                retry = true;
                goto Retry;
            }
        }


        public void UpdateTargets(Vector2 next)
        {
            for (int i = worm.Length - 1; i > 0; i--)
            {
                worm[i].target = worm[i - 1].target;
            }
            worm[0].target = next;
        }

        /// <summary>
        /// Disables the worm
        /// </summary>
        public override void Disable()
        {
            for (int i = 0; i < Length; i++)
            {
                field.Update(worm[i].target, null);
                worm[i].Enabled = false;
            }
        }


        /// <summary>
        /// Sets worms color
        /// </summary>
        /// <param name="color"></param>
        public void SetColor(Color color)
        {
            for (int i = 0; i < Length; i++)
                worm[i].Graphic.Color = color;
        }


#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
        /// <summary>
        /// Indexer for the worm
        /// </summary>
        /// <param name="i">index</param>
        /// <returns>Worms body part at index</returns>
        public WormBase this[int i]
        {
            get => worm[i];
            set => worm[i] = value;
        }
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
    }
}
