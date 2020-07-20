using Otter.Core;
using Otter.Graphics.Drawables;
using Otter.Utility;
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
        public static Config config = new Config();
        static void Main()
        {
            Image.CirclePointCount = (int)(config.size * 0.6f);
            Game game = new Game("Worm Blocks", config.windowWidth, config.windowHeight, config.refreshRate, config.fullscreen)
            {
                WindowResize = false,
                AlwaysUpdate = true,
                MouseVisible = true
            };
            game.Start(config.scene);
        }
#if DEBUG
        [OtterCommand(helpText: "Toggle collision visualizer.", group: "game")]
        static void Visualize()
        {
            config.visualizeCollision = !config.visualizeCollision;
        }
#endif
    }
}