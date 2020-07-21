using System;
using System.Collections;
using WormGame.Core;

namespace WormGame.Pooling
{
    /// @author Antti Harju
    /// @version 21.07.2020
    /// <summary>
    /// Object pooler.
    /// </summary>
    /// <typeparam name="T">Poolable object type</typeparam>
    public class Pooler<T> : IEnumerable where T : class, IPoolable
    {
#if DEBUG
        private readonly string type;
#endif
        private readonly int endIndex;

        public int EnableIndex { get; private set; }


        /// <summary>
        /// Returns the pool.
        /// </summary>
        public T[] Pool { get; }


        /// <summary>
        /// Returns pools length.
        /// </summary>
        public int Count { get { return Pool.Length; } }


        /// <summary>
        /// Initializes the object pool. PoolableEntities have to be manually added to the scene through Pool property.
        /// </summary>
        /// <param name="config">Configuration object</param>
        /// <param name="size">Pool size</param>
        public Pooler(Config config, int size)
        {
            endIndex = size - 1;
            Pool = new T[size];
            for (int i = 0; i < size; i++)
            {
                T currentPoolable;
                try
                {
                    currentPoolable = (T)Activator.CreateInstance(typeof(T), new object[] { config });
                }
                catch (MissingMethodException)
                {
                    currentPoolable = (T)Activator.CreateInstance(typeof(T), new object[] { });
                }
                currentPoolable.Enabled = false;
                currentPoolable.Id = i;
                Pool[i] = currentPoolable;
            }
#if DEBUG
            System.Text.RegularExpressions.MatchCollection matches = System.Text.RegularExpressions.Regex.Matches("" + Pool[0].GetType(), @"\.([^\.]*)$");
            if (matches.Count > 0 && matches[0].Groups.Count > 1)
                type = matches[0].Groups[1].Value;
            else
                throw new Exception("Couldn't solve poolable type.");
#endif
        }


        /// <summary>
        /// Enables a disabled object from the pool.
        /// </summary>
        /// <returns>Enabled object</returns>
        public T Enable()
        {
            if (EnableIndex == endIndex && Pool[EnableIndex].Enabled)
                if (Defrag())
                    return null;
            int newEntity = EnableIndex;
            Pool[newEntity].Enabled = true;
            if (EnableIndex != endIndex)
                EnableIndex++;
            return Pool[newEntity];
        }


        /// <summary>
        /// Check if the pool has required amount of poolables available.
        /// </summary>
        /// <param name="amount">How many poolables are needed</param>
        /// <returns>Is asked amount of poolables available</returns>
        public bool Check(int amount)
        {
            if (EnableIndex <= Count - amount)
                return true;
            Defrag();
            return EnableIndex <= Count - amount;
        }


        /// <summary>
        /// Defragments the pool in a way that disabled poolables are at the end of the array, readily available to be enabled.
        /// </summary>
        /// <returns>Are all poolables enabled</returns>
        /// <example>
        /// Before: [.2.45]
        ///              ^
        /// After:  [524..]
        ///             ^
        /// . = disabled, [number] = enabled, ^ = EnableIndex
        /// <pre name="test">
        ///  Config testConfig = new Config();
        ///  Pooler<PoolableObject> testPool = new Pooler<PoolableObject>(testConfig, 5);
        ///  PoolableObject p1 = testPool.Enable();
        ///  PoolableObject p2 = testPool.Enable();
        ///  PoolableObject p3 = testPool.Enable();
        ///  PoolableObject p4 = testPool.Enable();
        ///  PoolableObject p5 = testPool.Enable();
        ///  p1.Disable();
        ///  p3.Disable();
        ///  testPool[0] === p1;
        ///  testPool[1] === p2;
        ///  testPool[2] === p3;
        ///  testPool[3] === p4;
        ///  testPool[4] === p5;
        ///  testPool[5] === p5; #THROWS IndexOutOfRangeException
        ///  testPool.EnableIndex === 4;
        ///  testPool.Check(2) === true; // Triggers Defrag()
        ///  testPool.Check(3) === false;
        ///  testPool.EnableIndex === 3;
        ///  testPool[0] === p5;
        ///  testPool[1] === p2;
        ///  testPool[2] === p4;
        ///  testPool[3] === p3;
        ///  testPool[4] === p1;
        /// </pre>
        /// </example>
        private bool Defrag()
        {
            int defragAmount = EnableIndex;
            int current = 0;
            while (current < EnableIndex)
            {
                if (Pool[current].Enabled)
                    current++;
                else
                {
                    for (int enabled = EnableIndex; enabled > current; enabled--)
                    {
                        if (Pool[enabled].Enabled)
                        {
                            T swap = Pool[enabled];
                            Pool[enabled] = Pool[current];
                            Pool[current] = swap;
                            current++;
                            break;
                        }
                        else
                            EnableIndex--;
                    }
                }
            }
            defragAmount -= EnableIndex;
#if DEBUG
            ConsoleColor defaultColor = Console.ForegroundColor;
            if (defragAmount == 0)
            {   // Defragmentation didn't free any poolables: pool is fully utilized. This shouldn't happen with correct pool size.
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("[POOLER] ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{type,-11}");
                Console.ForegroundColor = defaultColor;
            }
            else
            {   // Defragmentation freed {defragAmount} of poolables back to use.
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write($"[POOLER] ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"{type,-11}");
                Console.ForegroundColor = defaultColor;
                Console.WriteLine($" {defragAmount} ");
            }
#endif
            return EnableIndex == endIndex;
        }


        /// <summary>
        /// Enables us to foreach through the pool.
        /// </summary>
        /// <returns>Pool enumerator</returns>
        public IEnumerator GetEnumerator()
        {
            return Pool.GetEnumerator();
        }


        /// <summary>
        /// Enables us to for loop through the pool and test it properly.
        /// </summary>
        /// <param name="i">Index</param>
        /// <returns>Poolable at index</returns>
        public T this[int i]
        {
            get { return Pool[i]; }
        }
    }
}
