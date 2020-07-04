using Otter;
using WormGame.Help;
using WormGame.Other;

namespace WormGame.GameObject
{
    /// @author Antti Harju
    /// @version 01.07.2020
    /// <summary>
    /// Tail class. Acts as the foundation for the entire worm.
    /// </summary>
    class Brick : Poolable
    {
        /// <summary>
        /// Constructor. Creates a circle graphic for the entity.
        /// </summary>
        /// <param name="size">Circle graphic diameter</param>
        public Brick(int size) : base()
        {
            Image image = Image.CreateRectangle(size);
            AddGraphic(image);
            image.CenterOrigin();
            Enabled = false;
        }

        /*
        /// <summary>
        /// Recursive method that returns the lowest vertical block position in the bunch.
        /// </summary>
        /// <param name="biggest"></param>
        /// <returns>Lowest vertical block position in the bunch</returns>
        public float Lowest(float biggest)
        {
            if (NextBlock != null)
                biggest = NextBlock.Lowest(Mathf.Bigger(Y, biggest));
            return biggest;
        }*/
    }
}
