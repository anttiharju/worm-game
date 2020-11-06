using Otter.Graphics;
using Otter.Graphics.Drawables;
using Otter.Utility.MonoGame;
using WormGame.Core;
using WormGame.Pooling;
using WormGame.Static;

namespace WormGame.Entities
{
    /// @author Antti Harju
    /// @version v0.5
    /// <summary>
    /// Custom pooler, manages worms. Uses a surface with autoclear off for efficient rendering.
    /// </summary>
    public class Worms : Pooler<Worm>
    {
        private readonly Surface surface;
        private readonly Pooler<WormModule> modules;


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="settings">Settings</param>
        /// <param name="scene">WormScene</param>
        public Worms(Settings settings, WormScene scene) : base(settings.wormAmount)
        {
            surface = settings.surface;
            modules = new Pooler<WormModule>(settings, scene, settings.moduleAmount);

            for (int i = 0; i < settings.wormAmount; i++)
            {
                Worm worm = new Worm(settings, scene, modules);
                worm.Disable(false);
                worm.Add(scene);
                pool[i] = worm;
            }
        }


        /// <summary>
        /// Clear surface and reset poolers.
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


    /// @author Antti Harju
    /// @version v0.5
    /// <summary>
    /// Worm entity. Worms are modular entities; it consists of one Otter2d entity and several regular objects (modules). This way it can grow almost infinitely.
    /// </summary>
    public class Worm : PoolableEntity
    {
        public WormModule firstModule;

        private readonly Pooler<WormModule> modules;
        private readonly Collision collision;
        private readonly WormScene scene;
        private readonly Image eraser;
        private readonly Image head;
        private readonly float step;
        private readonly int halfSize;
        private readonly int size;
        private readonly bool disableWorms;

        private WormModule lastModule;
        private WormModule newModule;
        private Vector2 direction;
        private Vector2 target;
        private int LengthCap;
        private bool moving;
        private bool retry;
        private bool grow;


        /// <summary>
        /// Player. If this is null the worm is controlled by a simple AI.
        /// </summary>
        public Player Player { get; set; }


        /// <summary>
        /// Worm length.
        /// </summary>
        public int Length { get; private set; }


        /// <summary>
        /// Worm color.
        /// </summary>
        public Color Color { get { return head.Color; } }


        /// <summary>
        /// Worm direction.
        /// </summary>
        public Vector2 Direction { get { return direction; } set { if (Random.ValidateDirection(collision, firstModule.Target, size, value)) direction = value; } }


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="settings">Settings</param>
        /// <param name="scene">WormScene</param>
        /// <param name="modules">Worm module pooler</param>
        public Worm(Settings settings, WormScene scene, Pooler<WormModule> modules)
        {
            disableWorms = settings.disableWorms;
            this.scene = scene;
            this.modules = modules;
            collision = settings.collision;
            halfSize = settings.halfSize;
            size = settings.size;
            step = settings.step;
            eraser = Image.CreateRectangle(size, Colors.background);
            head = Image.CreateRectangle(size);
            eraser.CenterOrigin();
            head.CenterOrigin();
            Surface = settings.surface;
            AddGraphic(eraser);
            AddGraphic(head);
        }


        /// <summary>
        /// Spawn worm.
        /// </summary>
        /// <param name="x">Horizontal field position</param>
        /// <param name="y">Vertical field position</param>
        /// <param name="length">Worm length</param>
        /// <param name="color">Worm color</param>
        /// <returns>Worm or null</returns>
        public Worm Spawn(int x, int y, int length, Color color)
        {
            LengthCap = 1;
            Length = 1;
            moving = true;

            head.Color = color;

            firstModule = modules.Enable();
            if (firstModule == null)
            {
                Disable(false);
                return null;
            }
            firstModule.Position = new Vector2(collision.EntityX(x), collision.EntityY(y));
            firstModule.SetTarget(collision.EntityX(x), collision.EntityY(y));
            eraser.X = -size;
            eraser.Y = -size;
            head.SetPosition(firstModule.Position);

            lastModule = firstModule;
            for (int i = 1; i < length; i++)
                Grow();

            direction = Random.ValidDirection(collision, Position, size);
            collision.Set(this, x, y);
            return this;
        }


        /// <summary>
        /// Grow worm by adding one module to it.
        /// </summary>
        private void Grow()
        {
            newModule = modules.Enable();
            if (newModule == null) return;
            newModule.Position = lastModule.Position;
            newModule.SetTarget(lastModule.Target);
            lastModule.ResetDirection();
            lastModule.Next = newModule;
            lastModule = newModule;
            LengthCap++;
        }


