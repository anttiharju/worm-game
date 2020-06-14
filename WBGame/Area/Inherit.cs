using System;
using System.Collections.Generic;
using System.Text;

namespace WBGame.Area
{
    class Inherit : Poolable
    {
        public Inherit(float x, float y, int size) : base(x, y, size) { }
        private string special = "lol";
    }
}
