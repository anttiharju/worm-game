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
    public class Worm : PoolableEntity
    {
        public WormModule firstModule;

        private readonly Pooler<WormModule> modules;
        private readonly Collision collision;
        private readonly WormScene scene;
        private readonly Image eraser;
        private readonly Image head;
        private readonly int size;

        private WormModule lastModule;
        private Vector2 direction;
        private Vector2 target;
        private int maxLength;
        private bool directionChange;
        private bool moving;


        public Player Player { get; set; }


        public Vector2 Direction
        {
            get { return direction; }
            set
            {
                if (Help.ValidateDirection(collision, target, size, value)) { if (value != direction) directionChange = true; direction = value; }
            }
        }


        public int Length { get; private set; }


        public Color Color { get { return head.Color; } }


        public Vector2 Target { get { return target; } }


        public Worm(Config config, WormScene scene, Pooler<WormModule> modules)
        {
            this.modules = modules;
            this.scene = scene;
            collision = config.collision;
            size = config.size;
            eraser = Image.CreateRectangle(size, config.backgroundColor);
            head = Image.CreateRectangle(size);
            eraser.CenterOrigin();
            head.CenterOrigin();
            Surface = config.surface;
            AddGraphic(eraser);
            AddGraphic(head);
        }

        public Worm Spawn(int x, int y, int length, Color color)
        {
            target.X = collision.EntityX(x);
            target.Y = collision.EntityY(y);
            head.SetPosition(target);
            maxLength = length;
            head.Color = color;
            direction = Random.ValidDirection(collision, target, size);
            firstModule = modules.Enable().Initialize(target, direction);
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
                    firstModule.Next = modules.Enable().Initialize(target, direction);
                    firstModule = firstModule.Next;
                    directionChange = false;
                }
                Vector2 nextTarget = target + direction * size;
                if (collision.Check(nextTarget) == collision.empty)
                {
                    target = nextTarget;
                    firstModule?.Grow();
                    collision.Add(this, target);
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
                        direction = Random.ValidDirection(collision, target, size);
                        directionChange = true;
                        retry = true;
                        goto Retry;
                    }
                }
            }
            head.SetPosition(target);
            eraser.SetPosition(lastModule.GetEnd());
        }


        public new void Update()
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
            target.X = 0;
            target.Y = 0;
            direction.X = 0;
            direction.Y = 0;
            Player = null;
            maxLength = 0;
            directionChange = false;
            moving = true;
            Length = 1;
        }
    }


    /// @author Antti Harju
    /// @version 20.08.2020
    /// <summary>
    /// Worm module. Scaled as needed by Worm.
    /// </summary>
    public class WormModule : Poolable
    {
        private readonly float halfSize;

        private Vector2 scale;
        private Vector2 position;
        private Vector2 direction;
        public WormModule Next { get; set; }
        public float Scale { get { if (direction.X == 0) return scale.Y; else return scale.X; } set { if (direction.X == 0) scale.Y = value; else scale.X = value; } }


        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="config"></param>
        public WormModule(Config config)
        {
            halfSize = config.halfSize;
        }


        /// <summary>
        /// Disable module.
        /// </summary>
        /// <param name="recursive"></param>
        public override void Disable(bool recursive = true)
        {
            base.Disable();
            scale.X = 1;
            scale.Y = 1;
            Next = null;
        }


        /// <summary>
        /// Get module end position.
        /// </summary>
        /// <returns>Module end position</returns>
        public Vector2 GetEnd()
        {
            return position - Scale * halfSize * direction + halfSize * direction;
        }


        /// <summary>
        /// Grow module and update position.
        /// </summary>
        public void Grow()
        {
            Scale++;
            Move();
        }


        /// <summary>
        /// Initialize module.
        /// </summary>
        /// <param name="position">Start entity position</param>
        /// <param name="direction">Worm direction</param>
        /// <returns>Itself</returns>
        public WormModule Initialize(Vector2 position, Vector2 direction)
        {
            this.position = position;
            this.direction = direction;
            return this;
        }


        /// <summary>
        /// Update position.
        /// </summary>
        public void Move()
        {
            position += direction * halfSize;
        }


        /// <summary>
        /// Shrink module and update position.
        /// </summary>
        public void Shrink()
        {
            Scale--;
            Move();
        }
    }
}
