using WormGame.Static;

namespace WormGame.Core
{
    /// <summary>
    /// Configuration. Work in progress.
    /// </summary>
    public class Config
    {
#if DEBUG
        public bool visualizeCollision = true;
#endif
        public readonly int windowWidth = 1280;
        public readonly int windowHeight = 720;
        public readonly int refreshRate = 144;
        public readonly bool fullscreen = false;

        public readonly Collision field;
        public readonly int width = 7;
        public readonly int height = 7;
        public readonly int margin = 4;
        public readonly int size;

        public readonly int maxWormLength = 3;
        public readonly int density = 4;
        public readonly float wormSpeed = 6; // 12 supports 144hz, 120hz, 60hz, 6 supports those plus 30hz
        public readonly float step;

        public readonly float brickFreq = 0.4f;

        public readonly int brainAmount;
        public readonly int bodyAmount;

        public readonly int imageSize = 64; //52x25+4 results in size of 32 on 1080p so 64 should be enough to support 4k displays properly

        public Config()
        {
            bodyAmount = width * height;
            brainAmount = bodyAmount / maxWormLength;
            size = CalculateSize(windowWidth, windowHeight);
            System.Console.WriteLine(size);
            step = wormSpeed / refreshRate * size;
            field = new Collision(windowWidth, windowHeight, this);
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
    }
}
