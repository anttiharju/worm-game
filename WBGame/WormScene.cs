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
        private readonly int groundTruth = 30;
        private int round = 0;

        public override void Begin()
        {
            base.Begin();
            manager = new Manager(this, 100, 10, 800, 16);
            manager.SpawnPlayer(Color.Red);
            Game.Coroutine.Start(WormRoutine());
        }

        IEnumerator WormRoutine()
        {
            float second = Game.Framerate;
            int frequency = (int)(second / groundTruth);
            yield return Coroutine.Instance.WaitForFrames(frequency);
            manager.WormUpdate();
            round++;
            if (round == groundTruth)
            {
                round = 0;
                manager.BlockUpdate();
            }
            Game.Coroutine.Start(WormRoutine());
        }
    }
}
