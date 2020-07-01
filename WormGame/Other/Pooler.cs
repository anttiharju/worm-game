using Otter;
using System;
using System.Collections;

namespace WormGame.Other
{
    /// @author Antti Harju
    /// @version 28.06.2020
    /// <summary>
    /// Entity pooler.
    /// </summary>
    /// <typeparam name="T">Poolable entity type</typeparam>
    public class Pooler<T> : IEnumerable where T : Poolable
    {
        private readonly T[] pool;
        private readonly int lastIndex;
        private int enablingIndex = 0;

        /// <summary>
        /// Initializes pool and poolable entities.
        /// </summary>
        /// <param name="scene">scene to add the objects to</param>
        /// <param name="capacity">pool size</param>
        /// <param name="entitySize">pooled entity size</param>
        public Pooler(Scene scene, int capacity, int entitySize)
        {
            lastIndex = capacity - 1;
            pool = new T[capacity];
            for (int i = 0; i < capacity; i++)
            {
                T tmp = (T)Activator.CreateInstance(typeof(T), new object[] { entitySize });
                tmp.Graphic.Visible = false;
                pool[i] = tmp;
            }
            scene.AddMultiple(pool);
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
        /// Defragments the pool. This is way cheaper than looping through the entire pool
        /// every time something needs to be enabled.
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
        /// So we can foreach through the pool. Once the project uses the new collision 
        /// system and we don't need to debug pooling this can be removed. Although may 
        /// be useful to writing tests.
        /// </summary>
        /// <returns>Pool enumerator</returns>
        public IEnumerator GetEnumerator()
        {
            return pool.GetEnumerator();
        }
    }
}