        /// <summary>
        /// Update worm and its modules. Kind of messy.
        /// </summary>
        public void Move()
        {
            if (grow)
                Grow();
            grow = false;
            moving = true;
            retry = false;
        Retry:
            target = firstModule.Target + Direction * size;
            int nextPosition = collision.GetType(target, true);
            if (nextPosition >= collision.fruit) // Move if next position is empty (4) or fruit (3).
            {
                if (Length < LengthCap)
                    Length++;
                else
                    collision.Set(null, lastModule.Target);
                if (nextPosition == collision.fruit)
                    grow = true;
                firstModule.DirectionFollow(direction);
                firstModule.TargetFollow(target);
                collision.Set(this, target);
            }
            else
            {
                if (retry) // If stuck, turn into a block.
                {
                    if (disableWorms)
                    {
                        scene.SpawnBlock(this);
                        Disable();
                    }
                }
                else if (Player == null) // Find a new direction if not posessed by player.
                {
                    direction = Random.ValidDirection(collision, firstModule.Target, size);
                    retry = true;
                    goto Retry;
                }
                moving = false;
            }
            Eraser();
        }


        /// <summary>
        /// Setup eraser.
        /// </summary>
        private void Eraser()
        {
            eraser.SetPosition(lastModule.Position);
            eraser.Scale = 1;
            if (lastModule.Direction.X == 0)
            {
                if (lastModule.Direction.Y < 0)
                {
                    eraser.SetOrigin(halfSize, size);
                    eraser.Y += halfSize;
                }
                else
                {
                    eraser.SetOrigin(halfSize, 0);
                    eraser.Y -= halfSize;
                }
                eraser.ScaledHeight = 0;
            }
            else
            {
                if (lastModule.Direction.X < 0)
                {
                    eraser.SetOrigin(size, halfSize);
                    eraser.X += halfSize;
                }
                else
                {
                    eraser.SetOrigin(0, halfSize);
                    eraser.X -= halfSize;
                }
                eraser.ScaledWidth = 0;
            }
        }


        /// <summary>
        /// Update graphics.
        /// </summary>
        public new void Update()
        {
            if (moving)
            {
                firstModule.PositionFollow();
                head.SetPosition(firstModule.Position);
                eraser.ScaledHeight += SimpleMath.Abs(lastModule.Direction.Y) * step;
                eraser.ScaledWidth += SimpleMath.Abs(lastModule.Direction.X) * step;
            }
        }


        /// <summary>
        /// Disable worm.
        /// </summary>
        /// <param name="recursive">Disable recursively. False only when disabling is done by pooler.</param>
        public override void Disable(bool recursive = true)
        {
            base.Disable();
            if (recursive)
                firstModule.Disable();
            moving = false;
            target.X = 0;
            target.Y = 0;
        }
    }


    /// @author Antti Harju
    /// @version 14.08.2020
    /// <summary>
    /// WormModule. Worm has one these per length unit.
    /// </summary>
    public class WormModule : Poolable
    {
        private readonly Collision collision;
        private readonly float step;


        /// <summary>
        /// Next worm module.
        /// </summary>
        public WormModule Next { get; set; }


        /// <summary>
        /// Worm module position.
        /// </summary>
        public Vector2 Position { get { return position; } set { position = value; } }
        private Vector2 position;


        /// <summary>
        /// Worm module direction.
        /// </summary>
        public Vector2 Direction { get { return direction; } set { direction = value; } }
        private Vector2 direction;


        /// <summary>
        /// Worm module target.
        /// </summary>
        public Vector2 Target { get { return target; } set { target = value; } }
        private Vector2 target;


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="settings">Settings</param>
        public WormModule(Settings settings)
        {
            collision = settings.collision;
            step = settings.step;
        }


        /// <summary>
        /// Recursively update worms every modules direction.
        /// </summary>
        /// <param name="newDirection">New direction</param>
        public void DirectionFollow(Vector2 newDirection)
        {
            if (Next != null)
                Next.DirectionFollow(Direction);
            Direction = newDirection;
        }


        /// <summary>
        /// Recursively update worms every modules position.
        /// </summary>
        public void PositionFollow()
        {
            position += Direction * step;
            if (Next != null)
                Next.PositionFollow();
        }


        /// <summary>
        /// Recursively update worm every modules target.
        /// </summary>
        /// <param name="newTarget">New target for worm body</param>
        public void TargetFollow(Vector2 newTarget)
        {
            if (Next != null)
                Next.TargetFollow(Target);
            Target = newTarget;
        }


        /// <summary>
        /// Reset worm module direction.
        /// </summary>
        public void ResetDirection()
        {
            direction.X = 0;
            direction.Y = 0;
        }


        /// <summary>
        /// Set worm module target.
        /// </summary>
        /// <param name="target">Target</param>
        public void SetTarget(Vector2 target)
        {
            SetTarget(target.X, target.Y);
        }


        /// <summary>
        /// Set worm module target.
        /// </summary>
        /// <param name="x">Horizontal target</param>
        /// <param name="y">Vertical target</param>
        public void SetTarget(float x, float y)
        {
            target.X = x;
            target.Y = y;
        }


        /// <summary>
        /// Disable module.
        /// </summary>
        /// <param name="recursive">Disable recursively. False only when disabling is done by pooler.</param>
        public override void Disable(bool recursive = true)
        {
            base.Disable();
            if (recursive && Next != null)
                Next.Disable();
            Next = null;
            if (collision.GetType(target) == collision.worm)
                collision.Set(null, target);
            ResetDirection();
            position.X = 0;
            position.Y = 0;
            target.X = 0;
            target.Y = 0;
        }
    }
}
