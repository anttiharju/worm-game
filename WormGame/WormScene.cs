using Otter.Core;
using Otter.Graphics;
using Otter.Utility.MonoGame;
using WormGame.Core;
using WormGame.Static;
using WormGame.Pooling;
using WormGame.GameObject;

namespace WormGame
{
    /// @author Antti Harju
    /// @version 01.07.2020
    /// <summary>
    /// The main scene for WormGame
    /// </summary>
    class WormScene : Scene
    {
        private readonly Config config;
        private readonly Collision field;

        private readonly Pool<Worm> worms;
        private readonly Pool<WormEntity> bodies;
        private readonly Pool<BrickBrain> brickBrains;
        private readonly Pool<Brick> bricks;

        private float wormCounter = 0;

        /// <summary>
        /// Initializes pools and collision system. Spawns initial entities.
        /// </summary>
        /// <param name="game"></param>
        public WormScene(Config config)
        {
            this.config = config;
            field = config.field;

            brickBrains = new Pool<BrickBrain>(config, config.brainAmount);
            bricks = new Pool<Brick>(config, config.bodyAmount);
            AddMultiple(bricks.GetPool());
            bodies = new Pool<WormEntity>(config, config.bodyAmount);
            AddMultiple(bodies.GetPool());
            worms = new Pool<Worm>(config, config.brainAmount);

            // Entity setup
            int density = config.density;
            for (int x = 0; x < config.width; x += density)
                for (int y = 0; y < config.height; y += density)
                    SpawnWorm(x, y);
            SpawnPlayer(config.windowWidth / 2, config.windowHeight / 2, Color.Red);
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
        public Worm SpawnWorm(int x, int y, int length = -1, Color color = null)
        {
            if (length == -1) length = config.maxWormLength;
            if (color == null) color = Random.Color();
            Worm worm = worms.Enable();
            if (worm == null) return null;
            worm.Spawn(bodies, field, field.EntityX(x), field.EntityY(y), length, color);
            return worm;
        }

        /// <summary>
        /// Turns the given worm into a collection of bricks
        /// </summary>
        /// <param name="worm">Worm to transform</param>
        public BrickBrain SpawnBrick(Worm worm)
        {
            BrickBrain brick = brickBrains.Enable();
            if (brick == null) return null;
            brick.Spawn(bricks, field, worm);
            return brick;
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
            Player tmpPlayer = new Player(this, 0, x, y, color, config.size);
            Add(tmpPlayer);
            return tmpPlayer;
        }


        /// <summary>
        /// Makes every worm move to their next position
        /// </summary>
        public override void Update()
        {
            wormCounter += config.wormStep;
            foreach (WormEntity body in bodies)
                if (body.Enabled)
                    body.Step();

            if (wormCounter >= config.size)
            {
                foreach (Worm worm in worms)
                    if (worm.Enabled)
                        worm.Move();
                wormCounter = 0;
            }
#if DEBUG
            if (config.visualizeCollision)
                field.Visualize();
#endif
        }
    }
}
