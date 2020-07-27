using Otter.Graphics;
using Otter.Utility.MonoGame;
using WormGame.Core;
using WormGame.Static;
using WormGame.Pooling;

namespace WormGame.GameObject
{
    /// @author Antti Harju
    /// @version 23.07.2020
    /// <summary>
    /// Worm entity. Worms are modular entities; it consists of one Otter2d entity and several regular objects (modules). This way the worm can grow infinitely.
    /// </summary>
    public class Worm : PoolableEntity
    {
        public WormModule firstModule;

        private readonly Collision collision;
        private readonly float step;
        private readonly int size;

        private WormScene scene;
        private Pooler<WormModule> modules;
        private WormModule lastModule;
        private WormModule newModule;
        private bool moving;
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
        public override Color Color { get { return firstModule.Graphic.Color ?? null; } }


        /// <summary>
        /// Get or set worm direction.
        /// </summary>
        public Vector2 Direction { get { return direction; } set { if (Help.ValidateDirection(collision, firstModule.Target, size, value)) direction = value; } }
        private Vector2 direction;


        /// <summary>
        /// Get worm target.
        /// </summary>
        public Vector2 Target { get; private set; }


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="config">Configuration</param>
        public Worm(Config config) : base()
        {
            size = config.size;
            step = config.step;
            collision = config.collision;
        }


        /// <summary>
        /// Spawns the worm.
        /// </summary>
        /// <param name="wormModules">WormBody pool so the worm can grow.</param>
        /// <param name="x">Horizontal field position</param>
        /// <param name="y">Vertical field position</param>
        /// <param name="length">Worm length</param>
        /// <param name="color">Worm color</param>
        /// <returns>Worm</returns>
        public Worm Spawn(Pooler<WormModule> wormModules, int x, int y, int length, Color color)
        {
            scene = (WormScene)Scene;
            modules = wormModules;
            SetPosition(collision.EntityX(x), collision.EntityY(y));
            Target = Position;
            LengthCap = 1;
            Length = 1;
            moving = true;

            firstModule = wormModules.Enable();
            firstModule.Target = Position;
            firstModule.Graphic.Color = color;
            AddGraphic(firstModule.Graphic);

            lastModule = firstModule;
            for (int i = 1; i < length; i++)
                Grow();

            direction = Random.ValidDirection(collision, Position, size);
            collision.Set(this, x, y);
            return this;
        }


        /// <summary>
        /// Grow worm by one module.
        /// </summary>
        private void Grow()
        {
            newModule = modules.Enable();
            if (newModule == null) return;
            newModule.Graphic.SetPosition(lastModule.Graphic.X, lastModule.Graphic.Y);
            newModule.SetTarget(X + lastModule.Graphic.X, Y + lastModule.Graphic.Y);
            newModule.Graphic.Color = Color;
            AddGraphic(newModule.Graphic);
            lastModule.ResetDirection();
            lastModule.Next = newModule;
            lastModule = newModule;
            LengthCap++;
        }


        public void Blockify()
        {
            scene.SpawnBlock(this);
            Disable();
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
            bool retry = false;
        Retry:
            Target = firstModule.Target + Direction * size;
            int nextPosition = collision.Check(Target, true);
            if (nextPosition >= 3) // Move if next position is empty (4) or fruit (3).
            {
                if (Length < LengthCap)
                    Length++;
                else
                    collision.Set(null, lastModule.Target);
                if (nextPosition == 3)
                    grow = true;
                firstModule.DirectionFollow(direction);
                firstModule.TargetFollow(Target);
                collision.Set(this, Target);
            }
            else
            {
                if (retry) // If stuck, turn into a block.
                {
                    Blockify();
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
        /// Update position and modules.
        /// </summary>
        public override void Update()
        {
            if (moving)
            {
                Vector2 positionDelta = firstModule.Direction * step;
                Position += positionDelta;
                firstModule.Next.GraphicFollow(positionDelta, step);
            }
        }


        /// <summary>
        /// Disable worm.
        /// </summary>
        public override void Disable()
        {
            firstModule.Disable();
            ClearGraphics();
            Enabled = false;
            moving = false;
        }
    }
}
