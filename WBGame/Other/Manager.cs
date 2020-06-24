using Otter;
using WBGame.GameObject;

namespace WBGame.Other
{
    /// <summary>
    /// Manager. Manages all kinds of things, mainly Poolers. Currently kind of bloated.
    /// </summary>
    class Manager
    {
        private readonly int collisionSize;
        private readonly Scene scene;
        private readonly Pooler<Worm> worms;
        private readonly Pooler<Tail> tails;
        private readonly Pooler<Block> blocks;
        private int lastPosessed = 0;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="scene">Scene to manage</param>
        /// <param name="tailCount">How many bodies to pool</param>
        /// <param name="wormCount">How many heads to pool</param>
        /// <param name="blockCount">How many blocks to pool</param>
        /// <param name="size">How big should the pooled things be</param>
        public Manager(Scene scene, int wormCount, int maxWormLength, int blockCount, int size)
        {
            this.scene = scene;
            worms = new Pooler<Worm>(scene, wormCount, size);
            tails = new Pooler<Tail>(scene, wormCount * maxWormLength, size);
            blocks = new Pooler<Block>(scene, blockCount, size);
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
            Worm worm = worms.Enable();
            if (worm == null) return null;
            worm.Spawn(this, x, y, length, color, directions);

            int bodyCount = length - 1; // - 1 because head already counts as 1
            Tail currentBody = worm;
            for (int i = 0; i < bodyCount; i++)
            {
                Tail tmpBody = tails.Enable();
                if (tmpBody == null) return null;
                tmpBody.Position = new Vector2(x, y);
                tmpBody.SetTarget(x, y);
                tmpBody.Graphic.Color = color;
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
            Vector2[] blockPositions = worm.GetPositions(new Vector2[worm.Length]);
            Block previousBlock = null;
            for (int i = 0; i < blockPositions.Length; i++)
            {
                Block tmpBlock = blocks.Enable();
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
        /// <returns>Next unposessed worm, null if no unposessed worm was found</returns>
        public Worm Posess()
        {
            lastPosessed++;
            if (lastPosessed == worms.Count) lastPosessed = 0;
            if (worms.Count < 1) return null;
            return worms[lastPosessed];
        }


        /// <summary>
        /// W.I.P. Separate from WormUpdate because we want worms to move faster than bricks fall
        /// </summary>
        public void BlockUpdate()
        {
        }


        /// <summary>
        /// Could be done in tail update override but we hate if's right?
        /// Premature optimization is the root of all evil?
        /// </summary>
        public void Update()
        {
            foreach (Worm worm in worms)
                worm.Tween();
            foreach (Tail tail in tails)
                tail.Tween();
        }


        /// <summary>
        /// Worm movement is updated here
        /// </summary>
        public void WormUpdate()
        {
            foreach (Worm worm in worms)
                worm.Move();
        }


        /// <summary>
        /// Collision.
        /// </summary>
        /// <param name="newPosition">Where the worm wants to move</param>
        /// <returns>If it can or not move to the new position</returns>
        public bool CanMove(Vector2 newPosition)
        {
            foreach (Worm worm in worms)
                if (Helper.RoughlyEquals(worm.GetTarget(), newPosition, collisionSize))
                    return false;
            foreach (Tail tail in tails)
                if (Helper.RoughlyEquals(tail.GetTarget(), newPosition, collisionSize))
                    return false;
            foreach (Block block in blocks)
                if (Helper.RoughlyEquals(block.Position, newPosition, collisionSize))
                    return false;
            return true;
        }
    }
}
