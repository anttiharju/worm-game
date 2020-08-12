using Otter.Core;
using Otter.Graphics;

namespace WormGame.Pooling
{
    /// @author Antti Harju
    /// @version 12.08.2020
    /// <summary>
    /// Base class for poolable entities.
    /// </summary>
    public class PoolableEntity : Entity, IPoolable
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public PoolableEntity(int id)
        {
            Collidable = false;
            Id = id;
        }


        /// <summary>
        /// Add entity to the scene.
        /// </summary>
        /// <param name="scene">Scene</param>
        public virtual void Add(Scene scene)
        {
            scene.Add(this);
        }


        /// <summary>
        /// Entity color.
        /// </summary>
        public virtual Color Color { get { return Graphic.Color ?? null; } set { Graphic.Color = value; } }


        /// <summary>
        /// Disable entity.
        /// </summary>
        /// <param name="recursive">Disable recursively. False only when disabling is done by pooler.</param>
        public virtual void Disable(bool recursive = true)
        {
            AutoRender = false;
            AutoUpdate = false;
            Visible = false;
        }


        /// <summary>
        /// Is entity active.
        /// </summary>
        public bool Active { get { return Visible; } set { if (value) { AutoRender = true; AutoUpdate = true; Visible = true; } else Disable(); } }


        /// <summary>
        /// Identifier. Unique within the same pool.
        /// </summary>
        public int Id { get; }
    }
}