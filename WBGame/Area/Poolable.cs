using Otter;
using System;

namespace WBGame.Area
{
    interface IPoolable
    {
        bool Available();
        void Spawn(int x, int y);
        void Free();
    }

    class Poolable : Entity, IPoolable
    {
        private bool alive;

        public Poolable(float x, float y, int size = 32) : base(x, y)
        {
            Image image = Image.CreateRectangle(size);
            AddGraphic(image);
            image.CenterOrigin();
            Free();
        }

        public bool Available()
        {
            return !alive;
        }

        public void Spawn(int x, int y)
        {
            Graphic.Visible = true;
            Position = new Vector2(x, y);
            alive = true;
        }

        public void Free()
        {
            Graphic.Visible = false;
            alive = false;
        }
    }
}
