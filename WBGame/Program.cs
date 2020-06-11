using Otter;

namespace WBGame
{
    class Program
    {
        static void Main(string[] args)
        {
            var game = new Game("Worm bricks", 1280, 720, 144);
            Scene scene = new Scene();
            scene.Add(new PlayerEntity(Game.Instance.HalfWidth, Game.Instance.HalfHeight));
            game.Start(scene);
        }
    }
}