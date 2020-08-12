using Otter.Core;
using Otter.Graphics;
using Otter.Graphics.Drawables;
using System;
using System.Collections.Generic;
using System.Text;
using WormGame.Core;
using WormGame.Pooling;

namespace WormGame.Entities
{
    public class BlockManager
    {
        public Tilemap Tilemap;

        public BlockManager(Scene scene, Config config)
        {
            Tilemap = new Tilemap(config.width * config.size, config.height * config.size, config.size, config.size)
            {
                X = config.leftBorder - config.size / 2,
                Y = config.topBorder - config.size / 2
            };
            scene.AddGraphic(Tilemap);

            Tilemap.SetTile(0, 0, Color.Gold, "");
            Tilemap.SetTile(config.width - 1, config.height - 1, Color.Gold, "");
        }
    }
}
