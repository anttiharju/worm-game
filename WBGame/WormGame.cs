using Otter;
using WBGame.Worm;

namespace WBGame
{
    class WormGame
    {
        /// <summary>
        /// Test scene for worms
        /// </summary>
        /// <param name="scene">scene created in program.cs</param>
        /// <returns>scene back to program.cs</returns>
        public Scene Start(Scene scene)
        {
            Head worm = new Head(scene, 960, 540, 64, 3, 5);
            scene.Add(worm);
            return scene;
        }
    }
}
