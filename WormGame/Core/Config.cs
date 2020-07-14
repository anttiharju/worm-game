using WormGame.Static;

namespace WormGame.Core
{
    /// <summary>
    /// Configuration.
    /// </summary>
    public class Config
    {
#if DEBUG
        public bool visualizeCollision = false;
#endif
        public readonly bool fullscreen = false;
        public readonly int windowWidth = 1280;
        public readonly int windowHeight = 720;
        public readonly int refreshRate = 144;
        public readonly int imageSize = 64; // 52x25+4 results in size of 32 on 1080p so 64 should be large enough to support 4k displays properly

        public readonly WormScene scene;
        public readonly Collision field;
        public readonly int width = 52; // both width and height should fullfill (n % 2) == 0
        public readonly int height = 24;
        public readonly int margin = 4;

        public readonly int maxWormLength = 5;
        public readonly int wormSpeed = 6; // 6 because 144, 120, 60 and 30 are evenly dividable by it
        public readonly int brickFreq = 4;
        public readonly int density = 4;

        // Dynamic values
        public readonly int fruitAmount;
        public readonly int brainAmount;
        public readonly int bodyAmount;
        public readonly float step;
        public readonly int size;


        /// <summary>
        /// Calculates dynamic values and initializes collision field.
        /// </summary>
        public Config()
        {
            if (brickFreq % 2 != 0)
                brickFreq++;
            fruitAmount = Mathf.FastRound(Mathf.Bigger(width, height)) / 3;
            bodyAmount = width * height;
            brainAmount = bodyAmount / maxWormLength;
            size = CalculateSize(windowWidth, windowHeight);
            step = (float)wormSpeed / refreshRate * size;
            field = new Collision(windowWidth, windowHeight, this);
            scene = new WormScene(this);
        }


        /// <summary>
        /// Calculates entity size based on window and collision field dimensions.
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
