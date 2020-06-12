using Otter;
using WBGame.Entity.Snake;

namespace WBGame
{
    class WormGame
    {
        public Scene Start(Scene scene)
        {
            Worm worm = new Worm(scene, 960, 540, 64, 3, 5);
            scene.Add(worm);
            return scene;
        }
    }
}
