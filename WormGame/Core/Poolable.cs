using Otter.Core;
using Otter.Graphics;

namespace WormGame.Core
{
    /// @author Antti Harju
    /// @version 13.07.2020
    /// <summary>
    /// Class for poolable entities.
    /// </summary>
    public class Poolable : Entity
    {
        /// <summary>
        /// Disables Otter2D collision checks.
        /// </summary>
        public Poolable()
        {
            Collidable = false;
        }


        /// <summary>
        /// Set entity color.
        /// </summary>
        public virtual Color Color { get { return Graphic.Color ?? null; } set { Graphic.Color = value; } }


        /// <summary>
        /// Sets graphic visibility and various other Otter2d entity variables.
        /// </summary>
        public bool Enabled { get { return Visible; } set { AutoUpdate = value; AutoRender = value; Visible = value; } }


        /// <summary>
        /// Improves code readability: entity.Disable() is way easier to understand than entity.Enabled = false;
        /// </summary>
        public virtual void Disable()
        {
            Enabled = false;
        }
    }
}