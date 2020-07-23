using Otter.Graphics;
using Otter.Graphics.Drawables;
using Otter.Utility.MonoGame;
using WormGame.Core;
using WormGame.Pooling;

namespace WormGame.GameObject
{
    /// @author Antti Harju
    /// @version 23.07.2020
    /// <summary>
    /// Class for worm modules.
    /// </summary>
    public class WormModule : PoolableObject
    {
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
        public override bool Enabled { get { return enabled; } set { enabled = value; Graphic.Visible = value; } }
        private bool enabled;


        /// <summary>
        /// Constructor. Initializes graphic.
        /// </summary>
        /// <param name="config">Configuration</param>
        public WormModule(Config config)
        {
            Graphic = Image.CreateCircle(config.size / 2);
            Graphic.CenterOrigin();
        }


        /// <summary>
        /// Recursively update every worm module direction.
        /// </summary>
        /// <param name="newDirection"></param>
        public void DirectionFollow(Vector2 newDirection)
        {
            if (Next != null)
                Next.DirectionFollow(Direction);
            Direction = newDirection;
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
        /// <param name="color"></param>
        public void SetColor(Color color)
        {
            Graphic.Color = color;
            if (Next != null)
                Next.SetColor(color);
        }


        /// <summary>
        /// Set module target.
        /// </summary>
        /// <param name="x">Target.X</param>
        /// <param name="y">Target.Y</param>
        public void SetTarget(float x, float y)
        {
            target.X = x;
            target.Y = y;
        }


        /// <summary>
        /// Recursively disable every one of worms modules.
        /// </summary>
        public override void Disable()
        {
            Enabled = false;
            if (Next != null)
                Next.Disable();
            Next = null;
            ResetDirection();
            Graphic.X = 0;
            Graphic.Y = 0;
            target.X = 0;
            target.Y = 0;
        }
    }
}
