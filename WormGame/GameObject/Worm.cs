using Otter.Graphics;
using Otter.Utility.MonoGame;
using Otter.Graphics.Drawables;
using WormGame.Core;
using WormGame.Static;
using WormGame.Pooling;

namespace WormGame.GameObject
{
    /// @author Antti Harju
    /// @version 18.07.2020
    /// <summary>
    /// Worm entity. Worms are modular entities; it consists of one Otter2d entity and several normal objects so it can grow and be any length.
    /// </summary>
    /// TODO: Weird behaviour if turn into a brick in a tight space.
    public class Worm : PoolableEntity
    {
        private readonly Collision field;
        private readonly float step;
        private readonly int size;

        private int rampUp;
        private bool moving;
        private WormScene scene;
        private Vector2 target;
        private WormModule newModule;
        private WormModule lastModule;
        private Graphic newGraphic;
        private Pooler<WormModule> modules;

        public WormModule firstModule;


        /// <summary>
        /// We can access scene through player.
        /// </summary>
        public Player Player { get; set; }


        /// <summary>
        /// Get worm length.
        /// </summary>
        public int Length { get; private set; }


        /// <summary>
        /// Get and set worm color.
        /// </summary>
        public override Color Color { get { return firstModule.Graphic.Color ?? null; } set { SetColor(value); } }


        /// <summary>
        /// Get and set the worm direction.
        /// </summary>
        public Vector2 Direction { get { return direction; } set { if (Help.ValidateDirection(field, firstModule.Target, size, value)) direction = value; } }
        private Vector2 direction;


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="config"></param>
        public Worm(Config config) : base()
        {
            size = config.size;
            step = config.step;
            field = config.field;
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
            X = field.EntityX(x);
            Y = field.EntityY(y);
            target = Position;
            Length = length;

            lastModule = null;
            firstModule = wormModules.Enable();
            newModule = firstModule;
            for (int i = 0; i < Length; i++)
            {
                newGraphic = newModule.Graphic;
                newModule.Target = Position;

                if (lastModule != null)
                    lastModule.Next = newModule;
                AddGraphic(newGraphic);
                lastModule = newModule;
                if (i != Length - 1)
                    newModule = wormModules.Enable();
            }

            direction = Random.ValidDirection(field, Position, size);
            field.Set(this, x, y);
            Color = color;
            return this;
        }


        /// <summary>
        /// Updates worms directions, targets and updates its position on the collision field.
        /// </summary>
        public void Move()
        {
            moving = true;
            bool grow = false;
            bool tryAgain = true;
        Retry:
            target = firstModule.Target + Direction * size;
            int check = field.Check(target, true);
            if (check >= 3)
            {
                if (check == 3)
                    grow = true;
                if (rampUp < Length - 1)
                    rampUp++;
                else if (!grow)
                    field.Set(null, lastModule.Target);
                firstModule.DirectionFollow(direction);
                firstModule.TargetFollow(target);
                field.Set(this, target);
            }
            else
            {
                if (!tryAgain)
                    scene.SpawnBrick(this);
                else if (Player == null)
                {
                    direction = Random.ValidDirection(field, firstModule.Target, size);
                    tryAgain = false;
                    goto Retry;
                }
                moving = false;
            }
            if (grow && moving)
            {
                newModule = modules.Enable();
                if (newModule == null) return;
                newGraphic = newModule.Graphic;
                newGraphic.X = lastModule.Graphic.X;
                newGraphic.Y = lastModule.Graphic.Y;
                newModule.GetTarget().X = Position.X + newGraphic.X;
                newModule.GetTarget().Y = Position.Y + newGraphic.Y;
                newGraphic.Color = Color;
                Length++;
                rampUp++;
                AddGraphic(newGraphic);
                lastModule.Next = newModule;
                lastModule = newModule;
            }
        }


        /// <summary>
        /// Moves worms graphics.
        /// </summary>
        public override void Update()
        {
            base.Update();
            if (moving)
            {
                Vector2 positionDelta = firstModule.Direction * step;
                Position += positionDelta;
                firstModule.Next.GraphicFollow(positionDelta, step);
            }
        }


        /// <summary>
        /// Gets worms nth WormBodies target position. Kind of like an indexer, but not.
        /// </summary>
        /// <param name="n">WormBody index</param>
        /// <returns>Worms nth WormBodies target position</returns>
        public Vector2 GetTarget(int n)
        {
            return firstModule.GetTarget(n);
        }


        /// <summary>
        /// Sets worms color.
        /// </summary>
        /// <param name="color"></param>
        public void SetColor(Color color)
        {
            firstModule.SetColor(color);
        }


        /// <summary>
        /// Disables the worm.
        /// </summary>
        public override void Disable()
        {
            newGraphic = null;
            lastModule = null;
            newModule = null;
            modules = null;
            Enabled = false;
            moving = false;
            target.X = 0;
            target.Y = 0;
            rampUp = 0;
            firstModule.Disable();
            firstModule = null;
        }
    }
}
