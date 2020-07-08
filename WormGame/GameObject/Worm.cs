using Otter.Graphics;
using Otter.Utility.MonoGame;
using WormGame.Core;
using WormGame.Static;
using WormGame.Pooling;

namespace WormGame.GameObject
{
    public class Worm : BasicPoolable
    {
        private readonly Collision field;
        private readonly WormEntity[] worm;
        private readonly int size;

        private Vector2 nextTarget;
        private Vector2 direction;

        /// <summary>
        /// Worm position.
        /// </summary>
        public Vector2 Position { get { return worm[Length / 2].target; } }
        

        /// <summary>
        /// Worm direction.
        /// </summary>
        public Vector2 Direction { get { return direction; } set { if (Help.ValidateDirection(field, worm[0].target, size, value)) direction = value; } }
        

        /// <summary>
        /// Worm color.
        /// </summary>
        public Color Color { get { return worm[0].Color ?? null; } set { SetColor(value); } }
        
        
        /// <summary>
        /// Worms length. Use this instead of worm.Length.
        /// </summary>
        public int Length { get; private set; }


        /// <summary>
        /// Is the worm controlled by player or not.
        /// </summary>
        public bool Posessed { get; set; }


        /// <summary>
        /// Constructor. Takes size and collision from config and initializes WormEntity array with configs maxWormLength.
        /// </summary>
        /// <param name="config"></param>
        public Worm(Config config)
        {
            size = config.size;
            field = config.field;
            worm = new WormEntity[config.maxWormLength];
        }


        /// <summary>
        /// Creates the worm. Not done in constructor because we have object pooling.
        /// </summary>
        /// <param name="bodies">Pool containing WormEntities</param>
        /// <param name="field">Collision</param>
        /// <param name="x">Horizontal field position</param>
        /// <param name="y">Vertical field position</param>
        /// <param name="length">Worms length</param>
        /// <param name="color">Worms color</param>
        /// <param name="stationary">Should the worm start moving by itself or not</param>
        /// <returns>Spawned worm</returns>
        public Worm Spawn(Pool<WormEntity> bodies, Collision field, int x, int y, int length, Color color, bool stationary)
        {
            WormEntity previous = null;
            for (int i = 0; i < length; i++)
            {
                WormEntity current = bodies.Enable();
                if (current == null) return null;
                worm[i] = current;
                current.X = x; current.Y = y;
                current.target = current.Position;
                if (previous != null)
                    previous.Next = current;
                previous = current;
            }

            Length = length;
            field.Set(worm[0], worm[0].target);
            if (stationary)
                direction = Vector2.Zero;
            else
                direction = Random.ValidDirection(field, Position, size);
            Color = color;
            return this;
        }


        /// <summary>
        /// Updates worm to collision field and keeps the worm together.
        /// </summary>
        public void Move()
        {
            Stop(false);
            bool retry = false;
        Retry:
            nextTarget = worm[0].target + direction * size;
            if (field.Check(nextTarget))
            {
                field.Set(worm[Length - 1].target);
                worm[0].DirectionFollow(direction);
                worm[0].TargetFollow(nextTarget);
                field.Set(worm[0], nextTarget);
            }
            else
            {
                if (!Posessed && !retry)
                {
                    direction = Random.ValidDirection(field, worm[0].target, size);
                    retry = true;
                    goto Retry;
                }
                Stop();
            }
        }



        /// <summary>
        /// Stops/unstops the worm.
        /// </summary>
        /// <param name="stationary">Should the worm stop or go</param>
        private void Stop(bool stationary = true)
        {
            for (int i = 0; i < Length; i++)
                worm[i].Stationary = stationary;
        }


        /// <summary>
        /// Disables the worm.
        /// </summary>
        public override void Disable()
        {
            for (int i = 0; i < Length; i++)
            {
                field.Set(worm[i].target);
                worm[i].Enabled = false;
            }
            Enabled = false;
        }


        /// <summary>
        /// Sets the entire worms color.
        /// </summary>
        /// <param name="color"></param>
        public void SetColor(Color color)
        {
            for (int i = 0; i < Length; i++)
                worm[i].Graphic.Color = color;
        }


        /// <summary>
        /// Indexer so we can access all WormEntities of Worm.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public WormEntity this[int i]
        {
            get => worm[i];
            set => worm[i] = value;
        }
    }
}
