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
    public class Block : PoolableEntity
    {
        private BlockModule firstModule;

        public int Count { get; private set; }

        public override Color Color { get { return firstModule.Graphic.Color ?? null; } set { SetColor(value); } }

        public void SetColor(Color color)
        {
            firstModule.SetColor(color);
        }

        public Block Spawn(Worm worm, Pooler<BlockModule> brickModules)
        {
            if (!brickModules.Ask(worm.Length))
            {
                Enabled = false;
                return null;
            }

            Count = worm.Length;
            X = worm.X;
            Y = worm.Y;

            firstModule = brickModules.Enable();
            firstModule.Graphic.Color = worm.Color;
            firstModule.CopyWorm(worm, worm.firstModule, this, brickModules);

            return this;
        }

        public override void Disable()
        {
            firstModule.Disable(Position);
            firstModule = null;
            Enabled = false;
        }
    }
}
