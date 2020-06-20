using Otter;
using System.Collections;
using WBGame.GameObject;
using WBGame.Other;

namespace WBGame
{
    class WormScene : Scene
    {
        private Manager manager;

        public override void Begin()
        {
            base.Begin();

            manager = new Manager(this, 120, 6, 1000, 32);

            Worm worm = manager.SpawnWorm(500, 500, 3, Color.Red);
            manager.SpawnPlayer(worm);
            Game.Coroutine.Start(MainRoutine());
        }

        IEnumerator MainRoutine()
        {
            float second = Game.Framerate;
            int frequency = (int)(second / 30);
            yield return Coroutine.Instance.WaitForFrames(frequency);
            manager.Move();
            Game.Coroutine.Start(MainRoutine());
        }
    }
}
