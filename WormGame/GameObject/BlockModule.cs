using Otter.Graphics;
using Otter.Graphics.Drawables;
using Otter.Utility.MonoGame;
using WormGame.Core;
using WormGame.Static;
using WormGame.Pooling;

namespace WormGame.GameObject
{
    public class BlockModule : PoolableObject
    {
        private readonly Collision collision;
        private readonly int size;

        public Image Graphic { get; private set; }

        public BlockModule Next { get; set; }

        public override bool Enabled { get { return enabled; } set { enabled = value; Graphic.Visible = value; } }
        private bool enabled;


        public BlockModule(Config config)
        {
            collision = config.collision;
            size = config.size;
            Graphic = Image.CreateRectangle(config.size);
            Graphic.Scale = 0;
        }


        public BlockModule Spawn(Block parent, float x, float y)
        {
            Graphic.Scale = 1;
            Graphic.SetOrigin(0, size);
            Graphic.SetPosition(x - size / 2, y + size / 2);
            parent.AddGraphic(Graphic);
            Graphic.Color = parent.Color;
            return this;
        }

        public override void Disable()
        {
            if (Next != null)
                Next.Disable();
            Enabled = false;
            Graphic.SetPosition(0, 0);
            Graphic.SetOrigin(0, 0);
            Graphic.Scale = 0;
        }
    }
}
