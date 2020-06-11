using Otter;
using WBGame.Entity;

namespace WBGame
{
    class Program
    {
        static void Main(string[] args)
        {
            var game = new Game("Worm bricks", 1280, 720, 144);
            Scene scene = new Scene();
            scene.Add(new Player(Game.Instance.HalfWidth, Game.Instance.HalfHeight));
            Square[] squares = new Square[10];
            for (int i = 0; i < 10; i++)
            {
                squares[i] = new Square(50 + Game.Instance.HalfWidth + i * 50, Game.Instance.HalfHeight);
                scene.Add(squares[i]);
            }
            Manager manager = new Manager(squares);
            scene.Add(manager);
            game.Start(scene);
        }
    }
}