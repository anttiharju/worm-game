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
        private readonly Pooler<Brick> bricks;
        private readonly Pooler<WormBody> wormBodies;

        private int brickCounter = 0;
        private float wormCounter = 0;


        /// <summary>
        /// Initializes poolers and scene entities.
        /// </summary>
        /// <param name="config"></param>
        public WormScene(Config config)
        {
            this.config = config;
            field = config.field;

            CreateBackground(config.size / 3, Color.Gray);
            CreateBackground(0, Color.Black);

            // Entity pools
            worms = new Pooler<Worm>(config, config.brainAmount);
            fruits = new Pooler<Fruit>(config, config.fruitAmount);
            bricks = new Pooler<Brick>(config, config.brainAmount);
            AddMultiple(worms.Pool);
            AddMultiple(fruits.Pool);
            AddMultiple(bricks.Pool);

            // Object pools
            wormBodies = new Pooler<WormBody>(config, config.bodyAmount);

            // Entity setup
            int density = config.density;
            if (density > 0)
                for (int x = 0; x < config.width; x += density)
                    for (int y = 0; y < config.height; y += density)
                        SpawnWorm(x, y);

            if (config.fruits)
                for (int i = 0; i < fruits.Count; i++)
                    fruits.Enable().Spawn();

            SpawnPlayer(config.windowWidth / 2, config.windowHeight / 2, Color.Red);
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
            worm.Spawn(wormBodies, x, y, length, color);
            return worm;
        }


        /// <summary>
        /// Turn a worm into a brick.
        /// </summary>
        /// <param name="worm">Worm to transform</param>
        /// <returns>Brick</returns>
        public Brick SpawnBrick(Worm worm)
        {
            Brick brick = bricks.Enable();
            if (brick == null) return null;
            brick.Spawn(worm);
            return brick;
        }


        /// <summary>
        /// Spawn a player.
        /// </summary>
        /// <param name="x">Horizontal position</param>
        /// <param name="y">Vertical position</param>
        /// <param name="color">Color</param>
        /// <returns>Player</returns>
        /// TODO: Use pooling
        public Player SpawnPlayer(float x, float y, Color color)
        {
            Player player = new Player(this, 0, x, y, color, config.size);
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
                brickCounter++;
                if (brickCounter == config.brickFreq)
                {
                    foreach (Brick brick in bricks)
                        if (brick.Enabled)
                            brick.SoftDrop();
                    brickCounter = 0;
                }
                foreach (Worm worm in worms)
                    if (worm.Enabled)
                        worm.Move();
                wormCounter = 0;
                field.Scan();
#if DEBUG
                if (config.visualizeCollision)
                    field.Visualize(config);
#endif
            }
        }


        /// <summary>
        /// Creates a rectangular entity at on the middle of the field.
        /// </summary>
        /// <param name="offset">Size offset</param>
        /// <param name="color">Rectangle color</param>
        private void CreateBackground(int offset, Color color)
        {
            Image backgroundGraphic = Image.CreateRectangle(config.width * config.size + offset, config.height * config.size + offset, color);
            backgroundGraphic.CenterOrigin();
            Entity background = new Entity(config.windowWidth / 2, config.windowHeight / 2, backgroundGraphic)
            {
                Collidable = false
            };
            Add(background);
        }
    }
}
