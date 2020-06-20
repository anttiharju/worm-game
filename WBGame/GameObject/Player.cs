using Otter;
using System;
using WBGame.Other;

namespace WBGame.GameObject
{
    class Player : Entity
    {
        private readonly Manager manager;
        private Queue queue;
        private Worm worm;

        public Player(Manager manager)
        {
            this.manager = manager;
        }


        private void Posess()
        {
            worm = manager.Posess(worm);
            if (worm != null) queue = worm.GetQueue();
        }


        public override void Update()
        {
            base.Update();
            if (queue != null)
            {
                if (Input.KeyPressed(Key.W))
                    queue.Add(1);
                if (Input.KeyPressed(Key.A))
                    queue.Add(2);
                if (Input.KeyPressed(Key.S))
                    queue.Add(3);
                if (Input.KeyPressed(Key.D))
                    queue.Add(4);
            }

            if (Input.KeyPressed(Key.Tab))
                Posess();

            if (Input.KeyPressed(Key.Q))
                manager.SpawnWorm(500, 500, 5, Color.Red);

            if (Input.KeyPressed(Key.R))
                manager.Blockify(worm);
        }
    }
}
