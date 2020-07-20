using WormGame.GameObject;
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
#if DEBUG
        public bool visualizeCollision = false; // 119x29 fits in debug console
#endif
        public readonly bool fullscreen = false;
        public readonly int windowWidth = 1280;
        public readonly int windowHeight = 720;
        public readonly int refreshRate = 144; // See wormSpeed before changing this
        public readonly int imageSize = 32;

        public readonly WormScene scene;
        public readonly Collision field;
        public readonly int width = 20;
        public readonly int height = 10;
        public readonly int margin = 2;

        // wormSpeed has to divide refreshRate evenly. (6 supports 144, 120, 60 and 30). If not, this will be subtracted by one until it is.
        public readonly int wormSpeed = 6;
        public readonly int minWormLength = 5;

        // Not loaded from settings.cfg (yet?)
        public readonly bool fruits = true;
        public readonly float fruitPercentage = 0.015f;
        public readonly int density = 5;

        // Dynamic values
        public readonly int fruitAmount;
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
                    case "minWormLength":
                        minWormLength = int.Parse(value);
                        break;
                    case "wormSpeed":
                        wormSpeed = int.Parse(value);
                        break;
                    case "brickFreq":
                        brickFreq = int.Parse(value);
                        break;
                }
            }
        Skip: // End file reading
#endif
            #endregion

            if (width < 2) width = 2;
            if (height < 2) height = 2;
            while (refreshRate % wormSpeed != 0)
                wormSpeed--;
            fruitAmount = (int)(width * height * fruitPercentage);
            if (fruitAmount < 1)
                fruitAmount = 1;
            moduleAmount = width * height;
            entityAmount = moduleAmount / minWormLength;
            size = CalculateSize(windowWidth, windowHeight);
            step = (float)wormSpeed / refreshRate * (float)size;
            field = new Collision(this);
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
