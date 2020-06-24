using Otter;
using WBGame.Other;

namespace WBGame.GameObject
{
    /// <summary>
    /// Player class. Input is handled here and activates things based on it elsewhere.
    /// </summary>
    class Player : Entity
    {
        private readonly Manager manager;
        private readonly Color playerColor;

        private Color oldColor;
        private Controls queue;
        private Worm worm;
        private Worm oldWorm;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="manager">Manager</param>
        /// <param name="color">Players color</param>
        public Player(Manager manager, Color color)
        {
            this.manager = manager;
            playerColor = color;
        }


        /// <summary>
        /// Posesses next worm
        /// </summary>
        private void Posess()
        {
            oldWorm = worm;
            worm = manager.Posess();
            if (worm == null) return;
            if (oldWorm != null)
                oldWorm.SetColor(oldColor);
            oldColor = worm.Graphic.Color;
            worm.SetColor(playerColor);
            queue = worm.GrabControls();
        }


        /// <summary>
        /// KeyPresses are handled here
        /// </summary>
        public override void Update()
        {
            base.Update();
            if (queue != null)
            {
                if (Input.KeyPressed(Key.W))
                    queue.Add('W');
                if (Input.KeyPressed(Key.A))
                    queue.Add('A');
                if (Input.KeyPressed(Key.S))
                    queue.Add('S');
                if (Input.KeyPressed(Key.D))
                    queue.Add('D');
            }

            if (Input.KeyPressed(Key.Tab))
                Posess();

            if (Input.KeyPressed(Key.R))
            {
                manager.Blockify(worm);
                queue = null;
                Posess();
            }

            if (Input.KeyPressed(Key.Q))
                manager.SpawnWorm(640, 360, 5, Helper.RandomColor());
        }
    }
}
