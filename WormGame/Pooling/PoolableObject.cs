namespace WormGame.Pooling
{
    /// @author Antti Harju
    /// @version 18.07.2020
    /// <summary>
    /// Class for poolable non-entity objects.
    /// </summary>
    public class PoolableObject : IPoolable
    {
        /// <summary>
        /// Entity identifier.
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