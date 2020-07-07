using Otter.Utility.MonoGame;
using Otter.Graphics.Drawables;
using WormGame.Manager;

namespace WormGame.GameObject
{
    /// @author Antti Harju
    /// @version 01.07.2020
    /// <summary>
    /// Tail class. Acts as the foundation for the entire worm.
    /// </summary>
    class WormBase : Poolable
    {
        private readonly Config config;
        private Worm head;

        public WormBase Next { get; set; }
        public Vector2 target;

        /// <summary>
        /// Constructor. Creates a circle graphic for the entity.
        /// </summary>
        /// <param name="size">Circle graphic diameter</param>
        public WormBase(int size, Config config) : base()
        {
            Image image = Image.CreateCircle(size / 2);
            AddGraphic(image);
            image.CenterOrigin();
            this.config = config;
        }

        public void SetHead(Worm head)
        {
            if (Next != null)
                Next.SetHead(head);
            this.head = head;
        }

        /// <summary>
        /// Recursive method that makes every part of the tail follow the head
        /// </summary>
        /// <param name="newTarget">Position to move to</param>
        public void TailFollow(Vector2 newTarget)
        {
            if (Next != null)
            {
                Next.Position = Next.target;
                Next.TailFollow(target);
            }
            target = newTarget;
        }


        /// <summary>
        /// Tweening for the individual worm parts
        /// </summary>
        public override void Update()
        {
            base.Update();
            if (Enabled)
                Position += (target - Position) * config.WormSpeed;
        }
    }
}
