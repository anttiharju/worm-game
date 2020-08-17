using Otter.Graphics;
using Otter.Graphics.Drawables;
using System;
using WormGame.Core;
using WormGame.Pooling;

namespace WormGame.Entities
{
    public class Worms : Pooler<Worm>
    {
        private readonly Surface surface;
        private readonly Pooler<WormModule> modules;

        public Worms(Config config, WormScene scene)
        {
            surface = config.surface;
            modules = new Pooler<WormModule>(scene, config, config.moduleAmount);

            int capacity = config.wormAmount;
            pool = new Worm[capacity];
            endIndex = capacity - 1;
            for (int i = 0; i < capacity; i++)
            {
                Worm current = new Worm(config, scene, modules);
                current.Disable(false);
                current.Add(scene);
                pool[i] = current;
            }
        }

        public override void Reset()
        {
            surface.Clear();
            modules.Reset();
            base.Reset();
        }

        public Worm SpawnWorm(int x, int y, int length, Color color)
        {
            Worm worm = Enable();
            if (worm == null) return null;
            return worm.Spawn(x, y, length, color);
        }
    }
}
