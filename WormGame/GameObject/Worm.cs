using Otter.Graphics;
using Otter.Utility.MonoGame;
using Otter.Graphics.Drawables;
using WormGame.Core;
using WormGame.Static;

namespace WormGame.GameObject
{
    public class Worm : Poolable
    {
        private readonly Collision field;
        private readonly Image[] graphics;
        private readonly int size;
        private readonly float step;

        private int rampUp;
        private bool grow;
        private bool moving;
        private Vector2 target;
        private Vector2 direction;
        private Vector2[] targets;
        private Vector2[] directions;

        public bool Posessed { get; set; }
        public int Length { get; private set; }
        public override Color Color { get { return graphics[0].Color ?? null; } set { SetColor(value); } }
        public Vector2 Direction { get { return directions[0]; } set { if (Help.ValidateDirection(field, targets[0], size, value)) direction = value; } }


        public Worm(Config config) : base()
        {
            size = config.size;
            step = config.step;
            field = config.field;

            int maxLength = config.maxWormLength;
            graphics = new Image[maxLength];
            targets = new Vector2[maxLength];
            directions = new Vector2[maxLength];

            for (int i = 0; i < maxLength; i++)
            {
                Image tmp = Image.CreateCircle(config.imageSize / 2);
                tmp.Scale = (float)config.size / config.imageSize;
                tmp.Visible = false;
                tmp.CenterOrigin();
                graphics[i] = tmp;
                AddGraphic(tmp);
            }
        }

        public Worm Spawn(int x, int y, int length, Color color)
        {
            X = field.EntityX(x);
            Y = field.EntityY(y);
            Length = length;
            Color = color;
            for (int i = 0; i < Length; i++)
            {
                graphics[i].X = 0; graphics[i].Y = 0;
                graphics[i].Visible = true;
                targets[i] = Position;
            }

            direction = Random.ValidDirection(field, Position, size);
            field.Set(this, x, y);
            grow = false;
            return this;
        }


        /// <summary>
        /// 
        /// </summary>
        /// TODO: Fix a minor bug where another worm can temporarily go inside a newly grown part.
        public void Grow()
        {
            if (Length == graphics.Length) return;
            grow = true;
        }

        public Vector2 GetTarget(int index)
        {
            return targets[index];
        }

        public void SetColor(Color color)
        {
            for (int i = 0; i < Length; i++)
                graphics[i].Color = color;
        }

        public override void Disable()
        {
            for (int i = 0; i < Length; i++)
            {
                graphics[i].Visible = false;
                directions[i].X = 0; directions[i].Y = 0;
            }
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
            target = targets[0] + direction * size;
            int check = field.Check(target);
            if (check != 2)
            {
                if (check == 1)
                    Grow();
                if (rampUp < Length - 1)
                    rampUp++;
                else if (!grow)
                    field.Set(null, targets[Length - 1]);
                Follow(ref directions, direction);
                Follow(ref targets, target);
                field.Set(this, target);
            }
            else
            {
                if (!Posessed && !retry)
                {
                    direction = Random.ValidDirection(field, targets[0], size);
                    retry = true;
                    goto Retry;
                }
                moving = false;
            }
            if (grow && moving)
            {
                Image newGraphic = graphics[Length];
                newGraphic.Visible = true;
                newGraphic.X = graphics[Length - 1].X;
                newGraphic.Y = graphics[Length - 1].Y;
                targets[Length].X = Position.X + newGraphic.X;
                targets[Length].Y = Position.Y + newGraphic.Y;
                newGraphic.Color = Color;
                Length++;
                rampUp++;
                grow = false;
            }
        }


        public void Follow(ref Vector2[] array, Vector2 newVector)
        {
            for (int i = Length - 1; i > 0; i--)
                array[i] = array[i - 1];
            array[0] = newVector;
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
                    Vector2 positionDelta = directions[0] * step;
                    if (i == 0)
                        Position += positionDelta;
                    else
                    {
                        Vector2 graphicDelta = directions[i] * step - positionDelta;
                        graphics[i].X += graphicDelta.X; graphics[i].Y += graphicDelta.Y;
                    }
                }
            }
        }
    }
}
