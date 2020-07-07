using System.Collections;
using Otter.Core;
using Otter.Utility;
using Otter.Graphics;
using Otter.Utility.MonoGame;
using WormGame.Static;
using WormGame.Manager;
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
        private readonly Game game;
        private readonly Config config;
        private readonly Collision field;

        private readonly Pooler<Worm> worms;
        private readonly Pooler<Brick> bricks;
        private readonly Pooler<WormBase> tails;
        private readonly Pooler<BrickBase> basebricks;

        /// <summary>
        /// Initializes poolers and collision system. Spawns initial entities.
        /// </summary>
        /// <param name="game"></param>
        public WormScene(Game game, Config config)
        {
            this.game = game;
            this.config = config;
            field = new Collision(game, config);

            bricks = new Pooler<Brick>(this, config, config.wormAmount, config.size);
            basebricks = new Pooler<BrickBase>(this, config, config.tailAmount, config.size);
            tails = new Pooler<WormBase>(this, config, config.tailAmount, config.size);
            worms = new Pooler<Worm>(this, config, config.wormAmount, config.size);

            // Entity setup
            int density = 4;
            for (int x = 0; x < config.width; x += density)
                for (int y = 0; y < config.height; y += density)
                    SpawnWorm(x, y);
            SpawnPlayer(game.HalfWidth, game.HalfHeight, Color.Red);
            game.Coroutine.Start(WormRoutine());
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
            worm.Spawn(tails, field, field.EntityX(x), field.EntityY(y), length, color);
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
            Player tmpPlayer = new Player(this, 0, x, y, color, config.size);
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

        public override void UpdateFirst()
        {
            base.UpdateFirst();
            config.UpdateFirst(game);
        }

        /// <summary>
        /// Makes every worm move to their next position
        /// </summary>
        IEnumerator WormRoutine()
        {
            yield return Coroutine.Instance.WaitForSeconds(config.wormFreq);
            foreach (Worm worm in worms)
                if (worm.Enabled)
                    worm.Move();
            if (Config.visualizeCollision)
                field.Visualize();
            Game.Coroutine.Start(WormRoutine());
        }
    }
}
