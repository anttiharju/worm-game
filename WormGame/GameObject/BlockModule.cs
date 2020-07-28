using Otter.Graphics.Drawables;
using WormGame.Core;
using WormGame.Pooling;

namespace WormGame.GameObject
{
    /// @author Antti Harju
    /// @version 28.07.2020
    /// <summary>
    /// BlockModule. Scaled as needed by Block.
    /// </summary>
    /// TODO: Removal from collision field on disable.
    public class BlockModule : PoolableObject
    {
        private readonly int size;
        private readonly int halfSize;


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
        public override bool Enabled { get { return enabled; } set { enabled = value; Graphic.Visible = value; } }
        private bool enabled;


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="config">Configuration.</param>
        public BlockModule(Config config)
        {
            size = config.size;
            halfSize = size / 2;
            Graphic = Image.CreateRectangle(config.size);
            Graphic.Scale = 0;
        }


        /// <summary>
        /// Spawn module.
        /// </summary>
        /// <param name="parent">Block</param>
        /// <param name="x">Horizontal field position</param>
        /// <param name="y">Vertical field position</param>
        /// <returns>Module</returns>
        public BlockModule Spawn(Block parent, float x, float y)
        {
            Graphic.Scale = 1;
            Graphic.SetOrigin(0, size);
            Graphic.SetPosition(x - halfSize, y + halfSize);
            parent.AddGraphic(Graphic);
            Graphic.Color = parent.Color;
            return this;
        }


        /// <summary>
        /// Disable module.
        /// </summary>
        public override void Disable()
        {
            if (Next != null)
                Next.Disable();
            Enabled = false;
            Graphic.SetPosition(0, 0);
            Graphic.SetOrigin(0, 0);
            Graphic.Scale = 0;
        }
    }
}
