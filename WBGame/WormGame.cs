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
            Pooler<Body> bodyPool = new Pooler<Body>(scene, 44, 32);
            Pooler<Head> headPool = new Pooler<Head>(scene, 4, 32);

            headPool.Next().Spawn(true, 256 + 64 * 0, 256, 10, bodyPool, headPool, Color.Red);
            headPool.Next().Spawn(true, 256 + 64 * 3, 256, 10, bodyPool, headPool, Color.Green);
            headPool.Next().Spawn(true, 256 + 64 * 6, 256, 10, bodyPool, headPool, Color.Blue);
            headPool.Next().Spawn(true, 256 + 64 * 9, 256, 10, bodyPool, headPool, Color.Yellow);

            return scene;
        }
    }
}
