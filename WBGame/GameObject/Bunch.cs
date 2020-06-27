using System;
using Otter;
using WBGame.Other;

namespace WBGame.GameObject
{
    /// <summary>
    /// Bunch class. A manager for blocks but also a block. Very much work in progress.
    /// </summary>
    class Bunch : Block
    {
        private readonly Controls controls = new Controls();
        private readonly int size;

        public Bunch(int size) : base(size)
        {
            this.size = size;
        }

        public void Fall()
        {
        }
    }
}
