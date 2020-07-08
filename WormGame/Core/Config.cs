using WormGame.Static;

namespace WormGame.Core
{
    /// <summary>
    /// Configuration. Work in progress.
    /// </summary>
    public class Config
    {
#if DEBUG
        public bool visualizeCollision = false;
#endif
        public readonly int windowWidth;
        public readonly int windowHeight;
        public readonly int targetFramerate = 144;

        public readonly Collision field;
        public readonly int width = 64;
        public readonly int height = 32;
        public readonly int margin = 1;
        public readonly int size;

        public readonly int maxWormLength = 4;
        public readonly int density = 4;
        public readonly float wormSpeed = 12; // 60/12 = 5, 120/12 = 10, 144/12 = 12, we won't support 30fps because 6 is too slow
        public readonly float wormStep;

        public readonly float brickFreq = 0.4f;

        public readonly int brainAmount;
        public readonly int bodyAmount;

        public Config(int windowWidth, int windowHeight)
        {
            this.windowWidth = windowWidth;
            this.windowHeight = windowHeight;
            bodyAmount = width * height;
            brainAmount = bodyAmount / maxWormLength;
            size = CalculateSize(windowWidth, windowHeight);
            wormStep = wormSpeed / targetFramerate * size;
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
