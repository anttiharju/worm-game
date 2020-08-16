using Otter.Graphics;
using Otter.Graphics.Drawables;
using WormGame.Static;

namespace WormGame.Core
{
    /// @author Antti Harju
    /// @version 14.08.2020
    /// <summary>
    /// Configuration.
    /// </summary>
    public class Config
    {
        // Misc
        public readonly Collision collision;
        public readonly Tilemap tilemap;
        public readonly Surface surface;
        public readonly Color backgroundColor = Color.Black;
        public readonly Color foregroundColor = Color.White;
#if DEBUG
        public readonly bool visualizeCollision = false;
        public readonly bool visualizeBlockifying = false;
        public readonly bool disableBlocks = true;
        public readonly bool blockifyWorms = true;
#endif

        // Window
        public readonly bool fullscreen = false;
        public readonly int windowWidth = 1280;
        public readonly int windowHeight = 720;
        public readonly int refreshRate = 144;    // See wormSpeed before changing this

        // Scene
        public readonly int width = 20;
        public readonly int height = 10;
        public readonly int margin = 1;

        // Worm
        public readonly int wormCap = 0;           // Overrides wormPercentage if > 0.
        public readonly int wormSpeed = 6;         // wormSpeed has to divide refreshRate evenly. (6 supports 144, 120, 60 and 30).
        public readonly int minWormLength = 6;
        public readonly float wormPercentage = 0.2f;

        // Fruit
        public readonly bool spawnFruits = true;
        public readonly float fruitPercentage = 0.015f;

        #region Calculated variables
        // Amounts
        public readonly int fruitAmount;
        public readonly int wormAmount;
        public readonly int warningAmount;
        public readonly int moduleAmount;

        // Misc
        public readonly float wormStep;
        public readonly int size;
        public readonly int halfSize;

        // Scene
        public readonly int leftBorder;
        public readonly int topBorder;
        #endregion

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
            if (windowHeight < 600) windowHeight = 600;
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

            if (wormCap > 0)
                wormAmount = wormCap;
            else
            {
                wormAmount = FastMath.Round(moduleAmount / minWormLength * wormPercentage);
                wormCap = wormAmount;
            }

            // Other
            size = CalculateSize(windowWidth, windowHeight);
            halfSize = size / 2;
            leftBorder = windowWidth / 2 - width / 2 * size;
            topBorder = windowHeight / 2 - height / 2 * size;
            if (width % 2 == 0) leftBorder += size / 2;
            if (height % 2 == 0) topBorder += size / 2;
            wormStep = (float)wormSpeed / refreshRate * size;
            collision = new Collision(this);
            surface = new Surface(windowWidth, windowHeight, backgroundColor)
            {
                AutoClear = false
            };
            tilemap = new Tilemap(width * size, height * size, size, size)
            {
                X = leftBorder - halfSize,
                Y = topBorder - halfSize
            };
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
            int size;
            if (xSize < ySize) size = xSize;
            else size = ySize;
            if (size % 2 != 0) size--;
            return size;
        }
    }
}
