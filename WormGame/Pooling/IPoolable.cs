using Otter.Core;

namespace WormGame.Pooling
{
    /// @author Antti Harju
    /// @version 10.08.2020
    /// <summary>
    /// Interface for all poolables.
    /// </summary>
    public interface IPoolable
    {
        /// <summary>
        /// Entity identifier.
        /// </summary>
        public int Id { get; set; }


        /// <summary>
        /// Enable or get poolable status. To disable entity use Disable -method.
        /// </summary>
        public bool Enabled { get; set; }


        /// <summary>
        /// Disable poolable. Technically Enabled = false would be the same, but we also need to reset poolable variables so that's why separate method.
        /// </summary>
        public abstract void Disable();


        /// <summary>
        /// Only required by PoolableEntity, but Poolable also having this simplifies things.
        /// </summary>
        /// <param name="scene">Scene to add the entity to.</param>
        public abstract void Add(Scene scene);
    }
}