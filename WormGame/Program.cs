using Otter;

namespace WormGame
{
    /// @author anvemaha
    /// @version 01.07.2020
    /// <summary>
    /// Program starts here
    /// </summary>
    class Program
    {
        static void Main()
        {
            Game game = new Game("Worm bricks", 1280, 720, 144)
            {
                MeasureTimeInFrames = false,
                WindowResize = false,
                AlwaysUpdate = true,
                MouseVisible = true
            };
            game.Start(new WormScene(game));
        }
    }
}