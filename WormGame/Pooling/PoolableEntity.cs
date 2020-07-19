using Otter.Core;
using Otter.Graphics;

namespace WormGame.Pooling
{
    /// @author Antti Harju
    /// @version 18.07.2020
    /// <summary>
    /// Class for poolable entities.
    /// </summary>
    public class PoolableEntity : Entity, IPoolable
    {
        /// <summary>
        /// Disables Otter2D collision.
        /// </summary>
        public PoolableEntity()
        {
            Collidable = false;
        }


        /// <summary>
        /// Entity identifier.
        /// </summary>
        public int Id { get; set; }


        /// <summary>
        /// Set entity color.
        /// </summary>
        public virtual Color Color { get { return Graphic.Color ?? null; } set { Graphic.Color = value; } }


        /// <summary>
        /// Sets graphic visibility and various other Otter2d entity properties.
        /// </summary>
        public bool Enabled { get { return Visible; } set { AutoUpdate = value; AutoRender = value; Visible = value; } }


        /// <summary>
        /// Frees the entity back to the pool.
        /// </summary>
        public virtual void Disable()
        {
            Enabled = false;
        }
    }
}