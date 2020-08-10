using Otter.Core;
using WormGame.Core;

namespace WormGame.Pooling
{
    /// @author Antti Harju
    /// @version 10.08.2020
    /// <summary>
    /// Class for poolable non-entity objects.
    /// </summary>
    public class Poolable : IPoolable
    {
        /// <summary>
        /// Config constructor. Simplifies Pooler generics.
        /// </summary>
        /// <param name="config">Configuration</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Simplifies Pooler generics")]
        public Poolable(Config config) { }


        /// <summary>
        /// Parameterless constructor so inheritors don't need base(config).
        /// </summary>
        public Poolable() { }


        /// <summary>
        /// Entity identifier. Ids are unique per pool.
        /// </summary>
        public int Id { get; set; }


        /// <summary>
        /// Get or set poolable status is in use or not.
        /// </summary>
        public virtual bool Enabled { get; set; }


        /// <summary>
        /// Disables poolable.
        /// </summary>
        public virtual void Disable()
        {
            Enabled = false;
        }


        /// <summary>
        /// Not used, but simplifies Pooler generics.
        /// </summary>
        /// <param name="scene">Scene to add poolable to</param>
        public void Add(Scene scene) { }
    }
}