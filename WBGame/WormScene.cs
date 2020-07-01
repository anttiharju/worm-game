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
        private readonly int wormCount = 59;
        private readonly int maxWormLength = 3;

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
            int y = 0;
            SpawnWorm(20, 10, Helper.RandomColor(), 5, "", true);
            for (int x = 0; x < 40; x++)
            {
                SpawnWorm(x, y, Helper.RandomColor(), 5, "", false);
                if (x % 3 == 0) y++;
            }
            SpawnPlayer(game.HalfWidth, game.HalfHeight, Color.Red);
        }

        public WormScene(Game game)
        {
            collision = new Collision(game, 40, 20, 2);
            worms = new Pooler<Worm>(this, wormCount, collision.Size);
            tails = new Pooler<Tail>(this, wormCount * maxWormLength, collision.Size);
            bunches = new Pooler<Bunch>(this, wormCount * 2, collision.Size);
            blocks = new Pooler<Block>(this, wormCount * 2 * maxWormLength, collision.Size);
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
        /// <param name="gridX">Horizontal position</param>
        /// <param name="gridY">Vertical position</param>
        /// <param name="length">Worms length</param>
        /// <param name="color">Worms color</param>
        /// <returns>The spawned worm</returns>
        public Worm SpawnWorm(int gridX, int gridY, Color color, int length, string direction = "", bool noclip = false)
        {
            Worm worm = worms.Enable();
            if (worm == null) return null;
            worm.Spawn(tails, collision, gridX, gridY, length, color, direction, noclip);
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
            collision.VisualizeField();
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
