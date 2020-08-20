using Otter.Graphics.Drawables;
using WormGame.Static;

namespace WormGame.Core
{
    /// @author Antti Harju
    /// @version v0.5
    /// <summary>
    /// Settings.
    /// </summary>
    public class Settings
    {
        // Misc
        public readonly Collision collision;
        public readonly Surface surface;
        public readonly Tilemap tilemap;
#if DEBUG
        // Debug
        public readonly bool visualizeCollision = false;
        public readonly bool visualizeBlockSpawner = false; // Shouldn't print anything else than dots.
#endif
        // Gamerules
        public readonly bool disableBlocks = true;
        public readonly bool disableWorms = true;          // If false, set spawnFruits to false.

        // Window
        public readonly int windowWidth = 1280;
        public readonly int windowHeight = 720;
        public readonly int refreshRate = 60;              // See wormSpeed before changing this
        public readonly bool fullscreen = false;

        // Grid
        public readonly int width = 64;
        public readonly int height = 36;
        public readonly int margin = 3;

        // Worm
        public readonly float wormPercentage = 1;
        public readonly int wormCap = -1;                   // Overrides wormPercentage if positive.
        public readonly int wormSpeed = 12;                 // wormSpeed has to divide refreshRate evenly. (6 supports 144, 120, 60 and 30).
        public readonly int minWormLength = 6;

        // Fruit
        public readonly float fruitPercentage = 0.015f;
        public readonly bool spawnFruits = true;

        #region Calculated variables
        // Amounts
        public readonly int fruitAmount;
        public readonly int wormAmount;
        public readonly int warningAmount;
        public readonly int moduleAmount;

        // Misc
        public readonly float step;
        public readonly int size;
        public readonly int halfSize;

        // Scene
        public readonly int leftBorder;
        public readonly int topBorder;
        #endregion

        /// <summary>
        /// Read values from settings.cfg, sanitize them, calculate pooler capacities and calculate dynamic values.
        /// </summary>
        public Settings()
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
                    // Gamerules
                    case "disableBlocks":
                        disableBlocks = bool.Parse(value);
                        break;
                    case "disableWorms":
                        disableWorms = bool.Parse(value);
                        break;
                    // Window
                    case "windowWidth":
                        windowWidth = int.Parse(value);
                        break;
                    case "windowHeight":
                        windowHeight = int.Parse(value);
                        break;
                    case "refreshRate":
                        refreshRate = int.Parse(value);
                        break;
                    case "fullscreen":
                        fullscreen = bool.Parse(value);
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
                    case "wormPercentage":
                        wormPercentage = float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
                        break;
                    case "wormCap":
                        wormCap = int.Parse(value);
                        break;
                    case "wormSpeed":
                        wormSpeed = int.Parse(value);
                        break;
                    case "minWormLength":
                        minWormLength = int.Parse(value);
                        break;
                    // Fruits
                    case "fruitPercentage":
                        fruitPercentage = float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
                        break;
                    case "spawnFruits":
                        spawnFruits = bool.Parse(value);
                        break;
                }
            }
        Fileskip:
#endif
            #endregion

            // Input sanitization
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

            if (wormCap >= 0)
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
            step = (float)wormSpeed / refreshRate * size;
            collision = new Collision(this);
            surface = new Surface(windowWidth, windowHeight, Colors.background)
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
        /// Calculates entity size so that the whole field fits in the window. Minimum size is 2 so fields too large will just expand outside display.
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
            if (size < 2) size = 2;
            return size;
        }
    }
}
