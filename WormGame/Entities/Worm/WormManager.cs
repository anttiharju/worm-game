using Otter.Graphics;
using WormGame.Core;
using WormGame.Pooling;

namespace WormGame.Entities
{
    public class WormManager : Pooler<Worm>
    {
        private readonly Pooler<WormModule> modules;

        public WormManager(WormScene scene, Config config)
        {
            modules = new Pooler<WormModule>(scene, config, config.moduleAmount);

            int capacity = config.wormAmount;
            pool = new Worm[capacity];
            endIndex = capacity - 1;
            for (int i = 0; i < capacity; i++)
            {
                Worm current = new Worm(config, scene, modules);
                current.Disable(false);
                pool[i] = current;
            }
        }

        public override void Reset()
        {
            base.Reset();
            modules.Reset();
        }


        public Worm SpawnWorm(int x, int y, int length, Color color)
        {
            Worm worm = Enable();
            if (worm == null) return null;
            return worm.Spawn(x, y, length, color);
        }
    }
}
