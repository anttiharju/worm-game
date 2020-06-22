using Otter;

namespace WBGame.Pooling
{
    /// @author Antti Harju
    /// @version 21.06.2020
    /// <summary>
    /// Base class for all poolable entities
    /// </summary>
    public class Poolable : Entity
    {
        private bool enabled;

        public bool Enabled { get { return enabled; } }


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


        /// <summary>
        /// Doesn't really have anything to do with pooling, but since I pool everything it's okay to have this here
        /// </summary>
        public Color Color
        {
            get
            {
                return Graphic.Color ?? null;
            }
            set
            {
                Graphic.Color = value;
            }
        }
    }
}
