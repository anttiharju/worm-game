namespace WormGame.Pooling
{
    /// @author Antti Harju
    /// @version 18.07.2020
    /// <summary>
    /// Interface for all poolables.
    /// </summary>
    public interface IPoolable
    {
        /// <summary>
        /// Set / get wheter or not object is in use.
        /// </summary>
        public bool Enabled { get; set; }


        /// <summary>
        /// Frees poolable back to the pool.
        /// </summary>
        public abstract void Disable();
    }
}