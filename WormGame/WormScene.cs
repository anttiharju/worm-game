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
        private readonly Pooler<Player> players;
        private readonly Worms worms;
        private readonly Fruits fruits;
        private readonly Blocks blocks;
        private float currentStep;
        private int wormsAlive;

        /// Loaded from configuration
        private readonly Collision collision;
        private readonly Tilemap tilemap;
        private readonly Surface surface;
        private readonly bool spawnFruits;
        private readonly int fruitAmount;
        private readonly int minWormLength;
        private readonly float wormStep;
        private readonly int wormCap;
        private readonly int size;
        private readonly int width;
        private readonly int height;
        private readonly int windowWidth;
        private readonly int windowHeight;
        private readonly bool visualizeCollision;


        /// <summary>
        /// Initializes poolers and scene entities.
        /// </summary>
        /// <param name="config">Configuration</param>
        public WormScene(Config config)
        {
            AddGraphic(config.surface);
            AddGraphic(config.tilemap);
            collision = config.collision;
            tilemap = config.tilemap;
            surface = config.surface;
            spawnFruits = config.spawnFruits;
            fruitAmount = config.fruitAmount;
            minWormLength = config.minWormLength;
            wormStep = config.wormStep;
            wormCap = config.wormCap;
            size = config.size;
            width = config.width;
            height = config.height;
            windowWidth = config.windowWidth;
            windowHeight = config.windowHeight;
            visualizeCollision = config.visualizeCollision;
            // Config load end

            currentStep = config.size - config.wormStep;
            CreateBorders(config.width, config.height, config.foregroundColor);

            worms = new Worms(config, this);
            players = new Pooler<Player>(this, config, 5);
            fruits = new Fruits(config);
            blocks = new Blocks(config);
            Start();
        }


        /// <summary>
        /// Spawns initial entities. Worm spawning is handled in Update().
        /// </summary>
        private void Start()
        {
            if (spawnFruits)
                for (int i = 0; i < fruitAmount; i++)
                    fruits.Spawn();
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
            blocks.Reset();
            collision.Reset();
            wormsAlive = 0;
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
            currentStep += wormStep;
            if (FastMath.Round(currentStep, wormStep / 2) >= size)
            {
                foreach (Worm worm in worms)
                    if (worm.Active)
                        worm.Move();
                currentStep = 0;
                if (wormsAlive < wormCap)
                {
                    Vector2 random = Random.ValidPosition(collision, width, height, collision.empty);
                    if (random.X != -1)
                        SpawnWorm(collision.X(random.X), collision.Y(random.Y), minWormLength);
                    else
                    {
                        random = Random.ValidPosition(collision, width, height, collision.fruit);
                        if (random.X != -1 && collision.Check(random) == collision.fruit)
                        {
                            fruits.Remove(collision.X(random.X), collision.Y(random.Y));
                            SpawnWorm(collision.X(random.X), collision.Y(random.Y), minWormLength);
                        }
                        else
                        {
                            if (wormsAlive > 0)
                                wormsAlive--;
                        }
                    }
                }
#if DEBUG
                if (visualizeCollision)
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
        public void SpawnWorm(int x, int y, int length, Color color = null)
        {
            Worm worm = worms.Enable();
            if (worm == null) return;
            if (color == null) color = Random.Color;
            worm.Spawn(x, y, length, color);
            wormsAlive++;
        }


        /// <summary>
        /// Turn a worm into a block.
        /// </summary>
        /// <param name="worm">Worm to transform</param>
        /// <returns>Block or null</returns>
        public BlockModule SpawnBlock(Worm worm)
        {
            if (wormsAlive > 0)
                wormsAlive--;
            return blocks.SpawnBlock(worm);
        }


        /// <summary>
        /// Spawn a player.
        /// </summary>
        /// <param name="playerNumber">Player number</param>
        /// <returns>Player</returns>
        public Player SpawnPlayer(int playerNumber)
        {
            return players.Enable().Spawn(playerNumber, windowWidth / 2, windowHeight / 2);
        }


        /// <summary>
        /// Creates a visible border for the field.
        /// </summary>
        private void CreateBorders(int width, int height, Color color)
        {
            Image backgroundGraphic = Image.CreateRectangle(width * size, height * size, Color.None);
            backgroundGraphic.CenterOrigin();
            backgroundGraphic.OutlineColor = color;
            backgroundGraphic.OutlineThickness = size / 6;
            Entity background = new Entity(windowWidth / 2, windowHeight / 2, backgroundGraphic)
            {
                Collidable = false
            };
            Add(background);
        }
    }
}
