using Otter.Core;
using WormGame.Core;
using WormGame.Static;

namespace WormGame
{
    /// @author Antti Harju
    /// @version v0.5
    /// <summary>
    /// See README.md.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Program entry point.
        /// </summary>
        public static void Main()
        {
            System.Console.ForegroundColor = System.ConsoleColor.DarkGray;
            Settings config = new Settings();
            Game game = new Game("Worm Blocks", config.windowWidth, config.windowHeight, config.refreshRate, config.fullscreen)
            {
                WindowResize = false,
                AlwaysUpdate = true,
                MouseVisible = true,
                Color = Colors.background
            };
            WormScene scene = new WormScene(config, game);
            game.Start(scene);
        }
    }
}