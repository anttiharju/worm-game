using Otter.Core;
using WormGame.Core;

namespace WormGame
{
    /// @author Antti Harju
    /// @version 18.07.2020
    /// <summary>
    /// Program entry point. Initializes and starts the game.
    /// </summary>
    public class Program
    {
        static void Main()
        {
            Config config = new Config();
            Game game = new Game("Worm Blocks", config.windowWidth, config.windowHeight, config.refreshRate, config.fullscreen)
            {
                WindowResize = false,
                AlwaysUpdate = true,
                MouseVisible = true
            };
            game.Start(config.scene);
        }
    }
}