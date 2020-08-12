using System;
using System.Collections;
using Otter.Core;
using WormGame.Core;

namespace WormGame.Pooling
{
    /// @author Antti Harju
    /// @version 12.08.2020
    /// <summary>
    /// Object pooler. It's computationally cheaper to recycle objects by resetting their variables instead of destroying and creating a new ones.
    /// </summary>
    /// <typeparam name="T">Object type</typeparam>
    public class Pooler<T> : IEnumerable where T : class, IPoolable
    {
        private readonly T[] pool;
        private readonly int endIndex;


        /// <summary>
        /// Index to enable disabled objects from. Pool is full when the object at this index is enabled.
        /// </summary>
        public int EnableIndex { get; private set; }


        /// <summary>
        /// Returns pool capacity.
        /// </summary>
        public int Length { get { return pool.Length; } }


        /// <summary>
        /// Initializes pool.
        /// </summary>
        /// <param name="config">Configuration</param>
        /// <param name="capacity">Capacity</param>
        public Pooler(Scene scene, Config config, int capacity)
        {
            pool = new T[capacity];
            endIndex = capacity - 1;
            for (int i = 0; i < capacity; i++)
            {
                T currentPoolable = (T)Activator.CreateInstance(typeof(T), new object[] { config, i });
                currentPoolable.Disable(false);
                pool[i] = currentPoolable;
                currentPoolable.Add(scene);
            }
        }


        /// <summary>
        /// Enable a disabled object. Use object's Spawn method to configure it.
        /// </summary>
        /// <returns>Enabled object</returns>
        public T Enable()
        {
            if (EnableIndex == endIndex && pool[EnableIndex].Active)
                if (Defragment())
                    return null;
            int enabled = EnableIndex;
            pool[enabled].Active = true;
            if (EnableIndex != endIndex)
                EnableIndex++;
            return pool[enabled];
        }


        /// <summary>
        /// Disables all pooler objects.
        /// </summary>
        public void Reset()
        {
            for (int i = EnableIndex; i >= 0; i--)
                if (pool[i].Active)
                    pool[i].Disable(false);
            EnableIndex = 0;
        }


        /// <summary>
        /// Defragments the pool so that enabled objects come first.
        /// </summary>
        /// <returns>Is pool full</returns>
        /// <example>
        /// Before: [.2.45]
        ///              ^
        /// After:  [524..]
        ///             ^
        /// . = disabled, [number] = enabled, ^ = EnablingIndex
        /// <pre name="test">
        ///  Scene scene = new Scene();
        ///  Config config = new Config();
        ///  Pooler<Poolable> pooler = new Pooler<Poolable>(scene, config, 5);
        ///  Poolable p1 = pooler.Enable();
        ///  Poolable p2 = pooler.Enable();
        ///  Poolable p3 = pooler.Enable();
        ///  Poolable p4 = pooler.Enable();
        ///  Poolable p5 = pooler.Enable();
        ///  p1.Disable();
        ///  p3.Disable();
        ///  pooler[0] === p1;
        ///  pooler[1] === p2;
        ///  pooler[2] === p3;
        ///  pooler[3] === p4;
        ///  pooler[4] === p5;
        ///  pooler[5] === p5; #THROWS IndexOutOfRangeException
        ///  pooler.EnableIndex === 4;
        ///  pooler.Defragment();
        ///  pooler.EnableIndex === 3;
        ///  pooler[0] === p5;
        ///  pooler[1] === p2;
        ///  pooler[2] === p4;
        ///  pooler[3] === p3;
        ///  pooler[4] === p1;
        /// </pre>
        /// </example>
        public virtual bool Defragment()
        {
            int current = 0;
            while (current < EnableIndex)
            {
                if (pool[current].Active)
                    current++;
                else
                {
                    for (int enabled = EnableIndex; enabled > current; enabled--)
                    {
                        if (pool[enabled].Active)
                        {
                            T swap = pool[enabled];
                            pool[enabled] = pool[current];
                            pool[current] = swap;
                            current++;
                            break;
                        }
                        EnableIndex--;
                    }
                }
            }
#if DEBUG
            ConsoleColor defaultColor = Console.ForegroundColor;
            if (pool[EnableIndex].Active)
            {   // Pool is full. This can be intentional, but you can fix it by increasing pool capacity.
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("[POOLER] ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{pool[0].GetType().Name,-(11 + 5 * 2)}");
                Console.ForegroundColor = defaultColor;
            }
            else
            {   // Tells how many objects pooler has available.
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write($"[POOLER] ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"{pool[0].GetType().Name,-11} ");
                Console.ForegroundColor = defaultColor;
                Console.WriteLine($"{pool.Length - EnableIndex,5}");
            }
#endif
            return pool[EnableIndex].Active;
        }


        /// <summary>
        /// Enables foreach'ing pooler.
        /// </summary>
        /// <returns>Pool enumerator</returns>
        public IEnumerator GetEnumerator()
        {
            return pool.GetEnumerator();
        }


        /// <summary>
        /// Enables for-loop'ing pooler. Also required for tests.
        /// </summary>
        /// <param name="i">Index</param>
        /// <returns>Object at index</returns>
        public T this[int i]
        {
            get { return pool[i]; }
        }
    }
}
