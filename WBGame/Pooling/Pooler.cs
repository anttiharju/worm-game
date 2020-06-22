using Otter;
using System;
using System.Collections;

namespace WBGame.Pooling
{
    /// @author Antti Harju
    /// @version 21.06.2020
    /// <summary>
    /// Object that manages a pool
    /// </summary>
    /// <typeparam name="T">What is stored in the pool</typeparam>
    public class Pooler<T> : IEnumerable where T : Poolable
    {
        private readonly T[] pool;

        public int Length { get { return pool.Length; } }

        /// <summary>
        /// Indexer. Nothing special here
        /// </summary>
        /// <param name="index">index</param>
        /// <returns>Object from the pool at the index</returns>
        public T this[int index]
        {
            get
            {
                return (T)pool[index];
            }
        }


        /// <summary>
        /// Spawns (and disables) the pool entities to the scene
        /// </summary>
        /// <param name="scene">Where the entities should be</param>
        /// <param name="poolSize">How many entities does the pool need</param>
        /// <param name="poolableSize">How big entities are</param>
        public Pooler(Scene scene, int poolSize, int poolableSize)
        {
            pool = new T[poolSize];
            for (int i = 0; i < pool.Length; i++)
            {
                pool[i] = (T)Activator.CreateInstance(typeof(T), new object[] { poolableSize });
                pool[i].Disable();
            }
            scene.AddMultiple(pool);
        }


        /// <summary>
        /// Gives the first free entity from the pool
        /// </summary>
        /// <returns>Unused entity</returns>
        public T Next()
        {
            foreach (T entity in pool)
                if (!entity.Enabled)
                    return (T)entity;
            Console.WriteLine(this + " ran out of poolables.");
            return null;
        }

        /// <summary>
        /// Now we can foreach through the pool
        /// </summary>
        /// <returns>pool</returns>
        public IEnumerator GetEnumerator()
        {
            return pool.GetEnumerator();
        }
    }
}
