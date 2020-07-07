using Otter.Core;
using Otter.Graphics;

namespace WormGame.Manager
{
    /// @author Antti Harju
    /// @version 28.06.2020
    /// <summary>
    /// Class that all poolable entities must inherit.
    /// </summary>
    public class Poolable : Entity
    {
        /// <summary>
        /// Improves code readability.
        /// </summary>
        public bool Enabled { get { return Graphic.Visible; } set { Graphic.Visible = value; } }


        /// <summary>
        /// Not related to pooling, but useful. Overridable so syntax remains the same with multi-entity entities (worms, bunches).
        /// </summary>
        public virtual Color Color { get { return Graphic.Color ?? null; } set { Graphic.Color = value; } }
    

        /// <summary>
        /// Improves code readability. Overridable so syntax remains the same with multi-entity entities (worms, bunches).
        /// </summary>
        public virtual void Disable()
        {
            Enabled = false;
        }

        public override void Update()
        {
            base.Update();
        }
    }
}