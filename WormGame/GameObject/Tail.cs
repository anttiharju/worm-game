using Otter;
using WormGame.Other;

namespace WormGame.GameObject
{
    /// @author Antti Harju
    /// @version 21.06.2020
    /// <summary>
    /// The base worm class that by itself acts as the tail entity.
    /// </summary>
    class Tail : Poolable
    {
        public Tail NextBody { get; set; }
        public Vector2 Target { get; set; }

        /// <summary>
        /// Constructor. Creates a circle graphic for the entity.
        /// </summary>
        /// <param name="size">Circle graphic diameter</param>
        public Tail(int size) : base()
        {
            Image image = Image.CreateCircle(size / 2);
            AddGraphic(image);
            image.CenterOrigin();
        }

        public void Disable(Collision collision)
        {
            if (NextBody != null)
                NextBody.Disable();
            Enabled = false;
            collision.SetNull(Target);
        }

        public Vector2[] GetPositions(Vector2[] positions, int i = 0)
        {
            if (NextBody != null)
                NextBody.GetPositions(positions, i + 1);
            positions[i] = Target;
            return positions;
        }

        public Tail[] GetWorm(ref Tail[] worm, int i = 0)
        {
            if (NextBody != null)
                NextBody.GetWorm(ref worm, i + 1);
            worm[i] = this;
            return worm;
        }

        /// <summary>
        /// Recursive method that makes every part of the tail follow the head
        /// </summary>
        /// <param name="newPosition">Position to move to</param>
        public void TailFollow(Vector2 newPosition)
        {
            if (NextBody != null)
                NextBody.TailFollow(Target);
            Target = newPosition;
        }


        /// <summary>
        /// Recursively moves the whole worm
        /// </summary>
        /// <param name="xDelta">horizontal movement</param>
        /// <param name="yDelta">vertical movement</param>
        public void Move(float xDelta, float yDelta)
        {
            NextBody.TailFollow(Target);
            Target += new Vector2(xDelta, yDelta);
        }


        /// <summary>
        /// Tweening for the individual worm parts
        /// </summary>
        public override void Update()
        {
            if (Enabled)
                Position += (Target - Position) * 0.15f * (144 / Scene.Game.TargetFramerate);
        }


        /// <summary>
        /// Recursively changes the whole worms color
        /// </summary>
        /// <param name="color">Worms new color</param>
        public void SetColor(Color color)
        {
            if (NextBody != null)
                NextBody.SetColor(color);
            Graphic.Color = color;
        }
    }
}
