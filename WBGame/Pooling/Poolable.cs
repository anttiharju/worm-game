using Otter;

namespace WBGame.Pooling
{
    /// @author Antti Harju
    /// @version 15.6.2020
    /// <summary>
    /// Base class for all poolable entities
    /// </summary>
    public class Poolable : Entity
    {
        private bool enabled;

        /// <summary>
        /// Constructor.
        /// </summary>
        public Poolable() { }


        /// <summary>
        /// So we don't pick entities already in use
        /// </summary>
        /// <returns>Wheter or not the entity is enabled</returns>
        public bool Enabled()
        {
            return enabled;
        }


        /// <summary>
        /// Enables the entity
        /// </summary>
        public Poolable Enable()
        {
            Graphic.Visible = true;
            enabled = true;
            return this;
        }


        /// <summary>
        /// Disables the entity
        /// </summary>
        public void Disable()
        {
            Graphic.Visible = false;
            enabled = false;
        }
    }
}
