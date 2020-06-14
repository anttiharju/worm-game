using Otter;
using Grid = WBGame.Area.Grid;
using WBGame.Test;

namespace WBGame
{
    class AreaGame
    {
        public Scene Start(Scene scene)
        {
            Grid grid = new Grid(scene, 10, 20, 32, 5);
            scene.Add(new Player(Game.Instance.HalfWidth, Game.Instance.HalfHeight));
            return scene;
        }
    }
}