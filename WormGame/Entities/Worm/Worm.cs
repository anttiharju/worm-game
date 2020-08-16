using Otter.Core;
using Otter.Graphics;
using Otter.Utility.MonoGame;
using WormGame.Core;
using WormGame.Pooling;
using WormGame.Static;

namespace WormGame.Entities
{
    public class Worm : Poolable
    {
        private readonly Pooler<WormModule> modules;
        private readonly Collision collision;
        private readonly WormScene scene;

        private WormModule firstModule;
        private WormModule lastModule;
        private Vector2 position;
        private Vector2 direction;
        private int halfSize;
        private int size;
        private int maxLength;
        private bool directionChange;
        private bool moving;


        public Player Player { get; set; }


        public Vector2 Direction
        {
            get { return direction; }
            set
            {
                if (Help.ValidateDirection(collision, position, size, value)) { if (value != direction) directionChange = true; direction = value; }
            }
        }
        public Vector2 Position { get { return position; } }


        public int Length { get; private set; }


        public Color Color { get; private set; }


        public Worm(Config config, WormScene scene, Pooler<WormModule> modules)
        {
            this.scene = scene;
            this.modules = modules;
            collision = config.collision;
            halfSize = config.halfSize;
            size = config.size;
        }

        public Worm Spawn(int x, int y, int length, Color color)
        {
            position.X = collision.EntityX(x);
            position.Y = collision.EntityY(y);
            Length = 1;
            maxLength = length;
            Color = color;
            direction = Random.ValidDirection(collision, position, size);
            firstModule = modules.Enable().Initialize(position, direction, Color);
            lastModule = firstModule;
            directionChange = false;
            return null;
        }


        public void Update()
        {
            bool retry = false;
        Retry:
            if (moving)
            {
                if (directionChange)
                {
                    firstModule.Next = modules.Enable().Initialize(position, direction, Color);
                    firstModule = firstModule.Next;
                    directionChange = false;
                }
                Vector2 nextPosition = position + direction * size;
                if (collision.Check(nextPosition) == collision.empty)
                {
                    position = nextPosition;
                    firstModule?.Grow();
                    //collision.Add(this, position);
                    Length++;
                    if (Length > maxLength)
                    {
                        lastModule.Shrink();
                        //collision.Add(null, lastModule.GetEnd());
                        Length--;
                        if (lastModule.Scale == 1)
                        {
                            WormModule tmp = lastModule;
                            lastModule = lastModule.Next;
                            tmp.Disable();
                        }
                    }
                }
                else
                {
                    if (retry)
                    {
                        moving = false;
                    }
                    else if (Player == null)
                    {
                        direction = Random.ValidDirection(collision, position, size);
                        directionChange = true;
                        retry = true;
                        goto Retry;
                    }
                }
            }
        }

        /// <summary>
        /// Disable worm.
        /// </summary>
        /// <param name="recursive">Disable recursively. False only when disabling is done by pooler.</param>
        public override void Disable(bool recursive = true)
        {
            base.Disable();
            firstModule = null;
            Player = null;
            direction.X = 0;
            direction.Y = 0;
            directionChange = false;
            moving = true;
        }
    }
}