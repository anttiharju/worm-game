using Otter;

namespace WBGame
{
    class Program
    {
        /// <summary>
        /// Entry point to the program, setting up technical things here and game-specific things elsewhere
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Game game = new Game("Worm bricks", 1280, 720, 72, false);
            Scene scene = new Scene();
            game.MouseVisible = true;
            /**/
            WormGame snake = new WormGame();
            game.Start(snake.Start(scene));
            /**/
            /** /
            TestGame test = new TestGame();
            game.Start(test.Start(scene));
            /**/
        }
    }
}