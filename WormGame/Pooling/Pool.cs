using System;
using System.Collections;
using WormGame.Core;

namespace WormGame.Pooling
{
    /// @author Antti Harju
    /// @version 08.07.2020
    /// <summary>
    /// Object pooler.
    /// </summary>
    /// <typeparam name="T">Poolable object type</typeparam>
    /// TODO: Write tests
    public class Pool<T> : IEnumerable where T : class, IPoolable
    {
        private readonly T[] pool;
        private readonly int lastIndex;
        private int enablingIndex = 0;

        /// <summary>
        /// Initializes the object pool.
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
        /// If the pooled objects are Otter2d entities we can add them to the scene with this.
        /// Not sure if it's possible to do in-class due to how generics work.
        /// </summary>
        /// <returns>Object pool</returns>
        public T[] GetPool()
        {
            return pool;
        }


        /// <summary>
        /// Enables a disabled object from the pool.
        /// </summary>
        /// <returns>Enabled entity</returns>
        public T Enable()
        {
            if (enablingIndex == lastIndex && pool[enablingIndex].Enabled)
            {
                Defrag(); // Moves enablingIndex
                if (enablingIndex == lastIndex)
                {
                    Console.WriteLine("[EMPTY] " + this);
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
        /// We need to be able to loop through the pools contents to make calls to them etc.
        /// I would love to give an enumerator that only has the enabled ones, but I don't
        /// want to disable entities through the pool as the code would get really messy.
        /// </summary>
        /// <returns>Pool enumerator</returns>
        public IEnumerator GetEnumerator()
        {
            return pool.GetEnumerator();
        }
    }
}
