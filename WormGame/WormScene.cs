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
    /// @version 08.07.2020
    /// <summary>
    /// The main scene for Worm Bricks
    /// </summary>
    public class WormScene : Scene
    {
        private readonly Config config;
        private readonly Collision field;

        private readonly Pool<Worm> worms;
        private readonly Pool<Brick> bricks;
        private readonly Pool<BrickEntity> brickEntities;

        private float wormCounter = 0;

        /// <summary>
        /// Initializes pools and collision system. Spawns initial entities.
        /// </summary>
        /// <param name="config">Configuration</param>
        public WormScene(Config config)
        {
            this.config = config;
            field = config.field;

            worms = new Pool<Worm>(config, config.brainAmount);
            bricks = new Pool<Brick>(config, config.brainAmount);
            brickEntities = new Pool<BrickEntity>(config, config.bodyAmount);

            AddMultiple(worms.GetPool());
            AddMultiple(brickEntities.GetPool());

            // Entity setup
            int density = config.density;
            if (density > 0)
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
                    float distance = Vector2.Distance(position, worm.GetTarget(worm.Length / 2));
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
        /// <param name="x">Horizontal field position</param>
        /// <param name="y">Vertical field postition</param>
        /// <param name="length">Worm length</param>
        /// <param name="stationary">Should the worm start moving after spawning</param>
        /// <param name="color">Worm color, by default random</param>
        /// <returns>Spawned worm</returns>
        public Worm SpawnWorm(int x, int y, int length = 0, Color color = null)
        {
            if (length == 0) length = config.maxWormLength;
            if (color == null) color = Random.Color();
            Worm worm = worms.Enable();
            if (worm == null) return null;
            worm.Spawn(x, y, length, color, config.width);
            return worm;
        }


        /// <summary>
        /// Turns the given worm into a collection of bricks.
        /// </summary>
        /// <param name="worm">Worm to transform</param>
        public Brick SpawnBrick(Worm worm)
        {
            Brick brick = bricks.Enable();
            if (brick == null) return null;
            brick.Spawn(this, field, brickEntities, worm);
            return brick;
        }


        /// <summary>
        /// Spawn a player.
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
        /// Keeps the scene going.
        /// </summary>
        public override void Update()
        {
            wormCounter += config.step;

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
