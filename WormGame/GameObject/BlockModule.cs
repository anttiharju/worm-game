using Otter.Graphics;
using Otter.Graphics.Drawables;
using Otter.Utility.MonoGame;
using WormGame.Core;
using WormGame.Pooling;

namespace WormGame.GameObject
{
    public class BlockModule : PoolableObject
    {
        private readonly Collision field;

        public BlockModule Next { get; set; }

        public Vector2 Target { get; private set; }

        public Image Graphic { get; private set; }

        public override bool Enabled { get { return enabled; } set { enabled = value; Graphic.Visible = value; } }
        private bool enabled;


        public BlockModule(Config config)
        {
            field = config.field;
            Graphic = Image.CreateRectangle(config.size);
            Graphic.CenterOrigin();
        }

        public void CloneWorm(Worm worm, WormModule wormModule, Block parent, Pooler<BlockModule> brickModules, int currentLength, int i = 1)
        {
            Target = wormModule.Target;
            Graphic.X = wormModule.Target.X - worm.Position.X;
            Graphic.Y = wormModule.Target.Y - worm.Position.Y;
            Graphic.Color = worm.Color;

            parent.AddGraphic(Graphic);
            field.Set(parent, wormModule.Target);
            if (i < currentLength)
            {
                Next = brickModules.Enable();
                Next.CloneWorm(worm, wormModule.Next, parent, brickModules, currentLength, ++i);
            }
        }

        public void SetColor(Color color)
        {
            if (Next != null)
                Next.SetColor(color);
            Graphic.Color = color;
        }

        public void Disable(Vector2 parentPosition)
        {
            field.Set(null, parentPosition.X + Graphic.X, parentPosition.Y + Graphic.Y);
            if (Next != null)
                Next.Disable(parentPosition);
            Disable();
        }

        public override void Disable()
        {
            Enabled = false;
            Next = null;
            Graphic.X = 0;
            Graphic.Y = 0;
        }
    }
}
