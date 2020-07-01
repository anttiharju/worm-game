using Otter;

namespace WormGame
{
    /// @author Antti Harju
    /// @version 01.07.2020
    /// <summary>
    /// Program starts here
    /// </summary>
    class Program
    {
        static void Main()
        {
            Game game = new Game("Worm bricks", 1920, 1080, 144, true)
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