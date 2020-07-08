using Otter.Utility.MonoGame;
using Otter.Graphics.Drawables;
using WormGame.Core;
using WormGame.Pooling;

namespace WormGame.GameObject
{
    public class WormEntity : Poolable
    {
        private readonly float step;
        
        private Vector2 direction;

        public Vector2 target;


        /// <summary>
        /// Next WormEntity in Worm.
        /// </summary>
        public WormEntity Next { get; set; }


        /// <summary>
        /// The direction where WormEntity moves.
        /// </summary>
        public Vector2 Direction { get { return direction; } set { direction = value; } }
        
        
        /// <summary>
        /// WormEntity stops when set to true.
        /// </summary>
        public bool Stationary { get; set; }


        /// <summary>
        /// Constructor. Sets graphic and takes step from config.
        /// </summary>
        /// <param name="config"></param>
        public WormEntity(Config config) : base()
        {
            Image image = Image.CreateCircle(config.size / 2);
            AddGraphic(image);
            image.CenterOrigin();

            step = config.wormStep;
        }


        /// <summary>
        /// Recursively updates directions.
        /// </summary>
        /// <param name="newDirection"></param>
        public void DirectionFollow(Vector2 newDirection)
        {
            if (Next != null)
                Next.DirectionFollow(direction);
            direction = newDirection;
        }


        /// <summary>
        /// Recursively updates targets.
        /// </summary>
        /// <param name="newTarget"></param>
        public void TargetFollow(Vector2 newTarget)
        {
            if (Next != null)
                Next.TargetFollow(target);
            target = newTarget;
        }


        /// <summary>
        /// Moves the WormEntity.
        /// </summary>
        public void Step()
        {
            if (!Stationary)
                Position += Direction * step;
        }
    }
}
