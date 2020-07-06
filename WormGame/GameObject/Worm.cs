using Otter.Graphics;
using Otter.Utility.MonoGame;
using System.Net.Http.Headers;
using WormGame.Help;
using WormGame.Other;

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
        private Vector2 newTarget;
        private int direction;

        public override Color Color { get { return Graphic.Color ?? null; } set { SetColor(value); } }
        public bool Posessed { get; set; }
        public int Length { get; private set; }
        public int Direction { private get { return direction; } set { if (ValidateDirection(target.X, target.Y, value)) direction = value; } }

        /// <summary>
        /// Head constructor. Calls Body constructor.
        /// </summary>
        /// <param name="size">Diameter of the circle graphic</param>
        public Worm(int size) : base(size)
        {
            this.size = size;
            worm = new WormBase[Config.maxWormLength];
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
        public Worm Spawn(Pooler<WormBase> tails, Collision field, int x, int y, int length, Color color, int direction)
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

            // Head
            this.field = field;
            X = x;
            Y = y;
            Length = length;
            target = Position;
            Direction = direction;
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
            int deltaX = 0;
            int deltaY = 0;
            switch (direction)
            {
                case 0:
                    deltaY -= size;
                    break;
                case 1:
                    deltaX -= size;
                    break;
                case 2:
                    deltaY += size;
                    break;
                case 3:
                    deltaX += size;
                    break;
            }
            newTarget.X = target.X + deltaX; newTarget.Y = target.Y + deltaY;

            if (field.Check(newTarget))
            {
                field.Update(worm[^1].target, null);
                for (int i = worm.Length - 1; i > 0; i--)
                    field.Update(worm[i - 1].target, worm[i]);
                field.Update(newTarget, worm[0]);
                Move(deltaX, deltaY);
            }
            else if (!Posessed && !retry)
            {
                Direction = RandomValidDirection();
                retry = true;
                goto Retry;
            }
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

        private int RandomValidDirection()
        {
            int direction = Random.Range(0, 4);
            for (int i = 0; i < 3; i++)
            {
                if (ValidateDirection(target.X, target.Y, direction))
                    break;
                else
                {
                    direction--;
                    if (direction < 0)
                        direction = 3;
                }
            }
            return direction;
        }


        private bool ValidateDirection(float x, float y, int direction)
        {
            int x1 = field.X(x);
            int y1 = field.Y(y);
            switch (direction)
            {
                case 0:
                    y1 += 1;
                    break;
                case 1:
                    x1 -= 1;
                    break;
                case 2:
                    y1 -= 1;
                    break;
                case 3:
                    x1 += 1;
                    break;
            }
            return field.Check(x1, y1);
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
