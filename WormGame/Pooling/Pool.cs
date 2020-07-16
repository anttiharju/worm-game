using System;
using System.Collections;
using WormGame.Core;

namespace WormGame.Pooling
{
    /// @author Antti Harju
    /// @version 13.07.2020
    /// <summary>
    /// Entity pooler.
    /// </summary>
    /// <typeparam name="T">Poolable entity type</typeparam>
    /// TODO: Write tests
    public class Pool<T> : IEnumerable where T : class, IPoolable
    {
        private readonly T[] pool;
        private readonly int lastIndex;
        private int enablingIndex = 0;


        /// <summary>
        /// Returns pool max capacity.
        /// </summary>
        public int Count { get { return pool.Length; } }


        /// <summary>
        /// Returns pooled objects.
        /// </summary>
        public T[] Objects { get { return pool; } }


        /// <summary>
        /// Initializes the object pool. If you're pooling Poolables (not BasicPoolables) they have to be manually added to the scene with GetPool().
        /// </summary>
        /// <param name="config">Configuration object</param>
        /// <param name="capacity">Pool size</param>
        public Pool(Config config, int capacity)
        {
            lastIndex = capacity - 1;
            pool = new T[capacity];
            for (int i = 0; i < capacity; i++)
            {
                T tmp = (T)Activator.CreateInstance(typeof(T), new object[] { config });
                tmp.Enabled = false;
                pool[i] = tmp;
            }
        }


        /// <summary>
        /// Enables a disabled entity from the pool.
        /// </summary>
        /// <returns>Enabled entity</returns>
        public T Enable()
        {
            if (enablingIndex == lastIndex && pool[enablingIndex].Enabled)
            {
                Defrag(); // Moves enablingIndex
                if (enablingIndex == lastIndex)
                {
#if DEBUG
                    Console.WriteLine("[EMPTY] " + this);
#endif
                    return null;
                }
            }
            int current = enablingIndex;
            pool[current].Enabled = true;
            if (enablingIndex != lastIndex)
                enablingIndex++;
            return pool[current];
        }


        /// <summary>
        /// Defragments the pool. This is way cheaper than looping through the entire pool every time something needs to be enabled.
        /// </summary>
        private void Defrag()
        {
            int i = 0;
            while (i < enablingIndex)
            {
                if (pool[i].Enabled)
                    i++;
                else
                {
                    for (int j = enablingIndex; j > i; j--)
                    {
                        if (pool[j].Enabled)
                        {
                            T tmp = pool[j];
                            pool[j] = pool[i];
                            pool[i] = tmp;
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
        /// We need to be able to loop through the pools contents to make calls to the entities in it.
        /// I would love to give an enumerator that only has the enabled ones, but that would mean 
        /// disabling poolables through the pool and I don't want that.
        /// </summary>
        /// <returns>Pool enumerator</returns>
        public IEnumerator GetEnumerator()
        {
            return pool.GetEnumerator();
        }
    }
}
