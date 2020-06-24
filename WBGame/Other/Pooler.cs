using Otter;
using System;
using System.Collections;
using System.Collections.Generic;

namespace WBGame.Other
{
    /// @author Antti Harju
    /// @version 24.06.2020
    /// <summary>
    /// Object that manages a pool
    /// </summary>
    /// <typeparam name="T">What is stored in the pool</typeparam>
    public class Pooler<T> : IEnumerable where T : Entity
    {
        //TODO: Figure out how to make a custom enumator so I can just use an array []
        private readonly List<T> enabledPool;
        private readonly List<T> disabledPool;
        private T tmp;

        public int Count { get { return enabledPool.Count; } }


        /// <summary>
        /// Constructor, initializes the pools.
        /// </summary>
        /// <param name="scene">Where the entities should be</param>
        /// <param name="capacity">How many entities does the pool need</param>
        /// <param name="entitySize">How big entities are</param>
        public Pooler(Scene scene, int capacity, int entitySize)
        {
            enabledPool = new List<T>(capacity);
            disabledPool = new List<T>(capacity);
            for (int i = 0; i < capacity; i++)
            {
                tmp = (T)Activator.CreateInstance(typeof(T), new object[] { entitySize });
                tmp.Graphic.Visible = false;
                disabledPool.Add(tmp);
            }
            scene.AddMultiple(disabledPool.ToArray());
        }


        /// <summary>
        /// Enables an entity from the disabled pool
        /// </summary>
        /// <returns>Enabled entity</returns>
        public T Enable()
        {
            if (disabledPool.Count < 1)
                return null;
            tmp = disabledPool[0];
            disabledPool.Remove(tmp);
            enabledPool.Add(tmp);
            tmp.Graphic.Visible = true;
            return tmp;
        }


        /// <summary>
        /// Disables given entitty
        /// </summary>
        /// <returns>Unused entity</returns>
        public T Disable(T entityToDisable)
        {
            enabledPool.Remove(entityToDisable);
            disabledPool.Add(entityToDisable);
            tmp = disabledPool[0];
            disabledPool.Remove(tmp);
            enabledPool.Add(tmp);
            tmp.Graphic.Visible = false;
            return tmp;
        }


        /// <summary>
        /// So we can foreach through the pool
        /// </summary>
        /// <returns>pool</returns>
        public IEnumerator GetEnumerator()
        {
            return enabledPool.GetEnumerator();
        }


        /// <summary>
        /// So we can access entities at specifix indexes
        /// </summary>
        /// <param name="index">index</param>
        /// <returns>Object from the pool at the index</returns>
        public T this[int index]
        {
            get
            {
                return enabledPool[index];
            }
        }
    }
}
