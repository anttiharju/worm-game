﻿using Otter.Graphics;
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
    /// TODO: Erase on disable
    public class Worm : PoolableEntity
    {
#if DEBUG
        private readonly bool blockifyWorms;
#endif
        public WormModule firstModule;

        private readonly Pooler<WormModule> modules;
        private readonly Collision collision;
        private readonly WormScene scene;
        private readonly Image eraser;
        private readonly Image head;
        private readonly float step;
        private readonly int halfSize;
        private readonly int size;

        private WormModule lastModule;
        private WormModule newModule;
        private Vector2 target;
        private bool moving;
        private bool retry;
        private bool grow;
        private int LengthCap;


        /// <summary>
        /// Get or set player. Wheter or not this is null tells us wheter or not worm is posessed.
        /// </summary>
        public Player Player { get; set; }


        /// <summary>
        /// Get worm length.
        /// </summary>
        public int Length { get; private set; }


        /// <summary>
        /// Get worm color.
        /// </summary>
        public Color Color { get { return head.Color; } }


        /// <summary>
        /// Get or set worm direction.
        /// </summary>
        public Vector2 Direction { get { return direction; } set { if (Help.ValidateDirection(collision, firstModule.Target, size, value)) direction = value; } }
        private Vector2 direction;


        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="config">Configuration</param>
        public Worm(Config config, WormScene scene, Pooler<WormModule> modules)
        {
#if DEBUG
            blockifyWorms = config.disableWorms;
#endif
            this.scene = scene;
            this.modules = modules;
            collision = config.collision;
            halfSize = config.halfSize;
            size = config.size;
            step = config.step;
            eraser = Image.CreateRectangle(size, config.backgroundColor);
            head = Image.CreateRectangle(size);
            eraser.CenterOrigin();
            head.CenterOrigin();
            Surface = config.surface;
            AddGraphic(eraser);
            AddGraphic(head);
        }


        /// <summary>
        /// Spawn the worm.
        /// </summary>
        /// <param name="x">Horizontal field position</param>
        /// <param name="y">Vertical field position</param>
        /// <param name="length">Worm length</param>
        /// <param name="color">Worm color</param>
        /// <returns>Worm</returns>
        public Worm Spawn(int x, int y, int length, Color color)
        {
            LengthCap = 1;
            Length = 1;
            moving = true;

            head.Color = color;

            firstModule = modules.Enable();
            firstModule.Position = new Vector2(collision.EntityX(x), collision.EntityY(y));
            firstModule.SetTarget(collision.EntityX(x), collision.EntityY(y));
            eraser.X = -size;
            eraser.Y = -size;
            head.SetPosition(firstModule.Position);

            lastModule = firstModule;
            for (int i = 1; i < length; i++)
                Grow();

            direction = Random.ValidDirection(collision, Position, size);
            collision.Add(this, x, y);
            return this;
        }


        /// <summary>
        /// Grow worm by one module.
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
        /// Update worm directions, targets and collision.
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
            int nextPosition = collision.Check(target, true);
            if (nextPosition >= collision.fruit) // Move if next position is empty (4) or fruit (3).
            {
                if (Length < LengthCap)
                    Length++;
                else
                    collision.Add(null, lastModule.Target);
                if (nextPosition == collision.fruit)
                    grow = true;
                firstModule.DirectionFollow(direction);
                firstModule.TargetFollow(target);
                collision.Add(this, target);
            }
            else
            {
                if (retry) // If stuck, turn into a block.
                {
#if DEBUG
                    if (blockifyWorms)
                    {
#endif
                        scene.SpawnBlock(this);
                        Disable();
#if DEBUG
                    }
#endif
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
        /// Setups eraser.
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
        /// Update entity position and recursively module graphic positions.
        /// </summary>
        public new void Update()
        {
            if (moving)
            {
                firstModule.GraphicFollow();
                head.SetPosition(firstModule.Position);
                eraser.ScaledHeight += FastMath.Abs(lastModule.Direction.Y) * step;
                eraser.ScaledWidth += FastMath.Abs(lastModule.Direction.X) * step;
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


    /// @author anvemaha
    /// @version 14.08.2020
    /// <summary>
    /// WormModule. Thanks to modularity worm length can be increased during runtime.
    /// </summary>
    public class WormModule : Poolable
    {
        private readonly Collision collision;
        private readonly float step;


        /// <summary>
        /// Get or set next module.
        /// </summary>
        public WormModule Next { get; set; }


        /// <summary>
        /// Get module graphic.
        /// </summary>
        public Vector2 Position { get { return position; } set { position = value; } }
        private Vector2 position;


        /// <summary>
        /// Get or set worm direction.
        /// </summary>
        public Vector2 Direction { get { return direction; } set { direction = value; } }
        private Vector2 direction;


        /// <summary>
        /// Get or set target position.
        /// </summary>
        public Vector2 Target { get { return target; } set { target = value; } }
        private Vector2 target;


        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="config">Configuration</param>
        public WormModule(Config config)
        {
            collision = config.collision;
            step = config.step;
        }


        /// <summary>
        /// Recursively update every worm module direction.
        /// </summary>
        /// <param name="previousDirection">Previous direction</param>
        public void DirectionFollow(Vector2 previousDirection)
        {
            if (Next != null)
                Next.DirectionFollow(Direction);
            Direction = previousDirection;
        }


        /// <summary>
        /// Recursively update every worm module graphic position.
        /// </summary>
        /// <param name="positionDelta">Worm entity position delta</param>
        /// <param name="step">Worm step</param>
        public void GraphicFollow()
        {
            position += Direction * step;
            if (Next != null)
                Next.GraphicFollow();
        }


        /// <summary>
        /// Recursively update every worm module target.
        /// </summary>
        /// <param name="newTarget">New target for worm body</param>
        public void TargetFollow(Vector2 newTarget)
        {
            if (Next != null)
                Next.TargetFollow(Target);
            Target = newTarget;
        }


        /// <summary>
        /// Reset module direction.
        /// </summary>
        public void ResetDirection()
        {
            direction.X = 0;
            direction.Y = 0;
        }

        /// <summary>
        /// Set module target.
        /// </summary>
        /// <param name="target">Target</param>
        public void SetTarget(Vector2 target)
        {
            SetTarget(target.X, target.Y);
        }

        public void SetTarget(float x, float y)
        {
            target.X = x;
            target.Y = y;
        }

        /// <summary>
        /// Recursively disable every one of worms modules.
        /// </summary>
        /// <param name="recursive">Disable recursively. False only when disabling is done by pooler.</param>
        public override void Disable(bool recursive = true)
        {
            base.Disable();
            if (recursive && Next != null)
                Next.Disable();
            Next = null;
            if (collision.Check(target) == collision.worm)
                collision.Add(null, target);
            ResetDirection();
            position.X = 0;
            position.Y = 0;
            target.X = 0;
            target.Y = 0;
        }
    }
}