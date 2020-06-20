using Otter;
using WBGame.Pooling;
using WBGame.Worm;
using WBGame.Misc;

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
            Pooler<Body> bodyPool = new Pooler<Body>(scene, 120, 32);
            Pooler<Head> headPool = new Pooler<Head>(scene, 6, 32);
            Pooler<Block> blockPool = new Pooler<Block>(scene, 1000, 32);

            headPool.Next().Spawn(true, 256 + 64 * 0, 256, 4, bodyPool, headPool, blockPool, Color.Red);
            headPool.Next().Spawn(true, 256 + 64 * 3, 256, 4, bodyPool, headPool, blockPool, Color.Green);
            headPool.Next().Spawn(true, 256 + 64 * 6, 256, 4, bodyPool, headPool, blockPool, Color.Blue);
            headPool.Next().Spawn(true, 256 + 64 * 9, 256, 4, bodyPool, headPool, blockPool, Color.Yellow);
            headPool.Next().Spawn(true, 256 + 64 * 12, 256, 4, bodyPool, headPool, blockPool, Color.Cyan);
            headPool.Next().Spawn(true, 256 + 64 * 15, 256, 4, bodyPool, headPool, blockPool, Color.Orange);
            
            return scene;
        }
    }
}
