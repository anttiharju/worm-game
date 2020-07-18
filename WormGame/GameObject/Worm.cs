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
        private bool moving;
        private Vector2 target;
        private Vector2 direction;
        private WormBody firstBody;
        private WormBody lastBody;
        private Pooler<WormBody> wormBodies;

        public Player Player { get; set; }
        public int Length { get; private set; }
        public override Color Color { get { return firstBody.Graphic.Color ?? null; } set { SetColor(value); } }
        public Vector2 Direction { get { return firstBody.Direction; } set { if (Help.ValidateDirection(field, firstBody.Direction, size, value)) direction = value; } }


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

            lastBody = null;
            firstBody = wormBodies.Enable();
            WormBody current = firstBody;
            for (int i = 0; i < Length; i++)
            {
                current.Graphic.X = 0;
                current.Graphic.Y = 0;
                current.Graphic.Visible = true;
                current.Target = Position;

                if (lastBody != null)
                    lastBody.Next = current;
                AddGraphic(current.Graphic);
                lastBody = current;
                if (i != Length - 1)
                    current = wormBodies.Enable();
            }

            direction = Random.ValidDirection(field, Position, size);
            field.Set(this, x, y);
            Color = color;
            return this;
        }

        public Vector2 GetTarget(int wantedIndex)
        {
            return firstBody.GetTarget(wantedIndex);
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
                WormBody newBody = wormBodies.Enable();
                if (newBody == null) return;
                Image newGraphic = newBody.Graphic;
                newGraphic.Visible = true;
                newGraphic.X = lastBody.Graphic.X;
                newGraphic.Y = lastBody.Graphic.Y;
                newBody.Target = new Vector2(Position.X + newGraphic.X, Position.Y + newGraphic.Y);
                newGraphic.Color = Color;
                Length++;
                rampUp++;
                AddGraphic(newGraphic);
                lastBody.Next = newBody;
                lastBody = newBody;
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
                Vector2 positionDelta = firstBody.Direction * step;
                Position += positionDelta;
                firstBody.Next.Follow(positionDelta, step);
            }
        }
    }
}
