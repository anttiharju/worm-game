using Otter;
using WBGame.Other;

namespace WBGame.GameObject
{
    /// <summary>
    /// Block class. Very much work in progress.
    /// </summary>
    class Block : Poolable
    {
        public Block NextBlock { get; set; }

        public Block(int size) : base()
        {
            Image image = Image.CreateRectangle(size);
            AddGraphic(image);
            image.CenterOrigin();
        }

        public void Spawn(Vector2 position, Color color)
        {
            Position = position;
            Graphic.Color = color;
        }
    }
}
