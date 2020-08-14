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


        /// <summary>
        /// Get or set next module.
        /// </summary>
        public WormModule Next { get; set; }


        /// <summary>
        /// Get module graphic.
        /// </summary>
        public Image Graphic { get; private set; }


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
        /// Get or set wheter or not module is in use.
        /// </summary>
        public override bool Active { get { return active; } set { if (value) { active = true; Graphic.Visible = true; } else Disable(); } }
        private bool active;


        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="config">Configuration</param>
        public WormModule(Config config)
        {
            collision = config.collision;
            Graphic = Image.CreateCircle(config.size / 2);
            Graphic.CenterOrigin();
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
        public void GraphicFollow(Vector2 positionDelta, float step)
        {
            Vector2 delta = Direction * step - positionDelta;
            Graphic.X += delta.X;
            Graphic.Y += delta.Y;
            if (Next != null)
                Next.GraphicFollow(positionDelta, step);
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
        /// Recursively set worm color.
        /// </summary>
        /// <param name="color">Color</param>
        public void SetColor(Color color)
        {
            Graphic.Color = color;
            if (Next != null)
                Next.SetColor(color);
        }


        /// <summary>
        /// Set module target.
        /// </summary>
        /// <param name="target">Target</param>
        public void SetTarget(Vector2 target)
        {
            this.target = target;
        }


        /// <summary>
        /// Recursively disable every one of worms modules.
        /// </summary>
        /// <param name="recursive">Disable recursively. False only when disabling is done by pooler.</param>
        public override void Disable(bool recursive = true)
        {
            base.Disable();
            active = false;
            if (recursive && Next != null)
                Next.Disable();
            Next = null;
            if (collision.Check(target) == collision.worm)
                collision.Add(null, target);
            ResetDirection();
            Graphic.X = 0;
            Graphic.Y = 0;
            target.X = 0;
            target.Y = 0;
        }
    }
}
