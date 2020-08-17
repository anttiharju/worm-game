using Otter.Graphics;
using Otter.Graphics.Drawables;
using Otter.Utility.MonoGame;
using WormGame.Core;
using WormGame.Pooling;

namespace WormGame.Entities
{
    /// @author Antti Harju
    /// @version 14.08.2020
    /// <summary>
    /// WormModule. Thanks to modularity worm length can be increased during runtime.
    /// </summary>
    public class WormModule : Poolable
    {
        private readonly Collision collision;
        private readonly float step;


        /// <summary>
        /// Get or set next module.
        /// </summary>
        public WormModule Next { get; set; }


        /// <summary>
        /// Get module graphic.
        /// </summary>
        public Vector2 Position { get { return position; } set { position = value; } }
        private Vector2 position;


        /// <summary>
        /// Get or set worm direction.
        /// </summary>
        public Vector2 Direction { get { return direction; } set { direction = value; } }
        private Vector2 direction;


        /// <summary>
        /// Get or set target position.
        /// </summary>
        public Vector2 Target { get { return target; } set { target = value; } }
        private Vector2 target;


        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="config">Configuration</param>
        public WormModule(Config config)
        {
            collision = config.collision;
            step = config.step;
        }


        /// <summary>
        /// Recursively update every worm module direction.
        /// </summary>
        /// <param name="previousDirection">Previous direction</param>
        public void DirectionFollow(Vector2 previousDirection)
        {
            if (Next != null)
                Next.DirectionFollow(Direction);
            Direction = previousDirection;
        }


        /// <summary>
        /// Recursively update every worm module graphic position.
        /// </summary>
        /// <param name="positionDelta">Worm entity position delta</param>
        /// <param name="step">Worm step</param>
        public void GraphicFollow()
        {
            position += Direction * step;
            if (Next != null)
                Next.GraphicFollow();
        }


        /// <summary>
        /// Recursively update every worm module target.
        /// </summary>
        /// <param name="newTarget">New target for worm body</param>
        public void TargetFollow(Vector2 newTarget)
        {
            if (Next != null)
                Next.TargetFollow(Target);
            Target = newTarget;
        }


        /// <summary>
        /// Reset module direction.
        /// </summary>
        public void ResetDirection()
        {
            direction.X = 0;
            direction.Y = 0;
        }

        /// <summary>
        /// Set module target.
        /// </summary>
        /// <param name="target">Target</param>
        public void SetTarget(Vector2 target)
        {
            SetTarget(target.X, target.Y);
        }

        public void SetTarget(float x, float y)
        {
            target.X = x;
            target.Y = y;
        }

        /// <summary>
        /// Recursively disable every one of worms modules.
        /// </summary>
        /// <param name="recursive">Disable recursively. False only when disabling is done by pooler.</param>
        public override void Disable(bool recursive = true)
        {
            base.Disable();
            if (recursive && Next != null)
                Next.Disable();
            Next = null;
            if (collision.Check(target) == collision.worm)
                collision.Add(null, target);
            ResetDirection();
            position.X = 0;
            position.Y = 0;
            target.X = 0;
            target.Y = 0;
        }
    }
}
