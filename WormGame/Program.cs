using Otter.Core;
using Otter.Graphics.Drawables;
using Otter.Utility;
using WormGame.Core;

namespace WormGame
{
    /// @author Antti Harju
    /// @version 20.07.2020
    /// <summary>
    /// See README.md for an explanation of game mechanics.
    /// </summary>
    public class Program
    {
        public static Config config = new Config();
        
        
        /// <summary>
        /// Program entry point.
        /// </summary>
        static void Main()
        {
            int circleAccuracy = (int)(config.size * 0.6f);
            if (circleAccuracy > 6)
                Image.CirclePointCount = circleAccuracy;

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
        /// Toggle collision visualization in-game.
        /// </summary>
        [OtterCommand(helpText: "Toggle collision visualizer.", group: "game")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used by Otter2d in-game debug console.")]
        static void Visualize()
        {
            config.visualizeCollision = !config.visualizeCollision;
        }
#endif
    }
}