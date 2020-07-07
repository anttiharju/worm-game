using Otter.Core;
using Otter.Utility.MonoGame;
using WormGame.Static;

namespace WormGame.Manager
{
    public class Config
    {

        public readonly int targetFramerate = 144;
        public readonly int size;
        public readonly int margin = 1;
        public readonly int width = 50;
        public readonly int height = 26;
        public readonly int maxWormLength = 4;
        public readonly float wormFreq = 0.1f;
        public readonly float brickFreq = 0.4f;

        public static bool visualizeCollision = false;

        public readonly int tailAmount;
        public readonly int wormAmount;

        public float WormSpeed { private set; get; }

        public Config(int windowWidth, int windowHeight)
        {
            int cellAmount = width * height;
            wormAmount = cellAmount / maxWormLength;
            tailAmount = cellAmount - wormAmount;
            size = CalculateSize(windowWidth, windowHeight);
        }

        /// <summary>
        /// Calculates entity size based on window and play area dimensions.
        /// </summary>
        /// <returns>Entity size</returns>
        private int CalculateSize(int windowWidth, int windowHeight)
        {
            int xSize = windowWidth / (width + margin * 2);
            int ySize = windowHeight / (height + margin * 2);
            int size = Mathf.Smaller(xSize, ySize);
            if (size % 2 != 0) size--;
            return size;
        }

        public void UpdateFirst(Game game)
        {
            WormSpeed = wormFreq;
        }
    }
}
