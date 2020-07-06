using WormGame.Help;
using WormGame.Other;
using WormGame.GameObject;
using Otter.Utility.MonoGame;
using Otter.Graphics;
using Otter.Core;

namespace WormGame
{
    /// @author Antti Harju
    /// @version 01.07.2020
    /// <summary>
    /// The main scene for WormGame
    /// </summary>
    class WormScene : Scene
    {
        private readonly Collision field;

        private readonly Pooler<Worm> worms;
        private readonly Pooler<Brick> bricks;
        private readonly Pooler<WormBase> baseworms;
        private readonly Pooler<BrickBase> basebricks;

        private readonly float bunchTimerReset = 0.6f;
        private readonly float wormTimerReset = 0.1f;
        private float bunchTimer = 0;
        private float wormTimer = 0;

        /// <summary>
        /// Initializes poolers and collision system. Spawns initial entities.
        /// </summary>
        /// <param name="game"></param>
        public WormScene(Game game, Config config)
        {
            field = new Collision(game, Config.width, Config.height, Config.margin);
            bricks = new Pooler<Brick>(this, config.wormAmount, field.Size);
            basebricks = new Pooler<BrickBase>(this, config.tailAmount, field.Size);
            baseworms = new Pooler<WormBase>(this, config.tailAmount, field.Size);
            worms = new Pooler<Worm>(this, config.wormAmount, field.Size);

            // Entity setup
            SpawnWorm(8, 8);
            int density = 4;
            for (int x = 0; x < Config.width; x += density)
                for (int y = 0; y < Config.height; y += density)
                    SpawnWorm(x, y);
            SpawnPlayer(game.HalfWidth, game.HalfHeight, Color.Red);
        }


        /// <summary>
        /// Finds the nearest worm to the given position, within given range.
        /// Used by player class to posess worms.
        /// </summary>
        /// <param name="position">Search point</param>
        /// <param name="range">Maximum distance from search point to worm</param>
        /// <returns>Nearest worm</returns>
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
        /// <param name="x">Horizontal position (playArea)</param>
        /// <param name="y">Vertical postition (playArea)</param>
        /// <param name="length">Worms length</param>
        /// <param name="color">Worms color, by default uses a random color from Helper class</param>
        /// <param name="direction">Worms direction: UP, LEFT, DOWN or RIGHT</param>
        /// <returns>Spawned worm</returns>
        public Worm SpawnWorm(int x, int y, int length = -1, int direction = -1, Color color = null)
        {
            if (length == -1) length = Config.maxWormLength;
            if (direction == -1) direction = Random.Range(0, 4);
            if (color == null) color = Random.Color();
            Worm worm = worms.Enable();
            if (worm == null) return null;
            worm.Spawn(baseworms, field, field.EntityX(x), field.EntityY(y), length, color, direction);
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
            Player tmpPlayer = new Player(this, 0, x, y, color, field.Size);
            Add(tmpPlayer);
            return tmpPlayer;
        }


        /// <summary>
        /// Turns the given worm into a collection of bricks
        /// </summary>
        /// <param name="worm">Worm to transform</param>
        public Brick SpawnBrick(Worm worm)
        {
            Brick brick = bricks.Enable();
            if (brick == null) return null;
            brick.Spawn(basebricks, field, worm);
            return brick;
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
                //BrickUpdate();
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
            if (Config.visualizeCollision)
                field.Visualize();
        }
    }
}
