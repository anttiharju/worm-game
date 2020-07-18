namespace WormGame.Pooling
{
    /// @author Antti Harju
    /// @version 08.07.2020
    /// <summary>
    /// Class for poolable non-entity objects.
    /// </summary>
    public class PoolableObject : IPoolable
    {
        /// <summary>
        /// Tells wheter or not the object is in use.
        /// </summary>
        public virtual bool Enabled { get; set; }


        /// <summary>
        /// Improves code readability: object.Disable() is way easier to understand than object.Enabled = false;
        /// Overrideable because we have non-entity objects that manage multiple entities.
        /// </summary>
        public virtual void Disable()
        {
            Enabled = false;
        }
    }
}