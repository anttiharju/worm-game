using Otter.Core;
using WormGame.Core;

namespace WormGame
{
    /// @author Antti Harju
    /// @version 08.07.2020
    /// <summary>
    /// Program starts here.
    /// </summary>
    public class Program
    {
        static void Main()
        {
            int windowWidth = 1280;
            int windowHeight = 720;
            bool fullscreen = false;
            Config config = new Config(windowWidth, windowHeight);
            Game game = new Game("Worm Bricks", config.windowWidth, config.windowHeight, config.targetFramerate, fullscreen)
            {
                WindowResize = false,
                AlwaysUpdate = true,
                MouseVisible = true
            };
            game.Start(new WormScene(config));
        }
    }
}