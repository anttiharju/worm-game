using Otter;
using WBGame.Pooling;
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
            Pooler<Body> bodyPool = new Pooler<Body>(scene, 20, 64);
            Pooler<Head> wormPool = new Pooler<Head>(scene, 4, 64);

            wormPool.TakeOne().Spawn(352, 256, bodyPool, 5, Color.Red);
            wormPool.TakeOne().Spawn(548, 256, bodyPool, 5, Color.Green);
            wormPool.TakeOne().Spawn(744, 256, bodyPool, 5, Color.Blue);
            wormPool.TakeOne().Spawn(940, 256, bodyPool, 5, Color.Yellow);

            return scene;
        }
    }
}
