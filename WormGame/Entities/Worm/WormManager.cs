using Otter.Core;
using Otter.Graphics;
using Otter.Graphics.Drawables;
using WormGame.Core;
using WormGame.Pooling;

namespace WormGame.Entities
{
    public class WormManager : Pooler<Worm>
    {
        private readonly Tilemap tilemap;
        private readonly Entity graphicRenderer;
        private readonly Pooler<WormModule> modules;

        public WormManager(WormScene scene, Config config)
        {
            tilemap = config.tilemap;
            graphicRenderer = new Entity();
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


        public void Add(int x, int y, Color color)
        {
            tilemap.SetTile(x, y, color, "");
        }

        public void Clear(int x, int y)
        {
            tilemap.ClearTile(x, y, "");
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
