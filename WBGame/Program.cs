using Otter;

namespace WBGame
{
    /// @author Antti Harju
    /// @version 21.06.2020
    /// <summary>
    /// Entry point to the program, scenes and technical things are done / set up here
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