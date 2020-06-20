using Otter;
using WBGame.Worm;

namespace WBGame.Other
{
    class Player : Entity
    {
        private Manager manager;
        private Head worm;

        public Player(Manager manager, Head worm)
        {
            this.manager = manager;
            this.worm = worm;
        }


        public override void Update()
        {
            base.Update();
            if (Input.KeyPressed(Key.R))
                manager.Blockify(worm);
        }
    }
}
