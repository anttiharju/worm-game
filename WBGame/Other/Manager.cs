using Otter;
using WBGame.GameObject;

namespace WBGame.Other
{
    /// <summary>
    /// Manager. Manages pooled entities and interactions between them.
    /// </summary>
    class Manager
    {
        private readonly int collisionSize;
        private readonly int size;
        private readonly Scene scene;
        private readonly Pooler<Worm> worms;
        private readonly Pooler<Tail> tails;
        private readonly Pooler<Bunch> bunches;
        private readonly Pooler<Block> blocks;
        private readonly int width;
        private readonly int height;

        public Worm NearestWorm(Vector2 player, float range)
        {
            Worm nearestWorm = null;
            float nearestDistance = range;
            foreach (Worm worm in worms)
                if (worm.Enabled)
                {
                    float distance = Vector2.Distance(player, worm.Position);
                    if (distance < nearestDistance)
                    {
                        nearestWorm = worm;
                        nearestDistance = distance;
                    }
                }
            return nearestWorm;
        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="scene">Scene to manage</param>
        /// <param name="tailCount">How many bodies to pool</param>
        /// <param name="wormCount">How many heads to pool</param>
        /// <param name="bunchCount">How many blocks to pool</param>
        /// <param name="size">How big should the pooled things be</param>
        public Manager(Scene scene, int wormCount, int maxWormLength, int bunchCount, int size, int width, int height)
        {
            this.scene = scene;
            worms = new Pooler<Worm>(scene, 2, size);
            tails = new Pooler<Tail>(scene, 160 * 90, size);
            bunches = new Pooler<Bunch>(scene, bunchCount, size);
            blocks = new Pooler<Block>(scene, bunchCount * maxWormLength, size);
            collisionSize = (int)(0.9f * size);
            this.size = size;
            this.width = width;
            this.height = height;
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
            worm.Spawn(this, x, y, length, color);

            int bodyCount = length - 1; // - 1 because head already counts as 1
            Tail currentBody = worm;
            for (int i = 0; i < bodyCount; i++)
            {
                Tail tmpBody = tails.Enable();
                if (tmpBody == null) return null;
                tmpBody.Position = new Vector2(x, y);
                tmpBody.Target = tmpBody.Position;
                tmpBody.Graphic.Color = color;
                currentBody.NextBody = tmpBody;
                currentBody = tmpBody;
            }
            return worm;
        }


        /// <summary>
        /// Spawns player to the scene
        /// </summary>
        /// <param name="color">Players color</param>
        /// <returns>Spawned player</returns>
        public Player SpawnPlayer(float x, float y, Color color)
        {
            Player tmpPlayer = new Player(this, 0, x, y, color, size);
            scene.Add(tmpPlayer);
            return tmpPlayer;
        }


        /// <summary>
        /// Turns a worm to blocks
        /// </summary>
        /// <param name="worm">Worm to blockify</param>
        public Bunch Blockify(Worm worm)
        {
            Vector2[] positions = worm.GetPositions(new Vector2[worm.Length]);

            Bunch bunch = bunches.Enable();
            if (bunch == null) return null;
            bunch.Spawn(positions[0], Color.Gray, worm.Length, 712);

            Block tmpBlock = bunch;
            Block previousBlock = tmpBlock;

            for (int i = 1; i < positions.Length; i++)
            {
                tmpBlock = blocks.Enable();
                previousBlock.NextBlock = tmpBlock;
                tmpBlock.Spawn(positions[i], Color.Gray);
                previousBlock = tmpBlock;
            }

            return bunch;
        }


        /// <summary>
        /// Applies gravity to bunches
        /// </summary>
        public void BunchUpdate()
        {
            // TODO: If player is doing softdrop, don't apply gravity
            foreach (Bunch bunch in bunches)
                if (bunch.Enabled) { }
            // bunch.SoftDrop();
        }


        /// <summary>
        /// Worm movement is updated here
        /// </summary>
        public void WormUpdate()
        {
            foreach (Worm worm in worms)
                if (worm.Enabled)
                    worm.Move();
        }


        /// <summary>
        /// Collision.
        /// </summary>
        /// <param name="newPosition">Where the worm wants to move</param>
        /// <returns>If it can or not move to the new position</returns>
        public bool WormCollision(Vector2 newPosition)
        {
            if (newPosition.Y < scene.Game.WindowHeight / 2 - size * (height / 2) ||
                newPosition.Y > scene.Game.WindowHeight / 2 + size * (height / 2) ||
                newPosition.X < scene.Game.WindowWidth / 2 - size * (width / 2) ||
                newPosition.X > scene.Game.WindowWidth / 2 + size * (width / 2))
                return false;/*
            foreach (Worm worm in worms)
                if (worm.Enabled)
                    if (Helper.RoughlyEquals(worm.Target, newPosition, collisionSize))
                        return false;
            foreach (Tail tail in tails)
                if (tail.Enabled)
                    if (Helper.RoughlyEquals(tail.Target, newPosition, collisionSize))
                        return false;
            foreach (Block block in blocks)
                if (block.Enabled)
                    if (Helper.RoughlyEquals(block.Position, newPosition, collisionSize))
                        return false;*/
            return true;
        }
    }
}
