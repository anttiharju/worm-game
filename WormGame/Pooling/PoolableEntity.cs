using Otter.Core;
using Otter.Graphics;

namespace WormGame.Pooling
{
    /// @author Antti Harju
    /// @version 10.08.2020
    /// <summary>
    /// Class for poolable entities.
    /// </summary>
    public class PoolableEntity : Otter.Core.Entity, IPoolable
    {
        /// <summary>
        /// Disables Otter2D collision.
        /// </summary>
        public PoolableEntity()
        {
            Collidable = false;
        }


        /// <summary>
        /// Entity identifier. Unique within the same pool.
        /// </summary>
        public int Id { get; set; }


        /// <summary>
        /// Set entity color.
        /// </summary>
        public virtual Color Color { get { return Graphic.Color ?? null; } set { Graphic.Color = value; } }


        /// <summary>
        /// Sets entity visibility and various other Otter2d entity properties.
        /// </summary>
        public virtual bool Enabled { get { return Visible; } set { AutoUpdate = value; AutoRender = value; Visible = value; } }


        /// <summary>
        /// Add entity to the scene.
        /// </summary>
        /// <param name="scene"></param>
        public virtual void Add(Scene scene)
        {
            scene.Add(this);
        }


        /// <summary>
        /// Disable entity.
        /// </summary>
        public virtual void Disable()
        {
            Enabled = false;
        }
    }
}