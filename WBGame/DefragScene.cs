using Otter;
using WBGame.Other;
using WBGame.GameObject;

namespace WBGame
{
    class DefragScene : Scene
    {
        private Pooler<Block> blocks;

        public override void Begin()
        {
            blocks = new Pooler<Block>(this, 5, 32);
        }

        public override void Update()
        {
            if (Input.KeyPressed(Key.Num1))
            {
                Block tmp = blocks.Enable();
                if(tmp != null)
                {
                    tmp.X = Helper.RandomRange(1, 10) * 32 + 32;
                    tmp.Y = Helper.RandomRange(1, 10) * 32 + 32;
                    tmp.Color = Helper.RandomColor();
                }
            }

            if (Input.KeyPressed(Key.Num2))
            {
                foreach (Block block in blocks)
                {
                    if (block.Enabled)
                    {
                        block.Enabled = false;
                        break;
                    }
                }
            }
        }
    }
}
