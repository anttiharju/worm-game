using Otter.Core;

namespace WormGame.Pooling
{
    /// @author Antti Harju
    /// @version v0.5
    /// <summary>
    /// Base class for poolable entities.
    /// </summary>
    public class PoolableEntity : Entity, IPoolable
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public PoolableEntity()
        {
            Collidable = false;
        }


        /// <summary>
        /// Add entity to the scene.
        /// </summary>
        /// <param name="scene">Scene</param>
        public void Add(Scene scene)
        {
            scene.Add(this);
        }


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
    }
}