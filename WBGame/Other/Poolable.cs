using Otter;

namespace WBGame.Other
{
    public class Poolable : Entity
    {
        public bool Enabled { get { return Graphic.Visible; } set { Graphic.Visible = value; } }


        public virtual Color Color { get { return Graphic.Color ?? null; } set { Graphic.Color = value; } }
    }
}