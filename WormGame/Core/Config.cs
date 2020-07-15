using System;
using System.IO;
using WormGame.Static;

namespace WormGame.Core
{
    /// <summary>
    /// Configuration.
    /// </summary>
    /// TODO: There's still some bugs with scaling that might cause stack overflow
    public class Config
    {
#if DEBUG
        public bool visualizeCollision = true;
#endif
        public readonly bool fullscreen = false;
        public readonly int windowWidth = 1280;
        public readonly int windowHeight = 720;
        public readonly int refreshRate = 144;
        public readonly int imageSize = 64; // 52x24+4 results in size of 32 on 1080p so 64 should be large enough to support 4k displays properly

        public readonly WormScene scene;
        public readonly Collision field;
        public readonly int width = 14; // both width and height should fullfill (n % 2) == 0
        public readonly int height = 14;
        public readonly int margin = 4;

        public readonly int maxWormLength = 5;
        public readonly int wormSpeed = 6; // 6 because 144, 120, 60 and 30 are evenly dividable by it
        public readonly int brickFreq = 4;
        public readonly int density = 8;

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
            #region File reading
#if !DEBUG
            string path = AppDomain.CurrentDomain.BaseDirectory + "settings.cfg";
            string[] lines;
            try
            {
                lines = File.ReadAllLines(path);
            }
            catch (FileNotFoundException)
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
                switch (name)
                {
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
                    case "imageSize":
                        imageSize = int.Parse(value);
                        break;
                    case "width":
                        width = int.Parse(value);
                        break;
                    case "height":
                        height = int.Parse(value);
                        break;
                    case "margin":
                        margin = int.Parse(value);
                        break;
                    case "maxWormLength":
                        maxWormLength = int.Parse(value);
                        break;
                    case "wormSpeed":
                        wormSpeed = int.Parse(value);
                        break;
                    case "brickFreq":
                        brickFreq = int.Parse(value);
                        break;
                }
            }
        Skip:
            // End file reading
#endif
            #endregion
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
