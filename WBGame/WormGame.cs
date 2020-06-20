using Otter;
using WBGame.Pooling;
using WBGame.Worm;
using WBGame.Other;

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
            Manager manager = new Manager(scene, 120, 6, 1000, 32);

            Head worm = manager.SpawnWorm(500, 500, 5, Color.Red);
            manager.SpawnPlayer(worm);
            
            return scene;
        }
    }
}
