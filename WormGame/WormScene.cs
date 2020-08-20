using System.Collections;
using Otter.Core;
using Otter.Utility;
using Otter.Utility.MonoGame;
using Otter.Graphics;
using Otter.Graphics.Drawables;
using WormGame.Core;
using WormGame.Static;
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
        private readonly float updateInterval;
        private readonly Players players;
        private readonly Worms worms;
        private readonly Fruits fruits;
        private readonly Blocks blocks;
        private float currentStep;
        private int wormsAlive;

        /// Loaded from configuration
        private readonly Collision collision;
        private readonly float step;
        private readonly int fruitAmount;
        private readonly int minWormLength;
        private readonly int wormCap;
        private readonly int size;
        private readonly int width;
        private readonly int height;
        private readonly int windowWidth;
        private readonly int windowHeight;
        private readonly bool spawnFruits;
#if DEBUG
        private readonly bool visualizeCollision;
#endif


        /// <summary>
        /// Initializes poolers and scene entities.
        /// </summary>
        /// <param name="config">Configuration</param>
        public WormScene(Config config, Game game)
        {
#if DEBUG
            visualizeCollision = config.visualizeCollision;
#endif
            updateInterval = 1.0f / config.refreshRate;
            AddGraphic(config.surface);
            AddGraphic(config.tilemap);
            collision = config.collision;
            spawnFruits = config.spawnFruits;
            fruitAmount = config.fruitAmount;
            minWormLength = config.minWormLength;
            step = config.step;
            wormCap = config.wormCap;
            size = config.size;
            width = config.width;
            height = config.height;
            windowWidth = config.windowWidth;
            windowHeight = config.windowHeight;
            // Config load end

            currentStep = config.size - config.step;
            CreateBorders(config.width, config.height, config.foregroundColor);

            worms = new Worms(config, this);
            players = new Players(config, this);
            fruits = new Fruits(config);
            blocks = new Blocks(config);
            Start();
        }


        /// <summary>
        /// Kicks off the manual update coroutine.
        /// </summary>
        public override void Begin()
        {
            Game.Coroutine.Start(UpdateRoutine());
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
                players.Reset();
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
                    float distance = Vector2.Distance(position, worm.Target);
                    if (distance < nearestDistance)
                    {
                        nearestWorm = worm;
                        nearestDistance = distance;
                    }
                }
            return nearestWorm;
        }


        /// <summary>
        /// Runs config.refreshRate times per second.
        /// </summary>
        private IEnumerator UpdateRoutine()
        {
            yield return Coroutine.Instance.WaitForSeconds(updateInterval);
            currentStep += step;
            if (FastMath.Round(currentStep, step / 2) >= size)
                Move();
            foreach (Worm worm in worms)
                if (worm.Active)
                    worm.Update();
            Game.Coroutine.Start(UpdateRoutine());
        }


        /// <summary>
        /// Listens to input for restart.
        /// </summary>
        public override void Update()
        {
            if (Input.KeyPressed(Key.R))
                Restart();
        }


        /// <summary>
        /// Makes the world go round.
        /// </summary>
        public void Move()
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


        /// <summary>
        /// Spawns a worm.
        /// </summary>
        /// <param name="x">Horizontal field position</param>
        /// <param name="y">Vertical field position</param>
        /// <param name="length">Length, default config.minWormLength</param>
        /// <param name="color">Color, default Random.Color</param>
        public Worm SpawnWorm(int x, int y, int length)
        {
            wormsAlive++;
            return worms.SpawnWorm(x, y, length, Random.Color);
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
        /// Creates a visible border for the field.
        /// </summary>
        private void CreateBorders(int width, int height, Color color)
        {
            Image borders = Image.CreateRectangle(width * size, height * size, Color.None);
            borders.OutlineThickness = size / 6;
            if (borders.OutlineThickness < 1)
                borders.OutlineThickness = 1;
            borders.OutlineColor = color;
            borders.CenterOrigin();
            borders.X = windowWidth / 2;
            borders.Y = windowHeight / 2;
            AddGraphic(borders);
        }
    }
}
