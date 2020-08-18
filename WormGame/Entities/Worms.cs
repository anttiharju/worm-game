using Otter.Graphics;
using Otter.Graphics.Drawables;
using Otter.Utility.MonoGame;
using WormGame.Core;
using WormGame.Static;
using WormGame.Pooling;

namespace WormGame.Entities
{
    /// @author anvemaha
    /// @version 17.08.2020
    /// <summary>
    /// Custom pooler for worms that also has a WormModule pooler.
    /// </summary>
    public class Worms : Pooler<Worm>
    {
        private readonly Surface surface;
        private readonly Pooler<WormModule> modules;


        /// <summary>
        /// Initialize poolers.
        /// </summary>
        /// <param name="config">Configuration</param>
        /// <param name="scene">Worm scene</param>
        public Worms(Config config, WormScene scene) : base(config.wormAmount)
        {
            surface = config.surface;
            modules = new Pooler<WormModule>(scene, config, config.moduleAmount);

            for (int i = 0; i < config.wormAmount; i++)
            {
                Worm worm = new Worm(config, scene, modules);
                worm.Disable(false);
                worm.Add(scene);
                pool[i] = worm;
            }
        }


        /// <summary>
        /// Reset poolers and clear surface.
        /// </summary>
        public override void Reset()
        {
            surface.Clear();
            modules.Reset();
            base.Reset();
        }


        /// <summary>
        /// Spawn a worm.
        /// </summary>
        /// <param name="x">Horizontal grid position</param>
        /// <param name="y">Vertical grid position</param>
        /// <param name="length">Worm length</param>
        /// <param name="color">Worm color</param>
        /// <returns>Worm or null</returns>
        public Worm SpawnWorm(int x, int y, int length, Color color)
        {
            Worm worm = Enable();
            if (worm == null) return null;
            return worm.Spawn(x, y, length, color);
        }
    }


    /// @author anvemaha
    /// @version 14.08.2020
    /// <summary>
    /// Worm entity. Worms are modular entities; it consists of one Otter2d entity and several regular objects (modules). This way the worm can grow infinitely.
    /// </summary>
    public class Worm : Poolable
    {
        public WormModule firstModule;

        private readonly Pooler<WormModule> modules;
        private readonly Collision collision;
        private readonly WormScene scene;
        private readonly int size;

        private WormModule lastModule;
        private Vector2 position;
        private Vector2 direction;
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
            this.modules = modules;
            this.scene = scene;
            collision = config.collision;
            size = config.size;
        }

        public Worm Spawn(int x, int y, int length, Color color)
        {
            position.X = collision.EntityX(x);
            position.Y = collision.EntityY(y);
            maxLength = length;
            Color = color;
            direction = Random.ValidDirection(collision, position, size);
            firstModule = modules.Enable().Initialize(position, direction, Color);
            lastModule = firstModule;
            directionChange = false;
            return null;
        }


        public void Move()
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
                    collision.Add(this, position);
                    Length++;
                    if (Length > maxLength)
                    {
                        lastModule.Shrink();
                        collision.Add(null, lastModule.GetEnd());
                        Length--;
                        if (lastModule.Scale <= 1)
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


        public void Update()
        {

        }


        /// <summary>
        /// Disable worm.
        /// </summary>
        /// <param name="recursive">Disable recursively. False only when disabling is done by pooler.</param>
        public override void Disable(bool recursive = true)
        {
            base.Disable();
            firstModule = null;
            lastModule = null;
            position.X = 0;
            position.Y = 0;
            direction.X = 0;
            direction.Y = 0;
            Player = null;
            maxLength = 0;
            directionChange = false;
            moving = true;
            Length = 1;
        }
    }


    /// @author anvemaha
    /// @version 14.08.2020
    /// <summary>
    /// WormModule. Thanks to modularity worm length can be increased during runtime.
    /// </summary>
    public class WormModule : PoolableEntity
    {
        private readonly float halfSize;

        private Vector2 direction;
        public WormModule Next { get; set; }
        public float Scale { get { if (direction.X == 0) return Graphic.ScaleY; else return Graphic.ScaleX; } }


        public WormModule(Config config)
        {
            Graphic = Image.CreateRectangle(config.size);
            Graphic.CenterOrigin();
            halfSize = config.halfSize;
        }

        public WormModule Initialize(Vector2 position, Vector2 direction, Color color)
        {
            SetPosition(position);
            this.direction = direction;
            Graphic.Color = color;
            return this;
        }


        public void Grow()
        {
            if (direction.X == 0)
                Graphic.ScaleY++;
            else
                Graphic.ScaleX++;
            Move();
        }


        public void Move()
        {
            Position += direction * halfSize;
        }


        public Vector2 GetEnd()
        {
            return Position - halfSize * direction - Scale * halfSize * direction;
        }


        public void Shrink()
        {
            if (direction.X == 0)
                Graphic.ScaleY--;
            else
                Graphic.ScaleX--;
            Move();
        }


        public override void Disable(bool recursive = true)
        {
            base.Disable();
            Graphic.ScaleX = 1;
            Graphic.ScaleY = 1;
            direction.X = 0;
            direction.Y = 0;
            Next = null;
        }
    }
}
