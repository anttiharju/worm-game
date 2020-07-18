using System;
using System.Collections;
using WormGame.Core;

namespace WormGame.Pooling
{
    /// @author Antti Harju
    /// @version 18.07.2020
    /// <summary>
    /// Object pooler.
    /// </summary>
    /// <typeparam name="T">Poolable object type</typeparam>
    public class Pooler<T> : IEnumerable where T : class, IPoolable
    {
        private readonly int endIndex;

        private int enablingIndex = 0;


        /// <summary>
        /// Returns the pool.
        /// </summary>
        public T[] Pool { get; }


        /// <summary>
        /// Returns pools capacity.
        /// </summary>
        public int Count { get { return Pool.Length; } }


        /// <summary>
        /// Initializes the object pool. PoolableEntities have to be manually added to the scene through Pool property.
        /// </summary>
        /// <param name="config">Configuration object</param>
        /// <param name="capacity">Pool size</param>
        public Pooler(Config config, int capacity)
        {
            endIndex = capacity - 1;
            Pool = new T[capacity];
            for (int i = 0; i < capacity; i++)
            {
                T tmp = (T)Activator.CreateInstance(typeof(T), new object[] { config });
                tmp.Enabled = false;
                Pool[i] = tmp;
            }
        }


        /// <summary>
        /// Enables a disabled object from the pool.
        /// </summary>
        /// <returns>Enabled object</returns>
        public T Enable()
        {
            if (enablingIndex == endIndex && Pool[enablingIndex].Enabled)
            {
                Sort(); // Moves enablingIndex
                if (enablingIndex == endIndex)
                {
#if DEBUG
                    Console.WriteLine("[POOLER] Ran out of poolables (" + Pool[0].GetType() + ").");
#endif
                    return null;
                }
            }
            int current = enablingIndex;
            Pool[current].Enabled = true;
            if (enablingIndex != endIndex)
                enablingIndex++;
            return Pool[current];
        }


        /// <summary>
        /// Sorts the pools so that objects in use are at the beginning.
        /// </summary>
        private void Sort()
        {
            int i = 0;
            while (i < enablingIndex)
            {
                if (Pool[i].Enabled)
                    i++;
                else
                {
                    for (int j = enablingIndex; j > i; j--)
                    {
                        if (Pool[j].Enabled)
                        {
                            T tmp = Pool[j];
                            Pool[j] = Pool[i];
                            Pool[i] = tmp;
                            i++;
                            break;
                        }
                        else
                            enablingIndex--;
                    }
                }
            }
        }


        /// <summary>
        /// So we can foreach through the objects in the pool. Used in wormScene.
        /// </summary>
        /// <returns>Pool enumerator</returns>
        public IEnumerator GetEnumerator()
        {
            return Pool.GetEnumerator();
        }
    }
}
