using Otter.Graphics;
using Otter.Graphics.Drawables;
using Otter.Utility.MonoGame;
using WormGame.Core;
using WormGame.Pooling;

namespace WormGame.GameObject
{
    public class DebugEntity : PoolableEntity
    {
        public DebugEntity(Config config)
        {
            Image image = Image.CreateCircle(config.size / 4, Color.White);
            image.OutlineThickness = config.size / 8;
            image.OutlineColor = Color.Black;
            image.CenterOrigin();
            AddGraphic(image);
        }

        public void Spawn(Vector2 position)
        {
            X = position.X;
            Y = position.Y;
        }
    }
}
