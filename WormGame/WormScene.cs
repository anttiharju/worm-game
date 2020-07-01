using Otter;
using WormGame.Other;
using WormGame.GameObject;

namespace WormGame
{
    /// @author anvemaha
    /// @version 01.07.2020
    /// <summary>
    /// The main scene for WormGame
    /// </summary>
    class WormScene : Scene
    {
        private readonly int width = 100;
        private readonly int height = 50;
        private readonly int marginMinimum = 2;
        private readonly int wormCount = 100;
        private readonly int maxWormLength = 5;

        private readonly float bunchTimerReset = 0.6f;
        private readonly float wormTimerReset = 0.3f;
        private float bunchTimer = 0;
        private float wormTimer = 0;

        private readonly Pooler<Bunch> bunches;
        private readonly Pooler<Block> blocks;
        private readonly Pooler<Tail> tails;
        private readonly Pooler<Worm> worms;
        private readonly Collision collision;


        /// <summary>
        /// Initializes poolers and collision system. Spawns initial entities.
        /// </summary>
        /// <param name="game"></param>
        public WormScene(Game game)
        {
            collision = new Collision(game, width, height, marginMinimum);
            bunches = new Pooler<Bunch>(this, wormCount * 2, collision.Size);
            blocks = new Pooler<Block>(this, wormCount * 2 * maxWormLength, collision.Size);
            tails = new Pooler<Tail>(this, wormCount * maxWormLength, collision.Size);
            worms = new Pooler<Worm>(this, wormCount, collision.Size);

            // Entity setup
            SpawnWorm(0, 0, 5);
            SpawnWorm(0, height - 1, 5);
            SpawnWorm(width - 1, height - 1, 5);
            SpawnWorm(width - 1, 0, 5);
            for (int x = 0; x < width; x += 2)
                SpawnWorm(x, 15, 5);
            SpawnPlayer(game.HalfWidth, game.HalfHeight, Color.Red);
        }


        /// <summary>
        /// Finds the nearest worm to the given position, within given range.
        /// Used by player class to posess worms.
        /// </summary>
        /// <param name="position">Point that the worm has to be close to</param>
        /// <param name="range">The worm has to be at least this near</param>
        /// <returns></returns>
        public Worm NearestWorm(Vector2 position, float range)
        {
            Worm nearestWorm = null;
            float nearestDistance = range;
            foreach (Worm worm in worms)
                if (worm.Enabled)
                {
                    float distance = Vector2.Distance(position, worm.Position);
                    if (distance < nearestDistance)
                    {
                        nearestWorm = worm;
                        nearestDistance = distance;
                    }
                }
            return nearestWorm;
        }


        /// <summary>
        /// Spawns a worm
        /// </summary>
        /// <param name="x">Horizontal position (field)</param>
        /// <param name="y">Vertical postition (field)</param>
        /// <param name="length">Worms length</param>
        /// <param name="color">Worms color, by default uses a random color from Helper class</param>
        /// <param name="direction">Worms direction: UP, LEFT, DOWN or RIGHT</param>
        /// <returns>Spawned worm</returns>
        public Worm SpawnWorm(int x, int y, int length = -1, Color color = null, string direction = "")
        {
            if (color == null) color = Random.Color();
            if (length == -1) length = maxWormLength;
            Worm worm = worms.Enable();
            if (worm == null) return null;
            worm.Spawn(tails, collision, x, y, length, color, direction);
            return worm;
        }


        /// <summary>
        /// Spawn a player
        /// </summary>
        /// <param name="x">Horizontal position (actual)</param>
        /// <param name="y">Vertical position (actual)</param>
        /// <param name="color">Players color</param>
        /// <returns>Spawned player</returns>
        public Player SpawnPlayer(float x, float y, Color color)
        {
            Player tmpPlayer = new Player(this, 0, x, y, color, collision.Size);
            Add(tmpPlayer);
            return tmpPlayer;
        }


        /// <summary>
        /// Turns the given worm into a bunch of blocks
        /// </summary>
        /// <param name="worm">Worm to transform</param>
        public Bunch Blockify(Worm worm)
        {
            Vector2[] positions = worm.GetPositions(new Vector2[worm.Length]);

            Bunch bunch = bunches.Enable();
            if (bunch == null) return null;
            bunch.Spawn(positions[0], Color.Gray, worm.Length, collision.Y(0));

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
        /// Updates timers that control WormUpdate() and BunchUpdate()
        /// </summary>
        public override void Update()
        {
            base.Update();
            wormTimer += Game.DeltaTime;
            bunchTimer += Game.DeltaTime;
            if (wormTimer >= wormTimerReset)
            {
                wormTimer = 0;
                WormUpdate();
            }
            if (bunchTimer >= bunchTimerReset)
            {
                bunchTimer = 0;
                BunchUpdate();
            }
        }


        /// <summary>
        /// Makes every worm move to their next position
        /// </summary>
        public void WormUpdate()
        {
            foreach (Worm worm in worms)
                if (worm.Enabled)
                    worm.Move();
        }


        /// <summary>
        /// Applies gravity to bunches
        /// </summary>
        public void BunchUpdate()
        {
            /** / // TODO: If player is doing softdrop, don't apply gravity
            foreach (Bunch bunch in bunches)
                if (bunch.Enabled)
                    bunch.SoftDrop();
            /**/
        }
    }
}
