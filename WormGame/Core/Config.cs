using System;
using System.Globalization;
using WormGame.Static;

namespace WormGame.Core
{
    /// @author Antti Harju
    /// @version 18.07.2020
    /// <summary>
    /// Configuration.
    /// </summary>
    public class Config
    {
        // Misc
        public readonly WormScene scene;
        public readonly Collision collision;
#if DEBUG
        public bool visualizeCollision = true; // 119x29 fits in debug console
#endif
        // Window
        public readonly bool fullscreen = false;
        public readonly int windowWidth = 1280;
        public readonly int windowHeight = 720;
        public readonly int refreshRate = 144; // See wormSpeed before changing this

        // Scene
        public readonly int width = 20;
        public readonly int height = 10;
        public readonly int margin = 1;

        // Worm
        public readonly int wormSpeed = 24; // wormSpeed has to divide refreshRate evenly. (6 supports 144, 120, 60 and 30).
        public readonly int minWormLength = 5;
        public readonly float wormSpawnDuration = 2.2f;
        public readonly float wormPercentage = 0.025f;
        public readonly int wormCap = 0; // # Overrides wormPercentage if > 0.

        // Fruit
        public readonly bool fruits = true;
        public readonly float fruitPercentage = 0.025f;

        // Dynamic values
        public readonly int fruitAmount;
        public readonly int wormAmount;
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
                goto Skip;
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
                switch (name) // 
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
                    case "wormSpawnDuration":
                        wormSpawnDuration = float.Parse(value, CultureInfo.InvariantCulture);
                        break;
                    case "wormPercentage":
                        wormPercentage = float.Parse(value, CultureInfo.InvariantCulture);
                        break;
                    case "wormCap":
                        wormCap = int.Parse(value);
                        break;
                    // Fruits
                    case "fruits":
                        fruits = bool.Parse(value);
                        break;
                    case "fruitPercentage":
                        fruitPercentage = float.Parse(value, CultureInfo.InvariantCulture);
                        break;
                }
            }
        Skip: // End file reading
#endif
            #endregion

            if (width < 2) width = 2;
            if (height < 2) height = 2;
            if (wormSpeed < 1) wormSpeed = 1;
            while (refreshRate % wormSpeed != 0) wormSpeed--;
            fruitAmount = (int)(width * height * fruitPercentage);
            if (fruitAmount < 1) fruitAmount = 1;
            if (wormCap <= 0)
            {
                float tmp = width * height * wormPercentage;
                wormCap = (int)(tmp + 1);
                if (wormAmount < 1)
                    wormAmount = 1;
            }
            wormAmount = wormCap * 2;  // Although rare, there's a chance that every worm turns into a block simultaneously, that's why * 2.
            moduleAmount = width * height;
            entityAmount = moduleAmount / minWormLength;
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
