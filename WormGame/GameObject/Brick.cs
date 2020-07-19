using Otter.Graphics;
using WormGame.Core;
using WormGame.Pooling;

namespace WormGame.GameObject
{
    /// @author Antti Harju
    /// @version 18.07.2020
    /// <summary>
    /// Brick class. Work in progress.
    /// </summary>
    public class Brick : PoolableEntity
    {
        private BrickModule firstModule;


        public int Count { get; private set; }


        public Player Player { get; set; }


        public override Color Color { get { return firstModule.Graphic.Color ?? null; } set { SetColor(value); } }

        public Brick(Config config) : base() { }


        public void SetColor(Color color)
        {
            firstModule.SetColor(color);
        }

        public Brick Spawn(Worm worm, Pooler<BrickModule> brickModules)
        {
            Count = worm.Length;

            X = worm.X;
            Y = worm.Y;

            firstModule = brickModules.Enable();
            bool neighbour = firstModule.CopyWorm(worm, worm.firstModule, this, brickModules);
            if (neighbour)
            {
                Disable();
            }
            Color = worm.Color;

            return this;
        }

        public override void Disable()
        {
            firstModule.Disable(Position);
            base.Disable();
        }
    }
}
