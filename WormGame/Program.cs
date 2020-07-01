using Otter;

namespace WormGame
{
    /// @author Antti Harju
    /// @version 29.06.2020
    /// <summary>
    /// Program starts here
    /// </summary>
    /// <param name="args"></param>
    class Program
    {
        static void Main()
        {
            Game game = new Game("Worm bricks", 1280, 720, 144)
            {
                MeasureTimeInFrames = false,
                WindowResize = false,
                //WindowBorder = false,
                AlwaysUpdate = true,
                MouseVisible = true
            };
            game.Start(new WormScene(game));
        }
    }
}