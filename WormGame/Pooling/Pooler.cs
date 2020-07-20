using System;
using System.Collections;
using WormGame.Core;

namespace WormGame.Pooling
{
    /// @author Antti Harju
    /// @version 20.07.2020
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

        private int enablingIndex = 0;


        /// <summary>
        /// Returns the pool.
        /// </summary>
        public T[] Pool { get; }


        /// <summary>
        /// Returns pools length.
        /// </summary>
        public int Size { get { return Pool.Length; } }


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
                T tmp = (T)Activator.CreateInstance(typeof(T), new object[] { config });
                tmp.Enabled = false;
                tmp.Id = i;
                Pool[i] = tmp;
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
            if (enablingIndex == endIndex && Pool[enablingIndex].Enabled)
                if (Sort())
                    return null;
            int newEntity = enablingIndex;
            Pool[newEntity].Enabled = true;
            if (enablingIndex != endIndex)
                enablingIndex++;
            return Pool[newEntity];
        }


        /// <summary>
        /// Ask if the pool has required amount of poolables available.
        /// </summary>
        /// <param name="amount">How many poolables are needed</param>
        /// <returns>Is asked amount of poolables available</returns>
        public bool Ask(int amount)
        {
            if (enablingIndex <= Size - amount)
                return true;
            Sort();
            return enablingIndex <= Size - amount;
        }


        /// <summary>
        /// Sorts the pool in a way that disabled poolables are at the end of the pool array, readily available to be enabled.
        /// </summary>
        /// <returns>did sorting make more poolables available to enable</returns>
        /// <example>
        /// Before: [.2.45]
        ///              ^
        /// After:  [524..]
        ///             ^
        /// . = available, [number] = in use, ^ = enablingIndex
        /// <pre name="test">
        ///  #if DEBUG
        ///  Config testConfig = new Config();
        ///  Pooler<PoolableObject> testPool = new Pooler<PoolableObject>(testConfig, 5);
        ///  PoolableObject one = testPool.Enable();
        ///  PoolableObject two = testPool.Enable();
        ///  PoolableObject thr = testPool.Enable();
        ///  PoolableObject fou = testPool.Enable();
        ///  PoolableObject fiv = testPool.Enable();
        ///  one.Disable();
        ///  thr.Disable();
        ///  testPool[0] === one;
        ///  testPool[1] === two;
        ///  testPool[2] === thr;
        ///  testPool[3] === fou;
        ///  testPool[4] === fiv;
        ///  testPool[5] = fiv; #THROWS IndexOutOfRangeException
        ///  testPool[0] === one;
        ///  testPool.GetEnablingIndex() === 4;
        ///  testPool.Ask(2) === true; // Triggers Defragment()
        ///  testPool.Ask(3) === false;
        ///  testPool.GetEnablingIndex() === 3;
        ///  testPool[0] === fiv;
        ///  testPool[1] === two;
        ///  testPool[2] === fou;
        ///  #else
        ///  throw new Exception("Run tests in Debug mode.");
        ///  #endif
        /// </pre>
        /// </example>
        private bool Sort()
        {
#if DEBUG
            int freedAmount = enablingIndex;
#endif
            int current = 0;
            while (current < enablingIndex)
            {
                if (Pool[current].Enabled)
                    current++;
                else
                {
                    for (int enabled = enablingIndex; enabled > current; enabled--)
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
                            enablingIndex--;
                    }
                }
            }
#if DEBUG
            freedAmount -= enablingIndex;
            ConsoleColor defaultColor = Console.ForegroundColor;
            if (freedAmount == 0)
            {   // Defragmentation didn't free any poolables: pool is fully utilized. This shouldn't happen with correct pool size.
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("[POOLER] ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{type,-11}");
                Console.ForegroundColor = defaultColor;
            }
            else
            {   // Defragmentation freed {freedAmount} poolables back to use.
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write($"[POOLER] ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"{type,-11}");
                Console.ForegroundColor = defaultColor;
                Console.WriteLine($" {freedAmount} ");
            }
#endif
            return enablingIndex == endIndex;
        }


        /// <summary>
        /// Enables us to loop through the pool, e.g. to move the poolables in it.
        /// </summary>
        /// <returns>Pool enumerator</returns>
        public IEnumerator GetEnumerator()
        {
            return Pool.GetEnumerator();
        }

#if DEBUG
        /// <summary>
        /// Don't use! Only for testing.
        /// </summary>
        /// <returns>enablingIndex</returns>
        public int GetEnablingIndex()
        {
            return enablingIndex;
        }


        /// <summary>
        /// Don't use! Only for testing.
        /// </summary>
        /// <param name="i">Index</param>
        /// <returns>Poolable at index</returns>
        public T this[int i]
        {
            get { return Pool[i]; }
            set { Pool[i] = value; }
        }
#endif
    }
}
