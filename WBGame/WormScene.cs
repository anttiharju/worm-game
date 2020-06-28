using Otter;
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
        private readonly float bunchTimerReset = 0.4f;
        private readonly float wormTimerReset = 0.2f;
        private float bunchTimer = 0;
        private float wormTimer = 0;

        public override void Begin()
        {
            base.Begin();
            manager = new Manager(this, 100, 10, 800, 16);
            manager.SpawnWorm(640, 712, 5, Color.Blue);
            manager.SpawnPlayer(640, 376, Color.Red);
        }

        public override void Update()
        {
            base.Update();
            wormTimer += Game.DeltaTime;
            bunchTimer += Game.DeltaTime;
            if (wormTimer >= wormTimerReset)
            {
                wormTimer = 0;
                manager.WormUpdate();
            }
            if (bunchTimer >= bunchTimerReset)
            {
                bunchTimer = 0;
                manager.BunchUpdate();
            }
        }
    }
}
