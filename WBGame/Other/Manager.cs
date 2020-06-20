using Otter;
using System;
using WBGame.GameObject;
using WBGame.Pooling;

namespace WBGame.Other
{
    class Manager
    {
        private readonly int collisionSize;
        private readonly Scene scene;
        private readonly Pooler<Worm> wormPool;
        private readonly Pooler<Tail> tailPool;
        private readonly Pooler<Block> blockPool;


        public Manager(Scene scene, int bodyCount, int headCount, int blockCount, int size)
        {
            this.scene = scene;
            tailPool = new Pooler<Tail>(scene, bodyCount, size);
            wormPool = new Pooler<Worm>(scene, headCount, size);
            blockPool = new Pooler<Block>(scene, blockCount, size);
            collisionSize = (int)(0.9f * size);
        }

        public Worm SpawnWorm(int x, int y, int length, Color color)
        {
            Worm worm = wormPool.Next().Spawn(this, x, y, length, Color.Red);

            int bodyCount = length - 1; // - 1 because head is already counts as 1
            Tail currentBody = worm;
            for (int i = 0; i < bodyCount; i++)
            {
                Tail tmpBody = tailPool.Next();
                tmpBody.Enable();
                tmpBody.Position = new Vector2(x, y);
                tmpBody.SetTarget(x, y);
                tmpBody.Color = color;
                currentBody.SetNextBody(tmpBody);
                currentBody = tmpBody;
            }
            return worm;
        }


        public Player SpawnPlayer(Worm worm)
        {
            Player tmpPlayer = new Player(this);
            scene.Add(tmpPlayer);
            return tmpPlayer;
        }


        public void Blockify(Worm worm)
        {
            if (worm == null) return;
            Color color = worm.Color;
            Vector2[] blockPositions = worm.GetPositions(new Vector2[worm.GetLength()]);
            for (int i = 0; i < blockPositions.Length; i++)
            {
                Block tmpBlock = blockPool.Next();
                if (tmpBlock == null) break;
                tmpBlock.Spawn(blockPositions[i], color);
            }
        }


        public Worm Posess(Worm posessed)
        {
            foreach (Worm worm in wormPool)
                if (worm.Enabled && worm != posessed)
                    return worm;
            return null;
        }


        public void Move()
        {
            foreach (Worm worm in wormPool)
                if (worm.Enabled)
                    worm.EatQueue();
        }


        public bool CanMove(Worm asker, Vector2 newPosition)
        {
            foreach (Worm worm in wormPool)
                if (worm.Enabled && Math.RoughlyEquals(worm.GetTarget(), newPosition, collisionSize) && worm != asker)
                    return false;
            foreach (Tail body in tailPool)
                if (body.Enabled && Math.RoughlyEquals(body.GetTarget(), newPosition, collisionSize))
                    return false;
            return true;
        }
    }
}
