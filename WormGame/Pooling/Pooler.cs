using System;
using System.Collections;
using WormGame.Core;

namespace WormGame.Pooling
{
    /// @author Antti Harju
    /// @version 24.07.2020
    /// <summary>
    /// Object pooler.
    /// </summary>
    /// <typeparam name="T">Poolable object type</typeparam>
    public class Pooler<T> : IEnumerable where T : class, IPoolable
    {
#if DEBUG
        private readonly string type;
#endif
        private readonly T[] pool;
        private readonly int endIndex;


        /// <summary>
        /// The index from where new poolables are enabled from.
        /// </summary>
        public int EnableIndex { get; private set; }


        /// <summary>
        /// Get pool length.
        /// </summary>
        public int Count { get { return pool.Length; } }


        /// <summary>
        /// Initializes pool.
        /// </summary>
        /// <param name="config">Configuration object</param>
        /// <param name="size">Pool size</param>
        public Pooler(Config config, int size, WormScene scene = null)
        {
            endIndex = size - 1;
            pool = new T[size];
            for (int i = 0; i < size; i++)
            {
                T currentPoolable;
                currentPoolable = (T)Activator.CreateInstance(typeof(T), new object[] { config });
                currentPoolable.Enabled = false;
                currentPoolable.Id = i;
                pool[i] = currentPoolable;
                if (currentPoolable is PoolableEntity entity)
                {
                    if (scene == null)
                        throw new Exception("You can't pool entities without giving the scene as parameter."); ;
                    entity.Add(scene);
                }
            }
#if DEBUG
            System.Text.RegularExpressions.MatchCollection matches = System.Text.RegularExpressions.Regex.Matches("" + pool[0].GetType(), @"\.([^\.]*)$");
            if (matches.Count > 0 && matches[0].Groups.Count > 1)
                type = matches[0].Groups[1].Value;
            else
                throw new Exception("Couldn't solve poolable type.");
#endif
        }


        /// <summary>
        /// Enable a disabled poolable.
        /// </summary>
        /// <returns>Enabled poolable</returns>
        public T Enable()
        {
            if (EnableIndex == endIndex && pool[EnableIndex].Enabled)
                if (Sort())
                    return null;
            int newEntity = EnableIndex;
            pool[newEntity].Enabled = true;
            if (EnableIndex != endIndex)
                EnableIndex++;
            return pool[newEntity];
        }


        /// <summary>
        /// Check if the pool has enough poolables available.
        /// </summary>
        /// <param name="amount">Needed poolable amountd</param>
        /// <returns>Is asked amount of poolables available</returns>
        public bool HasAvailable(int amount)
        {
            if (EnableIndex <= Count - amount)
                return true;
            Sort();
            return EnableIndex <= Count - amount;
        }


        /// <summary>
        /// Sorts (defragments) the pool in a way that disabled poolables are at the end of the array, readily available to be enabled.
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
        ///  testPool.HasAvailable(2) === true; // Triggers Sort()
        ///  testPool.HasAvailable(3) === false;
        ///  testPool.EnableIndex === 3;
        ///  testPool[0] === p5;
        ///  testPool[1] === p2;
        ///  testPool[2] === p4;
        ///  testPool[3] === p3;
        ///  testPool[4] === p1;
        /// </pre>
        /// </example>
        private bool Sort()
        {
#if DEBUG
            int freedAmount = EnableIndex;
#endif
            int current = 0;
            while (current < EnableIndex)
            {
                if (pool[current].Enabled)
                    current++;
                else
                {
                    for (int enabled = EnableIndex; enabled > current; enabled--)
                    {
                        if (pool[enabled].Enabled)
                        {
                            T swap = pool[enabled];
                            pool[enabled] = pool[current];
                            pool[current] = swap;
                            current++;
                            break;
                        }
                        else
                            EnableIndex--;
                    }
                }
            }
#if DEBUG
            freedAmount -= EnableIndex;
            ConsoleColor defaultColor = Console.ForegroundColor;
            if (freedAmount == 0)
            {   // Sorting didn't free any poolables: pool is fully utilized. This shouldn't happen with the correct pool size.
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("[POOLER] ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{type,-11}");
                Console.ForegroundColor = defaultColor;
            }
            else
            {   // Sorting freed {freedAmount} of poolables back to use.
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write($"[POOLER] ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"{type,-11}");
                Console.ForegroundColor = defaultColor;
                Console.WriteLine($" {freedAmount}     ");
            }
#endif
            return EnableIndex == endIndex;
        }


        /// <summary>
        /// Enables us to foreach the pool.
        /// </summary>
        /// <returns>Pool enumerator</returns>
        public IEnumerator GetEnumerator()
        {
            return pool.GetEnumerator();
        }


        /// <summary>
        /// Enables us to for-loop the pool. Also required for testing.
        /// </summary>
        /// <param name="i">Index</param>
        /// <returns>Poolable at index</returns>
        public T this[int i]
        {
            get { return pool[i]; }
        }
    }
}
