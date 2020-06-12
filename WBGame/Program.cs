using Otter;

namespace WBGame
{
    class Program
    {
        /// <summary>
        /// Entry point to the program, scenes are set up here and game-specific things elsewhere
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Game game = new Game("Worm bricks", 1280, 720, 72, false);
            Scene scene = new Scene();
            game.MouseVisible = true;
            /** /
            TestGame test = new TestGame();
            game.Start(test.Start(scene));
            /**/
            /** /
            WormGame worm = new WormGame();
            game.Start(worm.Start(scene));
            /**/
            /**/
            AreaGame grid = new AreaGame();
            game.Start(grid.Start(scene));
            /**/
        }
    }
}