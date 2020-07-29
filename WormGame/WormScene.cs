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
    /// @version 28.07.2020
    /// <summary>
    /// Main scene for Worm Blocks.
    /// </summary>
    public class WormScene : Scene
    {
        public int wormAmount;
        public int wormCap;

        private readonly Config config;
        private readonly Collision collision;
        private readonly Pooler<Worm> worms;
        private readonly Pooler<WormWarning> wormWarnings;
        private readonly Pooler<WormModule> wormModules;
        private readonly Pooler<Fruit> fruits;
        private readonly Pooler<Block> blocks;
        private readonly Pooler<BlockModule> blockModules;
        private readonly float stepAccuracy;

        private float wormFrequency;


        /// <summary>
        /// Initializes poolers and scene entities.
        /// </summary>
        /// <param name="config">Configuration</param>
        public WormScene(Config config)
        {
            this.config = config;
            collision = config.collision;
            stepAccuracy = config.step / 2;
            wormCap = config.wormCap;
            CreateBorders();

            // Poolers
            fruits = new Pooler<Fruit>(config, config.fruitAmount, this);
            worms = new Pooler<Worm>(config, config.wormAmount, this);
            blocks = new Pooler<Block>(config, config.moduleAmount, this);
            if (config.wormSpawnDuration > 0) wormWarnings = new Pooler<WormWarning>(config, config.wormAmount, this);
            wormModules = new Pooler<WormModule>(config, config.moduleAmount); // In theory there can be a worm that fills up the whole field.
            blockModules = new Pooler<BlockModule>(config, config.moduleAmount);

            // Entity setup
            if (config.fruits)
                for (int i = 0; i < fruits.Count; i++)
                    fruits.Enable().Spawn();

            for (int i = 0; i < 5; i++)
                SpawnPlayer(i);

            wormFrequency = config.size - config.step;
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
        /// Makes the world go round.
        /// </summary>
        public override void Update()
        {
            wormFrequency += config.step;
            if (Mathf.FastRound(wormFrequency, stepAccuracy) >= config.size)
            {
                foreach (Worm worm in worms)
                    if (worm.Enabled)
                        worm.Move();
                wormFrequency = 0;
                if (wormAmount < wormCap)
                {
                    Vector2 random = Random.ValidPosition(collision, config.width, config.height, 4);
                    if (random.X != -1)
                        SpawnWorm(random);
                    else
                    {
                        random = Random.ValidPosition(collision, config.width, config.height, 3);
                        if (random.X != -1 && collision.Check(random) == 3)
                        {
                            Fruit fruit = (Fruit)collision.Get(random);
                            fruit.Disable();
                            SpawnWorm(random);
                        }
                        else
                        {
                            wormAmount--;
                        }
                    }
                }
#if DEBUG
                if (config.visualizeCollision)
                    collision.VisualizeCollision();
                if (config.visualizeBlocks)
                    collision.VisualizeBlocks();
#endif
            }
        }


        /// <summary>
        /// Spawn either worm or warning depending on configuration.
        /// </summary>
        /// <param name="position">Spawn position</param>
        private void SpawnWorm(Vector2 position)
        {
            if (Mathf.FastRound(config.wormSpawnDuration) <= 0)
                SpawnWorm(collision.X(position.X), collision.Y(position.Y));
            else
                SpawnWormWarning(collision.X(position.X), collision.Y(position.Y));
            wormAmount++;
        }


        /// <summary>
        /// Spawns a worm.
        /// </summary>
        /// <param name="x">Horizontal field position</param>
        /// <param name="y">Vertical field position</param>
        /// <param name="length">Length, default config.minWormLength</param>
        /// <param name="color">Color, default Random.Color</param>
        public void SpawnWorm(int x, int y, int length = 0, Color color = null)
        {
            Worm worm = worms.Enable();
            if (worm == null) return;
            if (color == null) color = Random.Color;
            if (length < config.minWormLength) length = config.minWormLength;
            worm.Spawn(wormModules, x, y, length, color);
        }


        /// <summary>
        /// Spawns worm spawn warning.
        /// </summary>
        /// <param name="x">Horizontal field position</param>
        /// <param name="y">Vertical field position</param>
        /// <param name="length">Length, default config.minWormLength</param>
        /// <param name="color">Color, default Random.Color</param>
        public void SpawnWormWarning(int x, int y, int length = 0, Color color = null)
        {
            WormWarning warning = wormWarnings.Enable();
            if (warning == null) return;
            if (color == null) color = Random.Color;
            if (length < config.minWormLength) length = config.minWormLength;
            warning.Spawn(x, y, length, color);
        }


        /// <summary>
        /// Turn a worm into a block.
        /// </summary>
        /// <param name="worm">Worm to transform</param>
        /// <returns>Block or null</returns>
        public Block SpawnBlock(Worm worm)
        {
            Block block = blocks.Enable();
            if (block == null || blockModules.HasAvailable(worm.Length) == false)
                return null;
            block = block.Spawn(worm, blockModules);
            wormAmount--;
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
        /// Creates a visible border for the field.
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
