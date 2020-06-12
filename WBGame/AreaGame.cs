using Otter;
using Grid = WBGame.Area.Grid;

namespace WBGame
{
    class AreaGame
    {
        public Scene Start(Scene scene)
        {
            Grid grid = new Grid(10, 20);
            return scene;
        }
    }
}