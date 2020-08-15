using Otter.Core;
using Otter.Graphics;
using Otter.Graphics.Drawables;
using WormGame.Core;
using WormGame.Pooling;

namespace WormGame.Entities
{
    public class WormManager : Pooler<Worm>
    {
        private readonly Entity graphicRenderer;
        private readonly Pooler<WormModule> modules;

        public WormManager(WormScene scene, Config config)
        {
            graphicRenderer = new Entity(0, 0);
            scene.Add(graphicRenderer);
            modules = new Pooler<WormModule>(null, config, config.moduleAmount);

            int capacity = config.wormAmount;
            pool = new Worm[capacity];
            endIndex = capacity - 1;
            for (int i = 0; i < capacity; i++)
            {
                Worm current = new Worm(scene, config, this);
                current.Disable(false);
                pool[i] = current;
            }
        }

        public void Update()
        {
            foreach (Worm worm in pool)
                worm.Update();
        }

        public override void Reset()
        {
            base.Reset();
            graphicRenderer.ClearGraphics();
        }


        public Worm SpawnWorm(int x, int y, int length, Color color)
        {
            Worm worm = Enable();
            if (worm == null) return null;
            return worm.Spawn(modules, x, y, length, color);
        }


        public void AddGraphic(Image image)
        {
            graphicRenderer.AddGraphic(image);
        }
    }
}
