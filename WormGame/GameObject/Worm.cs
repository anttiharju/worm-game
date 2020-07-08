using Otter.Graphics;
using Otter.Utility.MonoGame;
using WormGame.Core;
using WormGame.Static;
using WormGame.Pooling;

namespace WormGame.GameObject
{
    class Worm : BasicPoolable
    {
        private readonly Collision field;
        private readonly WormEntity[] worm;
        private readonly int size;

        private Vector2 nextTarget = new Vector2();

        public Vector2 Position { get { return worm[Length / 2].target; } }
        public Vector2 Direction { get { return worm[0].Direction; } set { worm[0].DirectionFollow(value); } }
        public Color Color { get { return worm[0].Color ?? null; } set { SetColor(value); } }
        public int Length { get; private set; }
        public bool Posessed { get; set; }

        public Worm(Config config)
        {
            size = config.size;
            field = config.field;
            worm = new WormEntity[config.maxWormLength];
        }


        public Worm Spawn(Pool<WormEntity> bodies, Collision field, int x, int y, int length, Color color)
        {
            WormEntity previous = null;
            for (int i = 0; i < length; i++)
            {
                WormEntity current = bodies.Enable();
                worm[i] = current;
                if (current == null) break;
                current.X = x; current.Y = y;
                current.target = current.Position;
                if (previous != null)
                    previous.Next = current;
                previous = current;
            }

            Length = length;
            field.Set(worm[0], worm[0].target);
            worm[0].Direction = Random.Direction();
            Color = color;
            return this;
        }

        public void Move()
        {
            Stuck(false);
            worm[0].Graphic.Color = Color.Red;
            nextTarget = worm[0].target + worm[0].Direction * size;
            if (field.Check(nextTarget))
            {
                field.SetNull(worm[^1].target);
                for (int i = worm.Length - 1; i > 0; i--)
                    field.Set(worm[i], worm[i - 1].target);
                field.Set(worm[0], nextTarget);
                worm[0].TargetFollow(nextTarget);
                worm[0].Next.DirectionFollow(worm[0].Direction);
            }
            else
            {
                if (!Posessed)
                {
                    worm[0].DirectionFollow(Random.ValidDirection(field, worm[0].target, size));
                }
                Stuck();
            }
        }

        private void Stuck(bool stuck = true)
        {
            for (int i = 0; i < Length; i++)
            {
                worm[i].Stuck = stuck;
            }
        }

        public override void Disable()
        {
            for (int i = 0; i < Length; i++)
            {
                field.SetNull(worm[i].target);
                worm[i].Enabled = false;
            }
            Enabled = false;
        }

        public void SetColor(Color color)
        {
            for (int i = 0; i < Length; i++)
                worm[i].Graphic.Color = color;
        }

        public WormEntity this[int i]
        {
            get => worm[i];
            set => worm[i] = value;
        }
    }
}
