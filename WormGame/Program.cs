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
        /// <summary>
        /// Program entry point.
        /// </summary>
        public static void Main()
        {
            Config config = new Config();
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
    }
}