using Otter;
using System.Runtime.InteropServices;
using WormGame.Other;

namespace WormGame.GameObject
{
    /// <summary>
    /// Block class. Very much work in progress.
    /// </summary>
    class Block : Poolable
    {
        private int size;
        public Block NextBlock { get; set; }

        public Block(int size) : base()
        {
            this.size = size;
            Image image = Image.CreateRectangle(size);
            AddGraphic(image);
            image.CenterOrigin();
        }

        public void Spawn(Vector2 position, Color color)
        {
            Position = position;
            Graphic.Color = color;
        }

        public virtual void HardDrop()
        {

        }

        public virtual void SoftDrop(int up = 1)
        {
            if (NextBlock != null)
                NextBlock.SoftDrop(up);
            Y += size * up;
        }

        public float Lowest(float biggest)
        {
            if (NextBlock != null)
                biggest = NextBlock.Lowest(Mathf.Bigger(Y, biggest));
            return biggest;
        }

        public Block GetBlock(int i, int currentI = 0)
        {
            if (i > currentI)
                return NextBlock.GetBlock(i, ++currentI);
            else
                return this;
        }

        /// <summary>
        /// Recursively changes color of the whole bunch
        /// </summary>
        /// <param name="color">Worms new color</param>
        public void SetColor(Color color)
        {
            if (NextBlock != null)
                NextBlock.SetColor(color);
            Graphic.Color = color;
        }
    }
}
