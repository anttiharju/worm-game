using Otter;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace WBGame.Area
{
    class Pooler<T> where T : Poolable
    {
        private Poolable[] pool;

        public Pooler(Scene scene, int poolSize, int poolableSize)
        {
            pool = new T[poolSize];
            for (int i = 0; i < pool.Length; i++)
                pool[i] = (T)Activator.CreateInstance(typeof(T), new object[] { 0, 0, poolableSize });

            scene.AddMultiple(pool);
        }

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
    }
}
