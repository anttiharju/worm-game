﻿using Otter;
using System;

namespace WBGame.Pooling
{
    /// @author Antti Harju
    /// @version 14.06.2020
    /// <summary>
    /// Class that handles entity pooling
    /// </summary>
    /// <typeparam name="T">What type of entity to pool. Has to inherit Poolable.</typeparam>
    class Pooler<T> where T : Poolable
    {
        private Poolable[] pool;


        #region Constructor
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="scene">Scene where we create the pool</param>
        /// <param name="poolSize">How big the pool should be</param>
        /// <param name="poolableSize">How big the entities in the pool should be</param>
        public Pooler(Scene scene, int poolSize, int poolableSize)
        {
            pool = new T[poolSize];
            for (int i = 0; i < pool.Length; i++)
                pool[i] = (T)Activator.CreateInstance(typeof(T), new object[] { poolableSize });

            scene.AddMultiple(pool);
        }
        #endregion


        #region Methods
        /// <summary>
        /// Spawns a poolable entity from the pool at the desired position
        /// </summary>
        /// <param name="x">Horizontal position</param>
        /// <param name="y">Vertical position</param>
        /// <returns></returns>
        public bool Spawn(int x, int y)
        {
            foreach (Poolable poolable in pool)
                if (poolable.Available())
                {
                    poolable.Spawn(x, y);
                    return true;
                }
            return false;
        }
        #endregion
    }
}
