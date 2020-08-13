using Otter.Core;
using Otter.Utility;
using WormGame.Core;

namespace WormGame
{
    /// @author Antti Harju
    /// @version 14.08.2020
    /// <summary>
    /// See README.md for an explanation of game mechanics.
    /// </summary>
    public class Program
    {
        private static Config config;


        /// <summary>
        /// Program entry point.
        /// </summary>
        public static void Main()
        {
            System.Console.ForegroundColor = System.ConsoleColor.DarkGray;
            config = new Config();
            Game game = new Game("Worm Blocks", config.windowWidth, config.windowHeight, config.refreshRate, config.fullscreen)
            {
                WindowResize = false,
                AlwaysUpdate = true,
                MouseVisible = true
            };
            game.Start(config.scene);
        }
#if DEBUG
        /// <summary>
        /// Command to toggle collision visualization.
        /// </summary>
        [OtterCommand(helpText: "Toggle collision visualizer.", group: "Custom Commands")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Otter debug console command")]
        private static void Visualize()
        {
            config.visualizeCollision = !config.visualizeCollision;
        }
#endif
    }
}