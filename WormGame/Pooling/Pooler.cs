using System;
using System.Collections;
using Otter.Core;
using WormGame.Core;
using WormGame.Static;

namespace WormGame.Pooling
{
    /// @author Antti Harju
    /// @version 14.08.2020
    /// <summary>
    /// Object pooler. It's computationally cheaper to recycle objects by resetting their variables instead of destroying and creating a new ones.
    /// </summary>
    /// <typeparam name="T">Object type</typeparam>
    public class Pooler<T> : IEnumerable where T : class, IPoolable
    {
        protected T[] pool;
        protected int endIndex;


        /// <summary>
        /// Initialize pool.
        /// </summary>
        /// <param name="config">Configuration</param>
        /// <param name="capacity">Pool length</param>
        public Pooler(Scene scene, Config config, int capacity)
        {
            pool = new T[capacity];
            endIndex = capacity - 1;
            for (int i = 0; i < capacity; i++)
            {
                T current = (T)Activator.CreateInstance(typeof(T), new object[] { config });
                current.Disable(false);
                current.Add(scene);
                pool[i] = current;
            }
        }


        /// <summary>
        /// Base constructor for custom poolers.
        /// </summary>
        public Pooler() { }


        /// <summary>
        /// Defragments the pool in a way that puts enabled objects at the beginning of the pool array.
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
        ///  pooler[pooler.EnableIndex].Active === true;
        ///  pooler.Defragment();
        ///  pooler.EnableIndex === 3;
        ///  pooler[pooler.EnableIndex].Active === false;
        ///  pooler[0] === p5;
        ///  pooler[1] === p2;
        ///  pooler[2] === p4;
        ///  pooler[3] === p3;
        ///  pooler[4] === p1;
        /// </pre>
        /// </example>
        public bool Defragment()
        {
            int i = 0;
            while (i < Index)
            {
                if (pool[i].Active)
                    i++;
                else
                {
                    for (int enabled = Index; enabled > i; enabled--)
                    {
                        if (pool[enabled].Active)
                        {
                            T swap = pool[enabled];
                            pool[enabled] = pool[i];
                            pool[i] = swap;
                            i++;
                            break;
                        }
                        Index--;
                    }
                }
            }
#if DEBUG
            ConsoleColor defaultColor = Console.ForegroundColor;
            if (pool[Index].Active)
            {   // Pool is full. This can be intentional, but in this project it shouldn't happen.
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write("[Pooling] ");
                if (Help.color)
                    Console.ForegroundColor = ConsoleColor.Red;
                else
                    Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine($"{pool[0].GetType().Name,-(11 + 5 * 2)}");
            }
            else
            {   // Tells how many objects pooler has available.
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write($"[Pooling] ");
                if (Help.color)
                    Console.ForegroundColor = ConsoleColor.Green;
                else
                    Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write($"{pool[0].GetType().Name,-11} ");
                Console.WriteLine($"{pool.Length - Index,5}");
            }
            Console.ForegroundColor = defaultColor;
            Help.color = !Help.color;
#endif
            return pool[Index].Active;
        }


        /// <summary>
        /// If all disabling is done through this, defragmentation won't trigger. Not sure if works properly.
        /// </summary>
        /// <param name="disableIndex"></param>
        public void Disable(int disableIndex)
        {
            pool[disableIndex].Disable(false);
            if (!pool[Index].Active)
                Index--;
            T swap = pool[disableIndex];
            pool[disableIndex] = pool[Index];
            pool[Index] = swap;
        }


        /// <summary>
        /// Enable a disabled object. Use object's Spawn method to configure it.
        /// </summary>
        /// <returns>Enabled object</returns>
        public T Enable()
        {
            if (pool[Index].Active)
                if (Defragment())
                    return null;
            int activated = Index;
            pool[activated].Active = true;
            if (Index < endIndex)
                Index++;
            return pool[activated];
        }


        /// <summary>
        /// Points to a disabled object. If the pool is at max capacity, the object is enabled.
        /// </summary>
        public int Index { get; protected set; }


        /// <summary>
        /// Enables the usage of foreach.
        /// </summary>
        /// <returns>Pool enumerator</returns>
        public IEnumerator GetEnumerator()
        {
            return pool.GetEnumerator();
        }


        /// <summary>
        /// Indexer. Enables the usage of for loop. Required for testing purposes.
        /// </summary>
        /// <param name="i">Index</param>
        /// <returns>Object at index</returns>
        public T this[int i]
        {
            get { return pool[i]; }
        }


        /// <summary>
        /// Returns pool capacity.
        /// </summary>
        public int Length { get { return pool.Length; } }


        /// <summary>
        /// Disables all pooler objects.
        /// </summary>
        public virtual void Reset()
        {
            for (int i = Index; i >= 0; i--)
                if (pool[i].Active)
                    pool[i].Disable(false);
            Index = 0;
        }
    }
}
