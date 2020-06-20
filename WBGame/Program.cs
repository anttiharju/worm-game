using Otter;

namespace WBGame
{
    /// @author Antti Harju
    /// @version 14.06.2020
    /// <summary>
    /// Entry point to the program, scenes are set up here and game-specific things elsewhere
    /// </summary>
    /// <param name="args"></param>
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game("Worm bricks", 1280, 720);
            game.FixedFramerate = false;
            game.AlwaysUpdate = true;
            game.MouseVisible = true;
            game.Start(new WormScene());
        }
    }
}