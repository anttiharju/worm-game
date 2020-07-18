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
    public class Worm : PoolableEntity
    {
        private readonly Collision field;
        private readonly float step;
        private readonly int size;

        private int rampUp;
        private bool moving;
        private Vector2 target;
        private Vector2 direction;
        private WormBody newBody;
        private WormBody lastBody;
        private WormBody firstBody;
        private Graphic newGraphic;
        private Pooler<WormBody> wormBodies;


        /// <summary>
        /// Not used by worm, but required by brick later on.
        /// </summary>
        public Player Player { get; set; }


        /// <summary>
        /// Get worm length.
        /// </summary>
        public int Length { get; private set; }


        /// <summary>
        /// Get and set worm color.
        /// </summary>
        public override Color Color { get { return firstBody.Graphic.Color ?? null; } set { SetColor(value); } }


        /// <summary>
        /// Get and set the worm direction.
        /// </summary>
        public Vector2 Direction { get { return firstBody.Direction; } set { if (Help.ValidateDirection(field, firstBody.Direction, size, value)) direction = value; } }


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
        /// <param name="wormBodies">WormBody pool so the worm can grow.</param>
        /// <param name="x">Horizontal field position</param>
        /// <param name="y">Vertical field position</param>
        /// <param name="length">Worm length</param>
        /// <param name="color">Worm color</param>
        /// <returns>Worm</returns>
        public Worm Spawn(Pooler<WormBody> wormBodies, int x, int y, int length, Color color)
        {
            this.wormBodies = wormBodies;
            X = field.EntityX(x);
            Y = field.EntityY(y);
            Length = length;

            lastBody = null;
            firstBody = wormBodies.Enable();
            newBody = firstBody;
            for (int i = 0; i < Length; i++)
            {
                newBody.Graphic.X = 0;
                newBody.Graphic.Y = 0;
                newBody.Graphic.Visible = true;
                newBody.Target = Position;

                if (lastBody != null)
                    lastBody.Next = newBody;
                AddGraphic(newBody.Graphic);
                lastBody = newBody;
                if (i != Length - 1)
                    newBody = wormBodies.Enable();
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
            bool retry = false;
        Retry:
            target = firstBody.Target + direction * size;
            int check = field.Check(target, true);
            if (check != 2)
            {
                if (check == 1)
                    grow = true;
                if (rampUp < Length - 1)
                    rampUp++;
                else if (!grow)
                    field.Set(null, lastBody.Target);
                firstBody.DirectionFollow(direction);
                firstBody.TargetFollow(target);
                field.Set(this, target);
            }
            else
            {
                if (Player == null && !retry)
                {
                    direction = Random.ValidDirection(field, firstBody.Target, size);
                    retry = true;
                    goto Retry;
                }
                moving = false;
            }
            if (grow && moving)
            {
                newBody = wormBodies.Enable();
                if (newBody == null) return;
                newGraphic = newBody.Graphic;
                newGraphic.X = lastBody.Graphic.X;
                newGraphic.Y = lastBody.Graphic.Y;
                newBody.GetTarget().X = Position.X + newGraphic.X;
                newBody.GetTarget().Y = Position.Y + newGraphic.Y;
                newGraphic.Color = Color;
                Length++;
                rampUp++;
                AddGraphic(newGraphic);
                lastBody.Next = newBody;
                lastBody = newBody;
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
                Vector2 positionDelta = firstBody.Direction * step;
                Position += positionDelta;
                firstBody.Next.GraphicFollow(positionDelta, step);
            }
        }


        /// <summary>
        /// Gets worms nth WormBodies target position. Kind of like an indexer, but not.
        /// </summary>
        /// <param name="n">WormBody index</param>
        /// <returns>Worms nth WormBodies target position</returns>
        public Vector2 GetTarget(int n)
        {
            return firstBody.GetTarget(n);
        }



        /// <summary>
        /// Sets worms color.
        /// </summary>
        /// <param name="color"></param>
        public void SetColor(Color color)
        {
            firstBody.SetColor(color);
        }


        /// <summary>
        /// Disables the worm.
        /// </summary>
        public override void Disable()
        {
            firstBody.Disable();
            Enabled = false;
        }
    }
}
