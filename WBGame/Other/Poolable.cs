using Otter;

namespace WBGame.Other
{
    /// @author anvemaha
    /// @version 28.06.2020
    /// <summary>
    /// Class that all poolable entities inherit. Production ready, although may get new features.
    /// </summary>
    public class Poolable : Entity
    {
        /// <summary>
        /// Improves code readability.
        /// </summary>
        public bool Enabled { get { return Graphic.Visible; } set { Graphic.Visible = value; } }


        /// <summary>
        /// Not related to pooling, but useful. Overridable so multi-entity entities can also use this.
        /// </summary>
        public virtual Color Color { get { return Graphic.Color ?? null; } set { Graphic.Color = value; } }
    

        /// <summary>
        /// Improves code readability.
        /// </summary>
        public void Disable()
        {
            Enabled = false;
        }
    }
}