using Otter;
using System;
using System.Collections.Generic;
using System.Text;

namespace WBGame.Area
{
    class Grid
    {
        byte[,] grid;

        public Grid(int x, int y)
        {
            grid = new byte[x, y];
        }
    }
}
