using Otter.Graphics;
using Otter.Graphics.Drawables;
using Otter.Utility.MonoGame;
using WormGame.Core;
using WormGame.Pooling;

namespace WormGame.GameObject
{
    /// @author Antti Harju
    /// @version 18.07.2020
    /// <summary>
    /// Class for worm bodies. Worm is still a single entity, but a modular one thanks to this class.
    /// </summary>
    public class WormModule : PoolableObject
    {
        private readonly Collision field;

        /// <summary>
        /// Get or set next WormBody in worm.
        /// </summary>
        public WormModule Next { get; set; }


        /// <summary>
        /// Get WormBody graphic.
        /// </summary>
        public Image Graphic { get; private set; }


        /// <summary>
        /// Get or set target position.
        /// Implemented this way because we need to be able to return it by reference to avoid creating a new Vector2 every wormUpdate per worm.
        /// </summary>
        public Vector2 Target { get { return target; } set { target = value; } }
        private Vector2 target;


        /// <summary>
        /// Get or set worm direction.
        /// </summary>
        public Vector2 Direction { get { return direction; } set { direction = value; } }
        private Vector2 direction;


        /// <summary>
        /// Set or get wheter or not object is in use.
        /// </summary>
        public override bool Enabled { get { return enabled; } set { enabled = value; Graphic.Visible = value; } }
        private bool enabled;


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="config">Config</param>
        public WormModule(Config config)
        {
            field = config.field;
            Graphic = Image.CreateCircle(config.imageSize / 2);
            Graphic.Scale = (float)config.size / config.imageSize;
            Graphic.CenterOrigin();
        }


        /// <summary>
        /// So we can get specific WormBody target positions. Try to use recursive methods instead of this in loops.
        /// </summary>
        /// <param name="n">Target index</param>
        /// <param name="i">Index to keep track of where we are</param>
        /// <returns>nth WormBody target position</returns>
        public Vector2 GetTarget(int n, int i = 0)
        {
            if (n == i)
                return Target;
            return Next.GetTarget(n, ++i);
        }


        /// <summary>
        /// Returns target. With this we can directly set target.X instead of creating a new Vector every time we want to modify it.
        /// </summary>
        /// <returns>Target (reference)</returns>
        public ref Vector2 GetTarget()
        {
            return ref target;
        }


        /// <summary>
        /// Updates WormBodies graphic positions
        /// </summary>
        /// <param name="positionDelta">Worm entitys position delta</param>
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
        /// Updates worms target positions recursively.
        /// </summary>
        /// <param name="newTarget">New target for worm body</param>
        public void TargetFollow(Vector2 newTarget)
        {
            if (Next != null)
                Next.TargetFollow(Target);
            Target = newTarget;
        }


        /// <summary>
        /// Updates worms directions recursively.
        /// </summary>
        /// <param name="newDirection"></param>
        public void DirectionFollow(Vector2 newDirection)
        {
            if (Next != null)
                Next.DirectionFollow(Direction);
            Direction = newDirection;
        }


        /// <summary>
        /// Recursively sets the whole worms color.
        /// </summary>
        /// <param name="color"></param>
        public void SetColor(Color color)
        {
            Graphic.Color = color;
            if (Next != null)
                Next.SetColor(color);
        }


        /// <summary>
        /// Disables all of worms WormBodies.
        /// </summary>
        public override void Disable()
        {
            Enabled = false;
            if (Next != null)
                Next.Disable();
            if (field.Check(target) == 1)
                field.Set(null, target);
            Next = null;
            target.X = 0;
            target.Y = 0;
            direction.X = 0;
            direction.Y = 0;
        }
    }
}
