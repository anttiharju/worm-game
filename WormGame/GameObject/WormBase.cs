using Otter.Graphics.Drawables;
using Otter.Utility.MonoGame;
using WormGame.Help;
using WormGame.Other;

namespace WormGame.GameObject
{
    /// @author Antti Harju
    /// @version 01.07.2020
    /// <summary>
    /// Tail class. Acts as the foundation for the entire worm.
    /// </summary>
    class WormBase : Poolable
    {
        public WormBase Next { get; set; }
        public Vector2 target;

        private readonly float speed;

        /// <summary>
        /// Constructor. Creates a circle graphic for the entity.
        /// </summary>
        /// <param name="size">Circle graphic diameter</param>
        public WormBase(int size) : base()
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
        /// <param name="deltaX">horizontal movement</param>
        /// <param name="deltaY">vertical movement</param>
        public void Move(float deltaX, float deltaY)
        {
            Next.TailFollow(target);
            target.X += deltaX;
            target.Y += deltaY;
        }


        /// <summary>
        /// Tweening for the individual worm parts
        /// </summary>
        public override void Update()
        {
            base.Update();
            if (Enabled)
                Position += (target - Position) * speed;
        }
    }
}
