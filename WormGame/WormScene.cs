using Otter;
using WormGame.Other;
using WormGame.GameObject;

namespace WormGame
{
    /// @author Antti Harju
    /// @version 21.6.2020
    /// <summary>
    /// Main game
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

        private void EntitySetup(Game game)
        {
            SpawnWorm(0, 0, 5);
            SpawnWorm(0, height - 1, 5);
            SpawnWorm(width - 1, height - 1, 5);
            SpawnWorm(width - 1, 0, 5);
            for (int x = 0; x < width; x += 2)
            {
                SpawnWorm(x, 15, 5);
            }
            SpawnPlayer(game.HalfWidth, game.HalfHeight, Color.Red);
        }

        public WormScene(Game game)
        {
            collision = new Collision(game, width, height, marginMinimum);
            bunches = new Pooler<Bunch>(this, wormCount * 2, collision.Size);
            blocks = new Pooler<Block>(this, wormCount * 2 * maxWormLength, collision.Size);
            tails = new Pooler<Tail>(this, wormCount * maxWormLength, collision.Size);
            worms = new Pooler<Worm>(this, wormCount, collision.Size);
            EntitySetup(game);
        }


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
        /// Spawns a worm to the scene
        /// </summary>
        /// <param name="x">Horizontal position</param>
        /// <param name="y">Vertical position</param>
        /// <param name="length">Worms length</param>
        /// <param name="color">Worms color</param>
        /// <returns>The spawned worm</returns>
        public Worm SpawnWorm(int x, int y, int length = -1, Color color = null, string direction = "")
        {
            if (color == null) color = Helper.RandomColor();
            if (length == -1) length = maxWormLength;
            Worm worm = worms.Enable();
            if (worm == null) return null;
            worm.Spawn(tails, collision, x, y, length, color, direction);
            return worm;
        }


        /// <summary>
        /// Spawns player to the scene
        /// </summary>
        /// <param name="color">Players color</param>
        /// <returns>Spawned player</returns>
        public Player SpawnPlayer(float x, float y, Color color)
        {
            Player tmpPlayer = new Player(this, 0, x, y, color, collision.Size);
            Add(tmpPlayer);
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
        /// Worm movement is updated here
        /// </summary>
        public void WormUpdate()
        {
            foreach (Worm worm in worms)
                if (worm.Enabled)
                    worm.Move();
            //collision.Visualize();
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
