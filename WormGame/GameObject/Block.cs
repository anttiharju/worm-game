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

        public override Color Color { get { return firstModule.Graphic.Color ?? null; } set { firstModule.SetColor(value); } }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Required by Pooler")]
        public Block(Config config) { }

        public Block Spawn(Worm worm, Pooler<BlockModule> blockModules, int currentLength)
        {
            X = worm.X;
            Y = worm.Y;

            firstModule = blockModules.Enable();
            if (firstModule == null)
            {
                Disable();
                return null;
            }
            firstModule.CloneWorm(worm, worm.firstModule, this, blockModules, currentLength);

            return this;
        }

        public override void Disable()
        {
            if (firstModule != null)
                firstModule.Disable(Position);
            ClearGraphics();
            Enabled = false;
        }
    }
}
