using Otter;
using WormGame.Help;
using WormGame.Other;

namespace WormGame.GameObject
{
    /// @author Antti Harju
    /// @version 01.07.2020
    /// <summary>
    /// Tail class. Acts as the foundation for the entire worm.
    /// </summary>
    class WormTail : Poolable
    {
        public WormTail Next { get; set; }
        public Vector2 target;

        private readonly float speed;

        /// <summary>
        /// Constructor. Creates a circle graphic for the entity.
        /// </summary>
        /// <param name="size">Circle graphic diameter</param>
        public WormTail(int size) : base()
        {
            Image image = Image.CreateCircle(size / 2);
            AddGraphic(image);
            image.CenterOrigin();
            speed = 0.15f * (144 / Config.targetFramerate);
        }


        /// <summary>
        /// Recursive method that makes every part of the tail follow the head
        /// </summary>
        /// <param name="newPosition">Position to move to</param>
        public void TailFollow(Vector2 newPosition)
        {
            if (Next != null)
                Next.TailFollow(target);
            target = newPosition;
        }


        /// <summary>
        /// Recursively moves the entire worm
        /// </summary>
        /// <param name="xDelta">horizontal movement</param>
        /// <param name="yDelta">vertical movement</param>
        public void Move(float xDelta, float yDelta)
        {
            Next.TailFollow(target);
            target.X += xDelta;
            target.Y += yDelta;
        }


        /// <summary>
        /// Tweening for the individual worm parts
        /// </summary>
        public override void Update()
        {
            Position += (target - Position) * speed;
        }
    }
}
