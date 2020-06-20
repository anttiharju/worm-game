using System;
using System.Collections.Generic;
using System.Text;

namespace WBGame.Other
{
    class Queue
    {
        private readonly int[] queue;


        public Queue(int length = 10)
        {
            queue = new int[length];
        }


        public void Add(int direction)
        {
            for (int i = 0; i < queue.Length; i++)
                if (queue[i] == 0)
                {
                    queue[i] = direction;
                    break;
                }
        }


        public int Get()
        {
            int tmpValue = queue[0];
            queue[0] = 0;
            for (int i = 1; i < queue.Length; i++)
            {
                queue[i - 1] = queue[i];
                queue[i] = 0;
            }
            return tmpValue;
        }
    }
}
