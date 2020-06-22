using Otter;
using WBGame.GameObject;
using WBGame.Pooling;

namespace WBGame.Other
{
    /// <summary>
    /// Manager. Manages all kinds of things, mainly Poolers. Currently kind of bloated.
    /// </summary>
    class Manager
    {
        private readonly int collisionSize;
        private readonly Scene scene;
        private readonly Pooler<Worm> wormPool;
        private readonly Pooler<Tail> tailPool;
        private readonly Pooler<Block> blockPool;
        private int lastPosessed = 0;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="scene">Scene to manage</param>
        /// <param name="bodyCount">How many bodies to pool</param>
        /// <param name="headCount">How many heads to pool</param>
        /// <param name="blockCount">How many blocks to pool</param>
        /// <param name="size">How big should the pooled things be</param>
        public Manager(Scene scene, int bodyCount, int headCount, int blockCount, int size)
        {
            this.scene = scene;
            tailPool = new Pooler<Tail>(scene, bodyCount, size);
            wormPool = new Pooler<Worm>(scene, headCount, size);
            blockPool = new Pooler<Block>(scene, blockCount, size);
            collisionSize = (int)(0.9f * size);
        }


        /// <summary>
        /// Spawns a worm to the scene
        /// </summary>
        /// <param name="x">Horizontal position</param>
        /// <param name="y">Vertical position</param>
        /// <param name="length">Worms length</param>
        /// <param name="color">Worms color</param>
        /// <param name="directions">Movement instructions for the worm to follow</param>
        /// <returns>The spawned worm</returns>
        public Worm SpawnWorm(int x, int y, int length, Color color, char[] directions = null)
        {
            Worm worm = wormPool.Next();
            if (worm == null) return null;
            worm.Spawn(this, x, y, length, color, directions);

            int bodyCount = length - 1; // - 1 because head already counts as 1
            Tail currentBody = worm;
            for (int i = 0; i < bodyCount; i++)
            {
                Tail tmpBody = tailPool.Next();
                if (tmpBody == null) return null;
                tmpBody.Enable();
                tmpBody.Position = new Vector2(x, y);
                tmpBody.SetTarget(x, y);
                tmpBody.Color = color;
                currentBody.SetNextBody(tmpBody);
                currentBody = tmpBody;
            }
            return worm;
        }


        /// <summary>
        /// Spawns player to the scene
        /// </summary>
        /// <param name="color">Players color</param>
        /// <returns>Spawned player</returns>
        public Player SpawnPlayer(Color color)
        {
            Player tmpPlayer = new Player(this, color);
            scene.Add(tmpPlayer);
            return tmpPlayer;
        }


        /// <summary>
        /// Turns a worm to blocks
        /// </summary>
        /// <param name="worm">Worm to blockify</param>
        public void Blockify(Worm worm)
        {
            if (worm == null) return;
            Vector2[] blockPositions = worm.GetPositions(new Vector2[worm.GetLength()]);
            Block previousBlock = null;
            for (int i = 0; i < blockPositions.Length; i++)
            {
                Block tmpBlock = blockPool.Next();
                if (tmpBlock == null) break;
                if (previousBlock != null)
                    previousBlock.NextBlock = tmpBlock;
                tmpBlock.Spawn(blockPositions[i], Color.Gray);
                previousBlock = tmpBlock;
            }
        }


        /// <summary>
        /// Returns next unposessed worm. Also keeps track of what worm was last posessed so we cycle through them all.
        /// </summary>
        /// <param name="posessed">Currently posessed worm.</param>
        /// <returns>Next unposessed worm, null if no unposessed worm was found</returns>
        /// TODO: Posessed not needed?
        public Worm Posess(Worm posessed)
        {
            bool firstRound = true;
            for (int i = lastPosessed; i < wormPool.Length; i++)
            {
                Worm worm = wormPool[i];
                if (worm.Enabled && worm != posessed)
                {
                    lastPosessed = i;
                    return worm;
                }
                if (i == wormPool.Length - 1 && firstRound)
                {
                    firstRound = false;
                    i = -1; // -1 because i++ makes it 0
                }
            }
            return null;
        }

        /// <summary>
        /// W.I.P. Separate from WormUpdate because we want worms to move faster than bricks fall
        /// </summary>
        public void BlockUpdate()
        {
        }


        /// <summary>
        /// Worm movement is updated here
        /// </summary>
        public void WormUpdate()
        {
            SpawnWorm(640, 360, 5, Helper.RandomColor(), Helper.GenerateDirections(100));
            foreach (Worm worm in wormPool)
                if (worm.Enabled)
                    worm.Move();
        }


        /// <summary>
        /// Collision. Currently commented out because movement queuing (controls.cs) doesn't work properly with this enabled.
        /// </summary>
        /// <param name="asker">Worm that wants to move</param>
        /// <param name="newPosition">Where the worm wants to move</param>
        /// <returns>If it can or not move to the new position</returns>
        public bool CanMove(Worm asker, Vector2 newPosition)
        {   
            /** /
            foreach (Worm worm in wormPool)
                if (worm.Enabled && Helper.RoughlyEquals(worm.GetTarget(), newPosition, collisionSize) && worm != asker)
                    return false;
            foreach (Tail tail in tailPool)
                if (tail.Enabled && Helper.RoughlyEquals(tail.GetTarget(), newPosition, collisionSize))
                    return false;
            foreach (Block block in blockPool)
                if (block.Enabled && Helper.RoughlyEquals(block.Position, newPosition, collisionSize))
                    return false;
            /**/
            return true;
        }
    }
}
