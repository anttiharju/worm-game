namespace WBGame.Other
{
    /// @author Antti Harju
    /// @version 26.06.2020
    /// <summary>
    /// Abstraction layer that allows the same player to control many different worms one at a time.
    /// </summary>
    class Controls
    {
        private readonly char[] queue;
        private readonly char empty = '-';


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="length">How many instructions the queue can hold</param>
        public Controls(int length = 10)
        {
            queue = new char[length];
            for (int i = 0; i < queue.Length; i++)
                queue[i] = empty;
        }


        /// <summary>
        /// Takes a direction from the queue
        /// </summary>
        /// <returns>Next direction</returns>
        public char Next()
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
        /// Adds an instruction to the queue. Also Trims the queue. Trim is done in Add methods rather than in Next() because in theory it's more expensive.
        /// </summary>
        /// <param name="instruction">Instruction to add to the movement queue</param>
        public void Add(char instruction)
        {
            System.Console.WriteLine("HELLO");
            TrimQueue();
            queue[FirstFreeIndex()] = instruction;
        }

        /// <summary>
        /// Adds multiple instructions to the queue. Also Trims the queue just like Add()
        /// </summary>
        /// <param name="instructions">Directions to add to the movement queue</param>
        public void AddMultiple(char[] instructions)
        {
            TrimQueue();
            int startIndex = FirstFreeIndex();
            int endIndex = startIndex + instructions.Length;
            endIndex = endIndex < queue.Length ? endIndex : queue.Length;
            for (int i = startIndex; i < endIndex; i++)
                queue[i] = instructions[i - startIndex];
        }


        /// <summary>
        /// Trims the queue so it's fully utilized
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
        /// Returns index of the first free spot in the queue
        /// </summary>
        /// <returns>First free index in the queue</returns>
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