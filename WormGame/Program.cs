using Otter.Core;
using Otter.Graphics.Drawables;
using System;
using WormGame.Core;

namespace WormGame
{
    /// @author Antti Harju
    /// @version 24.07.2020
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
            Image.CirclePointCount = 36;

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