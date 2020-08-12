using Otter.Core;

namespace WormGame.Pooling
{
    /// @author Antti Harju
    /// @version 12.08.2020
    /// <summary>
    /// Interface for all poolables.
    /// </summary>
    public interface IPoolable
    {
        /// <summary>
        /// Is object active. Can be used to enable or disable object.
        /// </summary>
        public bool Active { get; set; }


        /// <summary>
        /// Add object to scene.
        /// </summary>
        /// <param name="scene">Scene to add the entity to.</param>
        public abstract void Add(Scene scene);


        /// <summary>
        /// Disable object.
        /// </summary>
        /// <param name="recursive">Disable other related objects</param>
        public void Disable(bool recursive = true);


        /// <summary>
        /// Poolable identifier. Unique within the same pool.
        /// </summary>
        public int Id { get; }
    }
}