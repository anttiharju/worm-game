﻿using WormGame.Core;

namespace WormGame.Pooling
{
    /// @author Antti Harju
    /// @version 24.07.2020
    /// <summary>
    /// Class for poolable non-entity objects.
    /// </summary>
    public class PoolableObject : IPoolable
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="config">Configuration</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Required by Pooler")]
        public PoolableObject(Config config) { }


        /// <summary>
        /// Constructor without parameters so inheritors don't need base(config).
        /// </summary>
        public PoolableObject() { }


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