using Otter;
using WBGame.Pooling;

namespace WBGame
{
    /// @author Antti Harju
    /// @version 14.06.2020
    /// <summary>
    /// Scene to test entity pooling in.
    /// </summary>
    /// <param name="scene">scene created in program.cs</param>
    /// <returns>scene back to program.cs</returns>
    class PoolGame
    {
        public Scene Start(Scene scene)
        {
            int width = 10;
            int height = 20;
            int cellSize = 32;
            int gap = 5;

            Pooler<Poolable> blocks = new Pooler<Poolable>(scene, width * height, cellSize);

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    blocks.Spawn(x * (cellSize + gap), y * (cellSize + gap));

            return scene;
        }
    }
}