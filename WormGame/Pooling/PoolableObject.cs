using WormGame.Core;

namespace WormGame.Pooling
{
    /// @author Antti Harju
    /// @version 20.07.2020
    /// <summary>
    /// Class for poolable non-entity objects.
    /// </summary>
    public class PoolableObject : IPoolable
    {
#if DEBUG
        /// <summary>
        /// Constructor. Exists for testing purposes.
        /// </summary>
        /// <param name="config">Config</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Parameter required for pooler testing.")]
        public PoolableObject(Config config = null) { }
#endif
        /// <summary>
        /// Entity identifier. Unique within the same pool.
        /// </summary>
        public int Id { get; set; }


        /// <summary>
        /// Get or set wheter the object is in use or not.
        /// </summary>
        public virtual bool Enabled { get; set; }


        /// <summary>
        /// Frees the object back to the pool.
        /// </summary>
        public virtual void Disable()
        {
            Enabled = false;
        }
    }
}