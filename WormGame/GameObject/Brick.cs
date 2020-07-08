using Otter.Graphics.Drawables;
using WormGame.Core;
using WormGame.Pooling;

namespace WormGame.GameObject
{
    class Brick : Poolable
    {
        public Brick(Config config) : base()
        {
            Image image = Image.CreateRectangle(config.size);
            AddGraphic(image);
            image.CenterOrigin();
        }
    }
}
