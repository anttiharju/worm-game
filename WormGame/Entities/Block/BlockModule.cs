using Otter.Graphics.Drawables;
using WormGame.Core;
using WormGame.Static;
using WormGame.Pooling;
using Otter.Graphics;

namespace WormGame.Entities
{
    /// @author Antti Harju
    /// @version 06.08.2020
    /// <summary>
    /// BlockModule. Scaled as needed by Block.
    /// </summary>
    public class BlockModule : Poolable
    {
        private readonly Collision collision;
        private readonly int size;
        private readonly int halfSize;

        private int startX;
        private int startY;


        /// <summary>
        /// Graphic.
        /// </summary>
        public Image Graphic { get; private set; }


        /// <summary>
        /// Next module.
        /// </summary>
        public BlockModule Next { get; set; }


        /// <summary>
        /// Enable the entity.
        /// </summary>
        public override bool Active { get { return active; } set { if (value) { active = true; Graphic.Visible = true; } else Disable(); } }
        private bool active;


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="config">Configuration.</param>
        public BlockModule(Config config)
        {
            collision = config.collision;
            size = config.size;
            halfSize = config.halfSize;
            Graphic = Image.CreateRectangle(config.size);
            Graphic.SetOrigin(0, config.size);
        }


        /// <summary>
        /// Spawn module.
        /// </summary>
        /// <param name="parent">Block</param>
        /// <param name="x">Relative entity position to firstModule</param>
        /// <param name="y">Relative vertical entity position to firstModule</param>
        /// <returns>Module</returns>
        public BlockModule Spawn(Block parent, float x, float y)
        {
            Graphic.SetPosition(x - halfSize, y + halfSize);
            Graphic.Color = parent.Color;
            parent.AddGraphic(Graphic);
            startX = collision.X(parent.X + x);
            startY = collision.Y(parent.Y + y);
            return this;
        }


        /// <summary>
        /// Recursively disables all modules and removes them from collision.
        /// </summary>
        /// <param name="recursive">Disable recursively. False only when disabling is done by pooler.</param>
        public override void Disable(bool recursive = true)
        {
            base.Disable();
            active = false;
            if (recursive && Next != null)
                Next.Disable();
            Next = null;
            int endX = startX + FastMath.Round(Graphic.ScaleX);
            int endY = startY - FastMath.Round(Graphic.ScaleY);
            for (int x = startX; x < endX; x++)
                for (int y = startY; y > endY; y--)
                    collision.Set(null, x, y);
            Graphic.Scale = 1;
        }
    }
}
