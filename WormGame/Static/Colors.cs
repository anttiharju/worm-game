using Otter.Graphics;
using Otter.Utility.MonoGame;

namespace WormGame.Static
{
    /// @author Antti Harju
    /// @version v0.5
    /// <summary>
    /// Static class for color related things.
    /// </summary>
    public static class Colors
    {
#if DEBUG // Debug console message color is alternated between color and gray with this. Is this the rare case where a global variable is okay?
        public static bool colorfulMessage = true;
#endif
        // https://coolors.co/1a535c-4ecdc4-f7fff7-ff6b6b-ffe66d
        public static readonly Color[] palette = { new Color("1A535C"), new Color("F7FFF7"), new Color("4ECDC4"), new Color("FF6B6B"), new Color("FFE66D") };
        public static readonly Color background = palette[0];
        public static readonly Color foreground = palette[1];
        public static readonly Vector2[] directions = { new Vector2(0, -1), new Vector2(-1, 0), new Vector2(0, 1), new Vector2(1, 0) };


        /// <summary>
        /// Function to check if two colors are the same. Otter didn't seem to have one so I made this.
        /// </summary>
        /// <param name="a">Color a</param>
        /// <param name="b">Color b</param>
        /// <returns>Are the two colors the same (ignores alpha)</returns>
        public static bool Equal(Color a, Color b)
        {
            if (a.R == b.R && a.G == b.G && a.B == b.B)
                return true;
            return false;
        }
    }
}
