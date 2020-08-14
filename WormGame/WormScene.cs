using Otter.Core;
using Otter.Graphics;
using Otter.Utility.MonoGame;
using Otter.Graphics.Drawables;
using WormGame.Core;
using WormGame.Static;
using WormGame.Pooling;
using WormGame.Entities;

namespace WormGame
{
    /// @author Antti Harju
    /// @version 30.07.2020
    /// <summary>
    /// Main scene for Worm Blocks.
    /// </summary>
    public class WormScene : Scene
    {
        private readonly Config config;
        private readonly Collision collision;
        private readonly Pooler<Player> players;
        private readonly Pooler<Worm> worms;
        private readonly Pooler<Fruit> fruits;
        private readonly Pooler<WormModule> wormModules;
        private readonly BlockManager blocks;
        private readonly float stepAccuracy;
        private readonly int wormCap;

        private float wormFrequency;
        private int wormAmount;

        /// <summary>
        /// Initializes poolers and scene entities.
        /// </summary>
        /// <param name="config">Configuration</param>
        public WormScene(Config config)
        {
            this.config = config;
            wormCap = config.wormCap;
            collision = config.collision;
            stepAccuracy = config.step / 2;
            wormFrequency = config.size - config.step;
            CreateBorders(config.width, config.height);
            worms = new Pooler<Worm>(this, config, config.wormAmount);
            fruits = new Pooler<Fruit>(this, config, config.fruitAmount);
            players = new Pooler<Player>(this, config, 5);
            wormModules = new Pooler<WormModule>(this, config, config.moduleAmount);
            blocks = new BlockManager(this, config, config.moduleAmount);
            Start();
        }


        /// <summary>
        /// Spawns initial entities. Worm spawning is handled in Update().
        /// </summary>
        private void Start()
        {
            if (config.fruits)
                for (int i = 0; i < fruits.Length; i++)
                    SpawnFruit();
            for (int i = 0; i < 5; i++)
                SpawnPlayer(i);
        }


        /// <summary>
        /// Restarts the game by resetting poolers and calling Start().
        /// </summary>
        private void Restart()
        {
            players.Reset();
            worms.Reset();
            fruits.Reset();
            blocks.Reset();
            collision.Reset();
            wormModules.Reset();
            wormFrequency = config.size - config.step;
            wormAmount = 0;
            Start();
#if DEBUG
            System.Console.Clear();
            System.Console.WriteLine("[ Otter is running in debug mode! ]");
#endif
            System.GC.Collect();
        }


        /// <summary>
        /// Finds the nearest worm. Used by player to posess worms.
        /// </summary>
        /// <param name="position">Player position</param>
        /// <param name="range">Maximum distance from position to worm</param>
        /// <returns>Worm or null</returns>
        /// TODO: Optimize using collision.
        public Worm NearestWorm(Vector2 position, float range)
        {
            Worm nearestWorm = null;
            float nearestDistance = range;
            foreach (Worm worm in worms)
                if (worm.Active)
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
            if (Input.KeyPressed(Key.R))
                Restart();
            wormFrequency += config.step;
            if (FastMath.Round(wormFrequency, stepAccuracy) >= config.size)
            {
                foreach (Worm worm in worms)
                    if (worm.Active)
                        worm.Move();
                wormFrequency = 0;
                if (wormAmount < wormCap)
                {
                    Vector2 random = Random.ValidPosition(collision, config.width, config.height, collision.empty);
                    if (random.X != -1)
                        SpawnWorm(collision.X(random.X), collision.Y(random.Y));
                    else
                    {
                        random = Random.ValidPosition(collision, config.width, config.height, collision.fruit);
                        if (random.X != -1 && collision.Check(random) == collision.fruit)
                        {
                            Fruit fruit = (Fruit)collision.Get(random);
                            fruit.Disable();
                            SpawnWorm(collision.X(random.X), collision.Y(random.Y));
                        }
                        else
                        {
                            if (wormAmount > 0)
                                wormAmount--;
                        }
                    }
                }
#if DEBUG
                if (config.visualizeCollision)
                    collision.Visualize();
#endif
            }
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
            wormAmount++;
        }


        /// <summary>
        /// Turn a worm into a block.
        /// </summary>
        /// <param name="worm">Worm to transform</param>
        /// <returns>Block or null</returns>
        public BlockModule SpawnBlock(Worm worm)
        {
            if (wormAmount > 0)
                wormAmount--;
            return blocks.SpawnBlock(worm);
        }


        /// <summary>
        /// Spawn a player.
        /// </summary>
        /// <param name="playerNumber">Player number</param>
        /// <returns>Player</returns>
        public Player SpawnPlayer(int playerNumber)
        {
            return players.Enable().Spawn(playerNumber, config.windowWidth / 2, config.windowHeight / 2);
        }


        /// <summary>
        /// Spawn a fruit.
        /// </summary>
        private void SpawnFruit()
        {
            fruits.Enable().Spawn();
        }


        /// <summary>
        /// Creates a visible border for the field.
        /// </summary>
        private void CreateBorders(int width, int height)
        {
            Image backgroundGraphic = Image.CreateRectangle(width * config.size, height * config.size, Color.None);
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
