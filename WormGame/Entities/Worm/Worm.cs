using Otter.Graphics;
using Otter.Utility.MonoGame;
using WormGame.Core;
using WormGame.Static;
using WormGame.Pooling;
using Otter.Graphics.Drawables;

namespace WormGame.Entities
{
    /// @author Antti Harju
    /// @version 14.08.2020
    /// <summary>
    /// Worm entity. Worms are modular entities; it consists of one Otter2d entity and several regular objects (modules). This way the worm can grow infinitely.
    /// </summary>
    public class Worm : PoolableEntity
    {
#if DEBUG
        private readonly bool blockifyWorms;
#endif
        public WormModule firstModule;

        private readonly Pooler<WormModule> modules;
        private readonly Collision collision;
        private readonly WormScene scene;
        private readonly Image head;
        private readonly Image tail;
        private readonly Image eraser;
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
            blockifyWorms = config.blockifyWorms;
#endif
            this.scene = scene;
            this.modules = modules;
            collision = config.collision;
            size = config.size;
            eraser = Image.CreateCircle(config.halfSize, config.backgroundColor);
            head = Image.CreateCircle(config.halfSize);
            tail = Image.CreateCircle(config.halfSize);
            eraser.CenterOrigin();
            head.CenterOrigin();
            tail.CenterOrigin();
            ClearSurfaces();
            AddSurface(config.surface);
            AddGraphic(eraser);
            AddGraphic(head);
            AddGraphic(tail);
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
            target = Position;
            LengthCap = 1;
            Length = 1;
            moving = true;

            head.Color = color;
            tail.Color = color;

            firstModule = modules.Enable();
            firstModule.Position = new Vector2(collision.EntityX(x), collision.EntityY(y));
            firstModule.SetTarget(collision.EntityX(x), collision.EntityY(y));

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
        }


        /// <summary>
        /// Update entity position and recursively module graphic positions.
        /// </summary>
        public override void Update()
        {
            if (moving)
            {
                eraser.SetPosition(lastModule.Position);
                firstModule.GraphicFollow();
                head.SetPosition(firstModule.Position);
                tail.SetPosition(lastModule.Position);
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
            //ClearGraphics();
            moving = false;
        }
    }
}
