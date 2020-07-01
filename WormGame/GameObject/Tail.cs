using Otter;
using WormGame.Other;

namespace WormGame.GameObject
{
    /// @author Antti Harju
    /// @version 01.07.2020
    /// <summary>
    /// Tail class. Acts as the foundation for the entire worm. Managed by worm class (which is the head of the worm).
    /// </summary>
    class Tail : Poolable
    {
        public Tail NextTail { get; set; }
        public Vector2 Next { get; set; }
        
        public struct Target
        {
            public float X { get; set; }
            public float Y { get; set; }
        }

        /// <summary>
        /// Constructor. Creates a circle graphic for the entity.
        /// </summary>
        /// <param name="size">Circle graphic diameter</param>
        public Tail(int size) : base()
        {
            Image image = Image.CreateCircle(size / 2);
            AddGraphic(image);
            image.CenterOrigin();
        }


        /// <summary>
        /// Recursive method that makes every part of the tail follow the head
        /// </summary>
        /// <param name="newPosition">Position to move to</param>
        public void TailFollow(Vector2 newPosition)
        {
            if (NextTail != null)
                NextTail.TailFollow(Next);
            Next = newPosition;
        }


        /// <summary>
        /// Recursively moves the entire worm
        /// </summary>
        /// <param name="xDelta">horizontal movement</param>
        /// <param name="yDelta">vertical movement</param>
        public void Move(float xDelta, float yDelta)
        {
            NextTail.TailFollow(Next);
            Next += new Vector2(xDelta, yDelta);
            Point point = new Point();
            point.X += (int)xDelta;
        }


        /// <summary>
        /// Tweening for the individual worm parts
        /// </summary>
        public override void Update()
        {
            Position += (Next - Position) * 0.15f * (144 / Scene.Game.TargetFramerate);
        }


        /// <summary>
        /// Disables the worm. Should only be used by Worm.Disable().
        /// </summary>
        /// <param name="playArea">Play area</param>
        public void Disable(PlayArea playArea)
        {
            if (NextTail != null)
                NextTail.Disable();
            Enabled = false;
            playArea.Update(Next, null);
        }


        /// <summary>
        /// Recursively changes the whole worms color
        /// </summary>
        /// <param name="color">Worms new color</param>
        public void SetColor(Color color)
        {
            if (NextTail != null)
                NextTail.SetColor(color);
            Graphic.Color = color;
        }
    }
}
