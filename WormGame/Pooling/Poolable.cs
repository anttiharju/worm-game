using Otter.Core;
using WormGame.Core;

namespace WormGame.Pooling
{
    /// @author Antti Harju
    /// @version v0.5
    /// <summary>
    /// Base class for poolable non-entity objects.
    /// </summary>
    public class Poolable : IPoolable
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public Poolable() { }


        /// <summary>
        /// Test constructor.
        /// </summary>
        /// <param name="config">Configuration</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Simplifies pooler generics for testing.")]
        public Poolable(Settings config) { }


        /// <summary>
        /// Only used by PoolableEntity, but by also existing here it simplifies Pooler generics.
        /// </summary>
        /// <param name="scene">Scene to add poolable to</param>
        public void Add(Scene scene) { }


        /// <summary>
        /// Disables object.
        /// </summary>
        /// <param name="recursive">Disable recursively. False only when disabling is done by pooler.</param>
        public virtual void Disable(bool recursive = true)
        {
            active = false;
        }


        /// <summary>
        /// Is object active.
        /// </summary>
        public bool Active { get { return active; } set { if (value) active = true; else Disable(); } }
        private bool active;
    }
}