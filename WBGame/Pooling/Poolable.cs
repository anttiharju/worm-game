using Otter;

namespace WBGame.Pooling
{
    /// @author Antti Harju
    /// @version 14.06.2020
    /// <summary>
    /// Class that all poolable entities should inherit
    /// </summary>
    class Poolable : Entity, IPoolable
    {
        private bool alive;


        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="size">Size of the entity</param>
        /// TODO: Don't actually use this constructor for the pooled entities (so we can have rectangles, circles, all kinds of shapes and keep this generic)
        public Poolable(int size) : base(0, 0)
        {
            Image image = Image.CreateCircle(size / 2);
            AddGraphic(image);
            image.CenterOrigin();
            Destroy();
        }
        #endregion


        #region Methods
        /// <summary>
        /// Tells wheter or not the entity is in use.
        /// </summary>
        /// <returns>Wheter or not the entity is in use</returns>
        public bool Available()
        {
            return !alive;
        }


        /// <summary>
        /// Spawns the entity at the desired position
        /// </summary>
        /// <param name="x">horizontal position</param>
        /// <param name="y">vertical position</param>
        public void Spawn(int x, int y)
        {
            Graphic.Visible = true;
            Position = new Vector2(x, y);
            alive = true;
        }


        /// <summary>
        /// Puts the entity back into the pool
        /// </summary>
        public void Destroy()
        {
            Graphic.Visible = false;
            alive = false;
        }
        #endregion
    }

    #region Interface
    /// <summary>
    /// Interface so we can have a generic pooler for all kinds of entities
    /// </summary>
    interface IPoolable
    {
        bool Available();
        void Spawn(int x, int y);
        void Destroy();
    }
    #endregion
}
