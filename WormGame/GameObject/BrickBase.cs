using Otter.Graphics.Drawables;
using WormGame.Other;

namespace WormGame.GameObject
{
    /// @author Antti Harju
    /// @version 01.07.2020
    /// <summary>
    /// Tail class. Acts as the foundation for the entire worm.
    /// </summary>
    class BrickBase : Poolable
    {
        /// <summary>
        /// Constructor. Creates a circle graphic for the entity.
        /// </summary>
        /// <param name="size">Circle graphic diameter</param>
        public BrickBase(int size) : base()
        {
            Image image = Image.CreateRectangle(size);
            AddGraphic(image);
            image.CenterOrigin();
        }
    }
}
