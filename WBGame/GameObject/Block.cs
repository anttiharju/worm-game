using WBGame.Pooling;
using Otter;

namespace WBGame.GameObject
{
    class Block : Poolable
    {
        public Block(int size) : base()
        {
            Image image = Image.CreateRectangle(size);
            AddGraphic(image);
            image.CenterOrigin();
        }


        public void Spawn(Vector2 position, Color color)
        {
            Enable();
            Position = position;
            Color = color;
        }
    }
}
