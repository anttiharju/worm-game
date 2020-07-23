using Otter.Core;
using Otter.Graphics;
using Otter.Utility.MonoGame;
using Otter.Graphics.Drawables;
using WormGame.Core;
using WormGame.Static;
using WormGame.Pooling;
using WormGame.GameObject;

namespace WormGame
{
    /// @author Antti Harju
    /// @version 18.07.2020
    /// <summary>
    /// Main scene for Worm Bricks.
    /// </summary>
    public class WormScene : Scene
    {
        private readonly Config config;
        private readonly Collision field;
        private readonly Pooler<Worm> worms;
        private readonly Pooler<Fruit> fruits;
        private readonly Pooler<Block> blocks;
        private readonly Pooler<WormModule> wormModules;
        private readonly Pooler<BlockModule> blockModules;

        private float wormCounter;

        public int wormCount;
        public int maxWormCount;


        /// <summary>
        /// Initializes poolers and scene entities.
        /// </summary>
        /// <param name="config"></param>
        public WormScene(Config config)
        {
            this.config = config;
            field = config.field;
            maxWormCount = config.maxWormAmount;
            CreateBorders();

            // Entity pools
            blocks = new Pooler<Block>(config, config.moduleAmount);
            fruits = new Pooler<Fruit>(config, config.fruitAmount);
            worms = new Pooler<Worm>(config, config.wormAmount);
            AddMultiple(blocks.Pool);
            AddMultiple(fruits.Pool);
            AddMultiple(worms.Pool);

            // Object pools
            wormModules = new Pooler<WormModule>(config, config.moduleAmount);
            blockModules = new Pooler<BlockModule>(config, config.moduleAmount);

            // Entity setup
            if (config.fruits)
                for (int i = 0; i < fruits.Count; i++)
                    fruits.Enable().Spawn();

            for (int i = 0; i < 3; i++)
                SpawnPlayer(i);
        }


        /// <summary>
        /// Finds the nearest worm. Used by player to posess worms.
        /// </summary>
        /// <param name="position">Player position</param>
        /// <param name="range">Maximum distance from position to worm</param>
        /// <returns>Worm or null</returns>
        public Worm NearestWorm(Vector2 position, float range)
        {
            Worm nearestWorm = null;
            float nearestDistance = range;
            foreach (Worm worm in worms)
                if (worm.Enabled)
                {
                    float distance = Vector2.Distance(position, worm.firstModule.Target);
                    if (distance < nearestDistance)
                    {
                        nearestWorm = worm;
                        nearestDistance = distance;
                    }
                }
            return nearestWorm;
        }


        /// <summary>
        /// Spawns a worm.
        /// </summary>
        /// <param name="x">Horizontal field position</param>
        /// <param name="y">Vertical field position</param>
        /// <param name="length">Length, default config.minWormLength</param>
        /// <param name="color">Color, default Random.Color</param>
        /// <returns>Worm</returns>
        public Worm SpawnWorm(int x, int y, int length = 0, Color color = null)
        {
            Worm worm = worms.Enable();
            if (worm == null) return null;
            if (color == null) color = Random.Color;
            if (length < config.minWormLength) length = config.minWormLength;
            worm.Spawn(wormModules, x, y, length, color);
            return worm;
        }


        /// <summary>
        /// Turn a worm into a block.
        /// </summary>
        /// <param name="worm">Worm to transform</param>
        /// <returns>Block</returns>
        public Block SpawnBlock(Worm worm, int currentLength)
        {
            Block block = blocks.Enable();
            if (block == null || blockModules.HasAvailable(currentLength) == false)
                return null;
            block = block.Spawn(worm, blockModules, currentLength);
            wormCount--;
            return block;
        }


        /// <summary>
        /// Spawn a player.
        /// </summary>
        /// <param name="playerNumber">Player number</param>
        /// <returns>Player</returns>
        public Player SpawnPlayer(int playerNumber)
        {
            Player player = new Player(this, playerNumber, config.windowWidth / 2, config.windowHeight / 2, config.size);
            Add(player);
            return player;
        }


        /// <summary>
        /// Makes calls to scenes entities to keep them moving.
        /// </summary>
        public override void Update()
        {
            wormCounter += config.step;
            if (Mathf.FastRound(wormCounter) >= config.size)
            {
                foreach (Worm worm in worms)
                    if (worm.Enabled)
                        worm.Move();
                wormCounter = 0;
                if (wormCount < maxWormCount)
                {
                    Vector2 random = Random.ValidPosition(field, config.width, config.height, 4);
                    if (random.X != -1 && field.Get(random) == 4)
                    {
                        SpawnWorm(field.X(random.X), field.Y(random.Y));
                        wormCount++;
                    }
                    else
                        wormCount--;
                }
#if DEBUG
                if (config.visualizeCollision)
                    field.Visualize();
#endif
            }
        }


        /// <summary>
        /// Creates visible borders for the field.
        /// </summary>
        private void CreateBorders()
        {
            Image backgroundGraphic = Image.CreateRectangle(config.width * config.size, config.height * config.size, Color.Black);
            backgroundGraphic.CenterOrigin();
            backgroundGraphic.OutlineColor = Color.White;
            backgroundGraphic.OutlineThickness = config.size / 6;
            Entity background = new Entity(config.windowWidth / 2, config.windowHeight / 2, backgroundGraphic)
            {
                Collidable = false
            };
            Add(background);
        }
    }
}
