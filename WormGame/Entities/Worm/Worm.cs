using Otter.Core;
using Otter.Graphics;
using Otter.Utility.MonoGame;
using WormGame.Core;
using WormGame.Pooling;

namespace WormGame.Entities
{
    public class Worm : Poolable
    {
        private readonly Pooler<WormModule> modules;
        private readonly Collision collision;
        private readonly WormScene scene;

        private WormModule firstModule;
        private Vector2 position;
        private Vector2 direction;
        private Vector2 previousDirection;
        private Vector2 directionBuffer;
        private int halfSize;
        private int size;


        public Player Player { get; set; }


        public Vector2 Direction { get { return direction; } set { directionBuffer = value; } }


        public int Length { get; private set; }


        public Color Color { get; private set; }


        public Worm(Config config, WormScene scene, Pooler<WormModule> modules)
        {
            this.scene = scene;
            this.modules = modules;
            collision = config.collision;
            halfSize = config.halfSize;
            size = config.size;
        }

        public Worm Spawn(int x, int y, int length, Color color)
        {
            position.X = collision.EntityX(0);
            position.Y = collision.EntityY(0);
            Color = color;
            return null;
        }


        public void Move()
        {
            previousDirection = direction;
            direction = directionBuffer;
            if (direction != previousDirection)
                firstModule = modules.Enable().Initialize(position, direction, Color);
            firstModule?.Grow();
            position += direction * size;
        }

        /// <summary>
        /// Disable worm.
        /// </summary>
        /// <param name="recursive">Disable recursively. False only when disabling is done by pooler.</param>
        public override void Disable(bool recursive = true)
        {
            base.Disable();
            firstModule = null;
            Player = null;
            direction.X = 0;
            direction.Y = 0;
            previousDirection.X = 0;
            previousDirection.Y = 0;
            directionBuffer.X = 0;
            directionBuffer.Y = 0;
        }
    }
}