using System;
using System.Collections.Generic;
using System.Text;
using WBGame.Pooling;
using Otter;

namespace WBGame.Other
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
            SetColor(color);
        }

        public void SetColor(Color color)
        {
            Graphic.Color = color;
        }
    }
}
