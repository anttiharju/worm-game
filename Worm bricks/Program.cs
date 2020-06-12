using Otter;

namespace Worm_bricks
{
    class Program
    {
        static void Main(string[] args)
        {
            var game = new Game("Worm bricks", 1280, 720, 144, false);
            game.Start();
        }
    }
}