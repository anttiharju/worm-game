using Otter.Graphics;
using Otter.Utility.MonoGame;
using Otter.Graphics.Drawables;
using WormGame.Core;
using WormGame.Static;
using WormGame.Pooling;

namespace WormGame.GameObject
{
    public class Worm : PoolableEntity
    {
        private readonly Collision field;
        private readonly int size;
        private readonly float step;

        private int rampUp;
        private bool grow;
        private bool moving;
        private Vector2 target;
        private Vector2 direction;
        private WormBody firstBody;
        private Pooler<WormBody> wormBodies;

        public Player Player { get; set; }
        public int Length { get; private set; }
        public override Color Color { get { return firstBody.GetGraphic().Color ?? null; } set { SetColor(value); } }
        public Vector2 Direction { get { return firstBody.GetDirection(); } set { if (Help.ValidateDirection(field, firstBody.GetTarget(), size, value)) direction = value; } }


        public Worm(Config config) : base()
        {
            size = config.size;
            step = config.step;
            field = config.field;
        }

        public Worm Spawn(Pooler<WormBody> wormBodies, int x, int y, int length, Color color)
        {
            this.wormBodies = wormBodies;
            X = field.EntityX(x);
            Y = field.EntityY(y);
            Length = length;
            firstBody = wormBodies.Enable();
            firstBody.GetGraphic().X = 0;
            firstBody.GetGraphic().Y = 0;
            firstBody.GetGraphic().Visible = true;
            firstBody.GetTarget() = Position;
            AddGraphic(firstBody.GetGraphic());
            WormBody previous = firstBody;
            for (int i = 1; i < Length; i++)
            {
                WormBody current = wormBodies.Enable();
                current.GetGraphic().X = 0;
                current.GetGraphic().Y = 0;
                current.GetGraphic().Visible = true;
                current.GetTarget() = Position;
                previous.GetNext() = current;
                AddGraphic(firstBody.GetGraphic(i));
            }

            Color = color;
            direction = Random.ValidDirection(field, Position, size);
            field.Set(this, x, y);
            grow = false;
            return this;
        }


        public void Grow()
        {
            grow = true;
        }

        public Vector2 GetTarget(int index)
        {
            return firstBody.GetTarget(index);
        }

        public void SetColor(Color color)
        {
            firstBody.SetColor(color);
        }

        public override void Disable()
        {
            firstBody.Disable();
            Enabled = false;
        }

        /// <summary>
        /// Updates worms directions, targets and adds it to the collision field.
        /// </summary>
        public void Move()
        {
            moving = true;
            bool retry = false;
        Retry:
            target = firstBody.GetTarget() + direction * size;
            int check = field.Check(target, true);
            if (check != 2)
            {
                if (check == 1)
                    Grow();
                if (rampUp < Length - 1)
                    rampUp++;
                else if (!grow)
                    field.Set(null, firstBody.GetTarget(Length - 1));
                firstBody.DirectionFollow(direction);
                firstBody.TargetFollow(target);
                field.Set(this, target);
            }
            else
            {
                if (Player == null && !retry)
                {
                    direction = Random.ValidDirection(field, firstBody.GetTarget(), size);
                    retry = true;
                    goto Retry;
                }
                moving = false;
            }
            if (grow && moving)
            {
                WormBody current = wormBodies.Enable();
                firstBody.GetNext(Length) = current;
                Image newGraphic = current.GetGraphic();
                newGraphic.Visible = true;
                newGraphic.X = firstBody.GetGraphic(Length - 1).X;
                newGraphic.Y = firstBody.GetGraphic(Length - 1).Y;
                firstBody.GetTarget(Length).X = Position.X + newGraphic.X;
                firstBody.GetTarget(Length).Y = Position.Y + newGraphic.Y;
                newGraphic.Color = Color;
                Length++;
                rampUp++;
                grow = false;
            }
        }


        /// <summary>
        /// Moves worms graphics
        /// </summary>
        public override void Update()
        {
            base.Update();
            if (moving)
            {
                for (int i = 0; i < Length; i++)
                {
                    Vector2 positionDelta = firstBody.GetDirection() * step;
                    if (i == 0)
                        Position += positionDelta;
                    else
                    {
                        Vector2 graphicDelta = firstBody.GetDirection(i) * step - positionDelta;
                        firstBody.GetGraphic(i).X += graphicDelta.X; firstBody.GetGraphic(i).Y += graphicDelta.Y;
                    }
                }
            }
        }
    }
}
