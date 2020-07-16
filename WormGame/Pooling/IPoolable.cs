namespace WormGame.Pooling
{
    /// @author Antti Harju
    /// @version 08.07.2020
    /// <summary>
    /// Interface for poolable objects. Enables us to pool non-entity objects.
    /// </summary>
    public interface IPoolable
    {
        public bool Enabled { get; set; }

        public abstract void Disable();
    }
}