using Otter;
using System;
using System.Collections;

namespace WBGame.Other
{
    public class Pooler<T> : IEnumerable where T : Poolable
    {
        private readonly T[] pool;


        public Pooler(Scene scene, int capacity, int entitySize)
        {
            pool = new T[capacity];
            for (int i = 0; i < capacity; i++)
            {
                T tmp = (T)Activator.CreateInstance(typeof(T), new object[] { entitySize });
                tmp.Graphic.Visible = false;
                pool[i] = tmp;
            }
            scene.AddMultiple(pool);
        }


        public T Enable()
        {
            foreach (T poolable in pool)
                if (!poolable.Enabled)
                {
                    poolable.Enabled = true;
                    return poolable;
                }
            return null;
        }


        public void Disable(T entity)
        {
            entity.Enabled = false;
        }


        public IEnumerator GetEnumerator()
        {
            return pool.GetEnumerator();
        }
    }
}
