using WBGame.Pooling;

namespace WBGame.GameObject
{
    class Bunch : Poolable
    {
        private readonly Block[] blocks;


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
