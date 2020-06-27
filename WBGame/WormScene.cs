using Otter;
using System.Collections;
using WBGame.Other;

namespace WBGame
{
    /// @author Antti Harju
    /// @version 21.6.2020
    /// <summary>
    /// Scene for the worm game.
    /// </summary>
    class WormScene : Scene
    {
        private Manager manager;
        private float wormTimer = 0;
        private float wormTimerReset = 0.2f;

        public override void Begin()
        {
            base.Begin();
            manager = new Manager(this, 100, 10, 800, 16);
            manager.SpawnWorm(600, 600, 8, Color.Blue);
            manager.SpawnPlayer(Color.Red);
        }

        public override void Update()
        {
            base.Update();
            wormTimer += Game.DeltaTime;
            if (wormTimer >= wormTimerReset)
            {
                wormTimer = 0;
                manager.WormUpdate();
            }
        }
    }
}
