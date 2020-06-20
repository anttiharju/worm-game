using System;
using System.Collections.Generic;
using System.Text;
using WBGame.Pooling;

namespace WBGame.Misc
{
    class Bunch : Poolable
    {
        Block[] blocks;

        public Bunch(int size)
        {
            blocks = new Block[size];
        }

        public void SetBlock(Block block, int i)
        {
            blocks[i] = block;
        }
    }
}
