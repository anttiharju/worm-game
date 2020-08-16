using Otter.Graphics;
using WormGame.Core;
using WormGame.Pooling;

namespace WormGame.Entities
{
    public class Worms : Pooler<Worm>
    {
        private readonly Pooler<WormModule> modules;

        public Worms(Config config, WormScene scene)
        {
            modules = new WormModulePooler(config, scene);

            int capacity = config.wormAmount;
            pool = new Worm[capacity];
            endIndex = capacity - 1;
            for (int i = 0; i < capacity; i++)
            {
                Worm current = new Worm(config, modules);
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


    public class WormModulePooler : Pooler<WormModule>
    {
        public WormModulePooler(Config config, WormScene scene)
        {
            int capacity = config.moduleAmount;
            pool = new WormModule[capacity];
            endIndex = capacity - 1;
            for (int i = 0; i < capacity; i++)
            {
                WormModule current = new WormModule(config);
                current.Disable(false);
                pool[i] = current;
                current.AddTo(scene);
            }
        }
    }
}
