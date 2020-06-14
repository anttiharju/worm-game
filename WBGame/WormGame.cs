using Otter;
using WBGame.Worm;

namespace WBGame
{
    /// @author Antti Harju
    /// @version 14.06.2020
    /// <summary>
    /// Scene to test worms in.
    /// </summary>
    /// <param name="scene">scene created in program.cs</param>
    /// <returns>scene back to program.cs</returns>
    class WormGame
    {
        public Scene Start(Scene scene)
        {
            Head worm = new Head(scene, 960, 540, 64, 3, 5);
            scene.Add(worm);
            return scene;
        }
    }
}
