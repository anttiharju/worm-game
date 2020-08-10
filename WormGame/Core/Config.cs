using WormGame.Static;

namespace WormGame.Core
{
    /// @author Antti Harju
    /// @version 30.07.2020
    /// <summary>
    /// Configuration.
    /// </summary>
    public class Config
    {
        // Misc
        public readonly WormScene scene;
        public readonly Collision collision;
#if DEBUG
        public bool visualizeCollision = true;   // 119x29 fits in debug console
        public bool disableBlocks = true;         // For benchmarking purposes
#endif

        // Window
        public readonly bool fullscreen = false;
        public readonly int windowWidth = 1024;
        public readonly int windowHeight = 720;
        public readonly int refreshRate = 144;    // See wormSpeed before changing this

        // Scene
        public readonly int width = 20;
        public readonly int height = 10;
        public readonly int margin = 3;

        // Worm
        public readonly int wormCap = 0;          // Overrides wormPercentage if > 0.
        public readonly int wormSpeed = 6;       // wormSpeed has to divide refreshRate evenly. (6 supports 144, 120, 60 and 30).
        public readonly int minWormLength = 3;
        public readonly float wormPercentage = 0.1f;

        // Fruit
        public readonly bool fruits = true;
        public readonly float fruitPercentage = 0.01f;

        // Dynamic values
        public readonly int fruitAmount;
        public readonly int wormAmount;
        public readonly int warningAmount;
        public readonly int entityAmount;
        public readonly int moduleAmount;
        public readonly float step;
        public readonly int size;


        /// <summary>
        /// Calculates dynamic values and initializes collision field.
        /// </summary>
        public Config()
        {
            #region File reading
#if !DEBUG
            string path = System.AppDomain.CurrentDomain.BaseDirectory + "settings.cfg";
            string[] lines;
            try
            {
                lines = System.IO.File.ReadAllLines(path);
            }
            catch (System.IO.FileNotFoundException)
            {
                goto Fileskip;
            }

            foreach (string line in lines)
            {
                if (line[0] == '#') continue;
                int equalIndex = line.IndexOf('=');
                int endStartIndex = equalIndex + 2;
                int endIndex = line.IndexOf('#');
                if (endIndex == -1)
                    endIndex = line.Length;
                string name = line.Substring(0, equalIndex).Trim();
                string value = line.Substring(endStartIndex, endIndex - endStartIndex).Trim();
                switch (name)
                {
                    // Window
                    case "fullscreen":
                        fullscreen = bool.Parse(value);
                        break;
                    case "windowWidth":
                        windowWidth = int.Parse(value);
                        break;
                    case "windowHeight":
                        windowHeight = int.Parse(value);
                        break;
                    case "refreshRate":
                        refreshRate = int.Parse(value);
                        break;
                    // Scene
                    case "width":
                        width = int.Parse(value);
                        break;
                    case "height":
                        height = int.Parse(value);
                        break;
                    case "margin":
                        margin = int.Parse(value);
                        break;
                    // Worm
                    case "wormSpeed":
                        wormSpeed = int.Parse(value);
                        break;
                    case "minWormLength":
                        minWormLength = int.Parse(value);
                        break;
                    case "wormPercentage":
                        wormPercentage = float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
                        break;
                    case "wormCap":
                        wormCap = int.Parse(value);
                        break;
                    // Fruits
                    case "fruits":
                        fruits = bool.Parse(value);
                        break;
                    case "fruitPercentage":
                        fruitPercentage = float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
                        break;
                }
            }
        Fileskip:
#endif
            #endregion

            // Safeguards
            if (windowWidth < 800) windowWidth = 800;
            if (windowHeight < 800) windowHeight = 600;
            if (width < 2) width = 2;
            if (height < 2) height = 2;
            if (minWormLength < 1) minWormLength = 1;
            if (wormSpeed < 1) wormSpeed = 1;
            while (refreshRate % wormSpeed != 0)
                wormSpeed--;

            // Pooler amounts
            fruitAmount = (int)(width * height * fruitPercentage);
            if (fruitAmount < 1)
                fruitAmount = 1;

            moduleAmount = width * height;
            entityAmount = moduleAmount / minWormLength;

            if (wormCap > 0)
                wormAmount = wormCap;
            else
            {
                wormAmount = Mathf.FastRound(moduleAmount / minWormLength * wormPercentage);
                wormCap = wormAmount;
            }
            wormAmount *= 2; // Every worm might turn into a block simultaneously

            // Other
            size = CalculateSize(windowWidth, windowHeight);
            step = (float)wormSpeed / refreshRate * size;
            collision = new Collision(this);
            scene = new WormScene(this);
        }


        /// <summary>
        /// Calculates a entity size so that the whole field fits in the window.
        /// </summary>
        /// <param name="windowWidth">Window width</param>
        /// <param name="windowHeight">Window height</param>
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
