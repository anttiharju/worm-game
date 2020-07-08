using WormGame.Static;

namespace WormGame.Core
{
    /// <summary>
    /// Configuration. Work in progress.
    /// </summary>
    public class Config
    {
        public readonly int windowWidth;
        public readonly int windowHeight;
        public readonly int targetFramerate = 144;

        public readonly Collision field;
        public readonly int width = 6;
        public readonly int height = 6;
        public readonly int margin = 1;

        public readonly int size;
        public readonly int density = 6;
        public readonly int maxWormLength = 19;
        public readonly float wormSpeed = 6;
        public readonly float wormStep;

        public readonly float brickFreq = 0.4f;
#if DEBUG
        public bool visualizeCollision = true;
#endif
        public readonly int brainAmount;
        public readonly int bodyAmount;

        public float WormSpeed { private set; get; }

        public Config(int windowWidth, int windowHeight)
        {
            this.windowWidth = windowWidth;
            this.windowHeight = windowHeight;
            int cellAmount = width * height;
            brainAmount = cellAmount / maxWormLength;
            bodyAmount = cellAmount - brainAmount;
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
