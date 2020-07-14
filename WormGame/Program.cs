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
            Config config = new Config();
            Game game = new Game("Worm Bricks", config.windowWidth, config.windowHeight, config.refreshRate, config.fullscreen)
            {
                WindowResize = false,
                AlwaysUpdate = true,
                MouseVisible = true
            };
            game.Start(config.scene);
        }
    }
}