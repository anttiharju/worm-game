using Otter;
using SFML.Window;
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
        private int areaWidth = 20;
        private int areaHeight = 40;
        private int areaMargin = 2;

        public override void Begin()
        {
            base.Begin();
            int size = Scaling();
            manager = new Manager(this, 3, 60, 3, size, areaWidth, areaHeight);
            manager.SpawnWorm(Game.WindowWidth / 2 + size / 2,  Game.WindowHeight / 2 + areaHeight / 2 * size - size / 2, 59, Color.Blue);
            manager.SpawnPlayer(640, 376, Color.Red);
        }

        private int Scaling()
        {
            int xSize = Game.WindowWidth / (areaWidth + areaMargin * 2);
            int ySize = Game.WindowHeight / (areaHeight + areaMargin * 2);
            System.Console.WriteLine(xSize + " " + ySize);
            int size = Helper.Smaller(xSize, ySize);
            if (size % 2 != 0)
                size--;
            return size;
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
