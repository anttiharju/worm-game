using Otter;
using WBGame.Other;

namespace WBGame.GameObject
{
    /// @author Antti Harju
    /// @version 21.06.2020
    /// <summary>
    /// The base worm class that by itself acts as the tail entity.
    /// </summary>
    class Tail : Poolable
    {
        private Tail nextBody;
        private Vector2 targetPosition;


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
        /// This method moves the worms head and starts a recursive call that moves its tail
        /// </summary>
        /// <param name="xDelta">horizontal movement</param>
        /// <param name="yDelta">vertical movement</param>
        /// TODO: This should be in Worm.cs?
        public void MoveWorm(float xDelta, float yDelta)
        {
            nextBody.TailFollow(GetTarget());
            SetTarget(GetTarget() + new Vector2(xDelta, yDelta));
        }


        /// <summary>
        /// Required by the SetColor() of Worm.cs
        /// </summary>
        /// <param name="color">Worms new color</param>
        public void SetColor(Color color)
        {
            if (nextBody != null)
                nextBody.SetColor(color);
            Graphic.Color = color;
        }


        /// <summary>
        /// Recursive method that makes every part of the tail follow the head
        /// </summary>
        /// <param name="newPosition">Position to move to</param>
        public void TailFollow(Vector2 newPosition)
        {
            if (nextBody != null)
                nextBody.TailFollow(GetTarget());
            targetPosition = newPosition;
        }


        public Tail[] GetBodies(Tail[] bodies, int i = 0)
        {
            if (nextBody != null)
                nextBody.GetBodies(bodies, i + 1);
            bodies[i] = this;
            return bodies;
        }


        /// <summary>
        /// Makes sure the entity is where it's supposed to be.
        /// </summary>
        public override void Update()
        {
            if (Enabled)
                Position += (GetTarget() - Position) * 0.2f;
        }


        /// <summary>
        /// Sets a new target position
        /// </summary>
        /// <param name="newPosition">New target position</param>
        public void SetTarget(Vector2 newPosition)
        {
            targetPosition = newPosition;
        }


        /// <summary>
        /// Sets a new target position
        /// </summary>
        /// <param name="x">Horizontal target position</param>
        /// <param name="y">Vertical target position</param>
        public void SetTarget(float x, float y)
        {
            SetTarget(new Vector2(x, y));
        }


        /// <summary>
        /// Sets the nextBody field. Required to make the tail follow with a recursive method.
        /// </summary>
        /// <param name="nextBody">The body that is after this one</param>
        public void SetNextBody(Tail nextBody)
        {
            this.nextBody = nextBody;
        }


        /// <summary>
        /// Worm has to know where it's supposed to be
        /// </summary>
        /// <returns>The positions where this part of the worm is supposed to be</returns>
        public Vector2 GetTarget()
        {
            return targetPosition;
        }


        /// <summary>
        /// Required for the recursive method to make the tail follow
        /// </summary>
        /// <returns>Next body of the worm</returns>
        public Tail GetNextBody()
        {
            return nextBody;
        }
    }
}
