using Otter;
using WBGame.Worm;
using WBGame.Pooling;

namespace WBGame.Other
{
    class Manager
    {
        private int collisionSize;
        private Scene scene;
        private Pooler<Body> bodyPool;
        private Pooler<Head> headPool;
        private Pooler<Block> blockPool;


        public Manager(Scene scene, int bodyCount, int headCount, int blockCount, int size)
        {
            this.scene = scene;
            bodyPool = new Pooler<Body>(scene, bodyCount, size);
            headPool = new Pooler<Head>(scene, headCount, size);
            blockPool = new Pooler<Block>(scene, blockCount, size);
            collisionSize = (int)(0.9f * size);
        }


        public Head SpawnWorm(int x, int y, int length, Color color)
        {
            Head worm = headPool.Next().Spawn(this, x, y, length, Color.Red);

            int bodyCount = length - 1; // - 1 because head is already counts as 1
            Body currentBody = worm;
            for (int i = 0; i < bodyCount; i++)
            {
                Body tmpBody = bodyPool.Next();
                tmpBody.Enable();
                tmpBody.Position = new Vector2(x, y);
                tmpBody.SetTarget(x, y);
                tmpBody.SetColor(color);
                currentBody.SetNextBody(tmpBody);
                currentBody = tmpBody;
            }
            return worm;
        }


        public Player SpawnPlayer(Head worm)
        {
            Player tmpPlayer = new Player(this, worm);
            scene.Add(tmpPlayer);
            return tmpPlayer;
        }


        public void Blockify(Head worm)
        {
            Color color = worm.GetColor();
            Vector2[] blockPositions = worm.GetPositions(new Vector2[worm.GetLength()]);
            for (int i = 0; i < blockPositions.Length; i++)
            {
                Block tmpBlock = blockPool.Next();
                if (tmpBlock == null) break;
                tmpBlock.Spawn(blockPositions[i], color);
            }
        }


        public bool CanMove(Head asker, Vector2 newPosition)
        {
            foreach (Head head in headPool)
                if (head.Enabled() && Math.RoughlyEquals(head.GetTarget(), newPosition, collisionSize) && head != asker)
                    return false;
            foreach (Body body in bodyPool)
                if (body.Enabled() && Math.RoughlyEquals(body.GetTarget(), newPosition, collisionSize))
                    return false;
            return true;
        }
    }
}
