using Otter.Core;
using Otter.Graphics;

namespace WormGame.Pooling
{
    /// @author Antti Harju
    /// @version 08.07.2020
    /// <summary>
    /// Class for poolable entities.
    /// </summary>
    public class Poolable : Entity, IPoolable
    {
        /// <summary>
        /// Not related to pooling, but useful.
        /// </summary>
        public virtual Color Color { get { return Graphic.Color ?? null; } set { Graphic.Color = value; } }


        /// <summary>
        /// Sets graphic visibility. Improves code readability: it's way easier to understand that we want to know 
        /// if entity is enabled or not by looking at entity.Enabled rather than entity.Graphic.Visible.
        /// </summary>
        public bool Enabled { get { return Visible; } set { Collidable = value; AutoUpdate = value; AutoRender = value; Visible = value; } }


        /// <summary>
        /// Improves code readability: entity.Disable() is way easier to understand than entity.Enabled = false;
        /// </summary>
        public virtual void Disable()
        {
            Enabled = false;
        }
    }
}