namespace WBGame.Other
{
    /// @author Antti Harju
    /// @version 21.06.2020
    /// <summary>
    /// Abstraction layer that allows the same player to control many different worms one at a time.
    /// </summary>
    class Controls
    {
        private char[] queue;
        private readonly char empty = '-';

        /// <summary>
        /// Constructor. Queue length of 100 is quite absurd but cool for now.
        /// </summary>
        /// <param name="length">how many directions can the queue hold simultaneously</param>
        public Controls(int length = 100)
        {
            queue = new char[length];
            for (int i = 0; i < queue.Length; i++)
                queue[i] = empty;
        }


        /// <summary>
        /// Takes a direction from the queue
        /// </summary>
        /// <returns>Next direction</returns>
        public char Get()
        {
            char tmpChar = empty;
            for (int i = 0; i < queue.Length; i++)
                if (queue[i] != empty)
                {
                    tmpChar = queue[i];
                    queue[i] = empty;
                    break;
                }
            return tmpChar;
        }


        /// <summary>
        /// Adds a direction to the queue. Also sorts the queue because it's kind of expensive and Get() is called more often.
        /// </summary>
        /// <param name="direction">Direction to add to the movement queue</param>
        public void Add(char direction)
        {
            TrimQueue();
            queue[FirstFreeIndex()] = direction;
        }


        /// <summary>
        /// Adds multiple directions to the movement queue. Also sorts it just like Add()
        /// </summary>
        /// <param name="directions">Directions to add to the movement queue</param>
        public void AddMultiple(char[] directions)
        {
            TrimQueue();
            int startIndex = FirstFreeIndex();
            for (int i = startIndex; i < startIndex + directions.Length; i++)
                queue[i] = directions[i - startIndex];
        }


        /// <summary>
        /// Sorts the movement queue so it's fully utilized.
        /// </summary>
        private void TrimQueue()
        {
            for (int i = 0; i < queue.Length; i++)
                if (queue[i] == empty)
                    for (int j = i; j < queue.Length; j++)
                    {
                        if (queue[j] != empty)
                        {
                            queue[i] = queue[j];
                            queue[j] = empty;
                            break;
                        }
                        if (j == queue.Length - 1) return;
                    }
        }


        /// <summary>
        /// Returns the index of the first free spot at the movement queue
        /// </summary>
        /// <returns>First free index of the movement queue</returns>
        private int FirstFreeIndex()
        {
            int index = 0;
            for (int i = 0; i < queue.Length; i++)
                if (queue[i] == empty)
                {
                    index = i;
                    break;
                }
            return index;
        }
    }
}
