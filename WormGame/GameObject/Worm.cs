using Otter.Graphics;
using Otter.Utility.MonoGame;
using WormGame.Core;
using WormGame.Static;
using WormGame.Pooling;
using Otter.Graphics.Drawables;

namespace WormGame.GameObject
{
    public class Worm : Poolable
    {
        private readonly Collision field;
        private readonly Image[] worm;
        private readonly int size;
        private readonly float step;

        private bool moving;
        private Vector2 target;
        private Vector2 direction;
        private Vector2[] targets;
        private Vector2[] directions;

        public bool Posessed { get; set; }
        public int Length { get; private set; }
        public override Color Color { get { return worm[0].Color ?? null; } set { SetColor(value); } }
        public Vector2 Direction { get { return directions[0]; } set { if (Help.ValidateDirection(field, targets[0], size, value)) direction = value; } }


        public Worm(Config config) : base()
        {
            size = config.size;
            step = config.step;
            field = config.field;
            Length = config.maxWormLength;

            worm = new Image[config.maxWormLength];
            targets = new Vector2[config.maxWormLength];
            directions = new Vector2[config.maxWormLength];

            for (int i = 0; i < worm.Length; i++)
            {
                worm[i] = Image.CreateCircle(config.imageSize / 2);
                worm[i].Scale = config.size * 1.0f / config.imageSize;
                worm[i].Visible = false;
                worm[i].CenterOrigin();
                AddGraphic(worm[i]);
            }
        }

        public Worm Spawn(int x, int y, int length, Color color, int hack)
        {
            X = field.EntityX(x); Y = field.EntityY(y); Length = length; Color = color;
            for (int i = 0; i < Length; i++)
            {
                worm[i].X = 0; worm[i].Y = 0;
                worm[i].Visible = true;
                // Hacky, but makes collision work when the worm is just starting to move.
                targets[i].X = field.EntityX(hack); targets[i].Y = field.EntityY(0);
            }
            targets[0] = Position;

            direction = Random.ValidDirection(field, Position, size);
            field.Set(this, x, y);
            return this;
        }

        public Vector2 GetTarget(int index)
        {
            return targets[index];
        }

        public void SetColor(Color color)
        {
            for (int i = 0; i < Length; i++)
                worm[i].Color = color;
        }

        public override void Disable()
        {
            for (int i = 0; i < Length; i++)
            {
                worm[i].Visible = false;
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
            if (field.Check(target))
            {
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
                        worm[i].X += graphicDelta.X; worm[i].Y += graphicDelta.Y;
                    }
                }
            }
        }
    }
}
