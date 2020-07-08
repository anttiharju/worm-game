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

        private Vector2 tmp = new Vector2();

        public Vector2 Position { get { return worm[Length / 2].target; } }
        public Vector2 Direction { get { return worm[0].target; } set { worm[0].Direction = value; } }
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
            field.Update(worm[0].target, worm[0]);
            worm[0].Direction = Random.Direction();
            Color = color;
            return this;
        }

        public void Move()
        {
            bool retry = false;
        Retry:
            tmp = worm[0].target + worm[0].Direction * size;
            if (field.Check(tmp))
            {
                field.Update(worm[^1].target, null);
                for (int i = worm.Length - 1; i > 0; i--)
                    field.Update(worm[i - 1].target, worm[i]);
                field.Update(tmp, worm[0]);
                worm[0].TargetFollow(tmp);
                worm[0].Next.DirectionFollow(worm[0].Direction);
            }
            else if (!Posessed && !retry)
            {
                worm[0].Direction = Random.ValidDirection(field, worm[0].target, size);
                retry = true;
                goto Retry;
            }
        }

        public override void Disable()
        {
            for (int i = 0; i < Length; i++)
            {
                field.Update(worm[i].target, null);
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
