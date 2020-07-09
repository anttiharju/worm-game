using Otter.Graphics;
using Otter.Utility.MonoGame;
using WormGame.Core;
using WormGame.Static;
using WormGame.Pooling;
using Otter.Graphics.Drawables;
using System.Dynamic;

namespace WormGame.GameObject
{
    public class Worm : Poolable
    {
        private readonly Collision field;
        private readonly Image[] worm;
        private readonly int size;
        private readonly float step;

        private bool moving;
        private Vector2 nextTarget;
        private Vector2 nextDirection;
        private Vector2[] target;
        private Vector2[] direction;

        public bool Posessed { get; set; }
        public int Length { get; private set; }
        public override Color Color { get { return worm[0].Color ?? null; } set { SetColor(value); } }
        public Vector2 Direction { get { return direction[0]; } set { if (Help.ValidateDirection(field, target[0], size, value)) nextDirection = value; } }


        public Worm(Config config) : base()
        {
            size = config.size;
            step = config.step;
            field = config.field;
            Length = config.maxWormLength;

            worm = new Image[config.maxWormLength];
            target = new Vector2[config.maxWormLength];
            direction = new Vector2[config.maxWormLength];

            for (int i = 0; i < worm.Length; i++)
            {
                worm[i] = Image.CreateCircle(size / 2);
                worm[i].Visible = false;
                worm[i].CenterOrigin();
                AddGraphic(worm[i]);
            }
        }

        public Worm Spawn(int x, int y, int length, Color color)
        {
            X = field.EntityX(x); Y = field.EntityY(y); Length = length; Color = color;
            for (int i = 0; i < Length; i++)
            {
                worm[i].X = 0; worm[i].Y = 0;
                worm[i].Visible = true;
                target[i] = Position;
            }

            nextDirection = Random.ValidDirection(field, Position, size);
            field.Set(this, x, y);
            return this;
        }

        public Vector2 GetTarget(int index)
        {
            return target[index];
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
                direction[i].X = 0; direction[i].Y = 0;
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
            nextTarget = target[0] + nextDirection * size;
            if (field.Check(nextTarget))
            {
                field.Set(target[Length - 1]);
                Follow(ref direction, nextDirection);
                Follow(ref target, nextTarget);
                field.Set(this, nextTarget);
            }
            else
            {
                if (!Posessed && !retry)
                {
                    nextDirection = Random.ValidDirection(field, target[0], size);
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
                    worm[i].X += direction[i].X * step;
                    worm[i].Y += direction[i].Y * step;
                }
            }
        }
    }
}
