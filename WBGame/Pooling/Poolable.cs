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


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="size">Size of the entity</param>
        public Poolable(float x, float y) : base(x, y) { }


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
        public void Spawn(float x, float y)
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


        public void SetColor(Color color)
        {
            Graphic.Color = color;
        }
    }

    #region Interface
    /// <summary>
    /// Interface so we can have a generic pooler for all kinds of entities
    /// </summary>
    interface IPoolable
    {
        bool Available();
        void Spawn(float x, float y);
        void Destroy();
    }
    #endregion
}
