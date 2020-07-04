namespace WormGame.Help
{
    class Config
    {
        public static readonly int targetFramerate = 144;

        public static readonly int margin = 2;
        public static readonly int width = 64;
        public static readonly int height = 32;
        public static readonly int maxWormLength = 4;

        public static bool visualizeCollision = false;

        public readonly int tailAmount;
        public readonly int wormAmount;

        public Config()
        {
            int cellAmount = width * height;
            wormAmount = cellAmount / maxWormLength;
            tailAmount = cellAmount - wormAmount;
        }
    }
}
