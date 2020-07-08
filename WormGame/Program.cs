using Otter.Core;
using WormGame.Core;

namespace WormGame
{
    /// @author Antti Harju
    /// @version 08.07.2020
    /// <summary>
    /// Program starts here.
    /// </summary>
    class Program
    {
        static void Main()
        {
            int windowWidth = 1280; int windowHeight = 720;
            Config config = new Config(windowWidth, windowHeight);
            Game game = new Game("Worm Bricks", config.windowWidth, config.windowHeight, config.targetFramerate)
            {
                WindowResize = false,
                AlwaysUpdate = true,
                MouseVisible = true
            };
            game.Start(new WormScene(config));
        }
    }
}