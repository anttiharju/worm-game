using Otter;
using System;
using System.Collections.Generic;
using System.Text;

namespace WBGame.Area
{
    class Grid
    {
        byte[,] grid;

        public Grid(Scene scene, int width, int height, int cellSize, int gap)
        {
            grid = new byte[width, height];

            Pooler<Inherit> blocks = new Pooler<Inherit>(scene, width * height, cellSize);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    blocks.Spawn(x * (cellSize + gap), y * (cellSize + gap));
                }
            }
        }
    }
}
