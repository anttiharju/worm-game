using Otter;
using WormGame.Help;

namespace WormGame
{
    /// @author Antti Harju
    /// @version 01.07.2020
    /// <summary>
    /// Program starts here
    /// </summary>
    class Program
    {
        static void Main()
        {
            Config config = new Config();
            Game game = new Game("Worm bricks", 1280, 720, Config.targetFramerate)
            {
                MeasureTimeInFrames = false,
                WindowResize = false,
                AlwaysUpdate = true,
                MouseVisible = true
            };
            game.Start(new WormScene(game, config));
        }
    }
}