using Otter.Graphics;
using Otter.Graphics.Drawables;
using WormGame.Core;
using WormGame.Static;
using WormGame.Pooling;

namespace WormGame.GameObject
{
    /// @author Antti Harju
    /// @version 28.07.2020
    /// <summary>
    /// Spawns a worm spawn warning animation that spawns a worm.
    /// </summary>
    /// TODO: Merge to worm once done with optimizing performance.
    public class WormWarning : PoolableEntity
    {
        private readonly Collision collision;
        private readonly float size;
        private readonly float step;
        private readonly float stepAccuracy;
        private readonly float spawnDuration;

        private WormScene scene;
        private float scaleProgress;
        private int x;
        private int y;
        private int length;
        private bool spawnWorm;


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="config"></param>
        public WormWarning(Config config)
        {
            spawnDuration = config.wormSpawnDuration;
            step = 1 / spawnDuration;
            stepAccuracy = step / 2;
            size = config.size;
            collision = config.collision;
            Graphic = Image.CreateCircle(config.size / 2);
            Graphic.CenterOrigin();
            Graphic.Scale = 0;
        }


        /// <summary>
        /// Spawns the warning.
        /// </summary>
        /// <param name="x">Horizontal field position</param>
        /// <param name="y">Vertical field position</param>
        /// <param name="length">Worm length</param>
        /// <param name="color">Worm color</param>
        public void Spawn(int x, int y, int length, Color color = null)
        {
            spawnWorm = true;
            scaleProgress = 0;
            scene = (WormScene)Scene;
            this.x = x;
            this.y = y;
            this.length = length;
            Graphic.Color = color;
            X = collision.EntityX(x);
            Y = collision.EntityY(y);
            collision.Set(this, x, y);
        }


        /// <summary>
        /// Animates the warning.
        /// </summary>
        public override void Update()
        {
            if (Enabled)
            {
                scaleProgress += step;
                int currentScale = Mathf.FastRound(scaleProgress, stepAccuracy);
                if (currentScale < size)
                    Graphic.Scale = (scaleProgress / size);
                else
                {
                    if (spawnWorm)
                    {
                        scene.SpawnWorm(x, y, length, Graphic.Color);
                        spawnWorm = false;
                    }
                    else // Done like this so the transition seems smooth. Otherwise there's one black frame.
                    {
                        Disable();
                    }
                }
            }
        }
    }
}
