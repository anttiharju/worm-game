using Otter;
using WBGame.Pooling;

namespace WBGame.Worm
{
    /// @author Antti Harju
    /// @version 15.06.2020
    /// <summary>
    /// Body class for the worm object. Head class ("the worm") also inherits this.
    /// </summary>
    class Body : Poolable
    {
        private Body nextBody;
        private Vector2 targetPosition;
        private int speed = 3;
        private int size;


        /// <summary>
        /// Constructor. Creates a circle graphic for the entity.
        /// </summary>
        /// <param name="size">Circle graphic diameter</param>
        /// <param name="speed">How fast the worm is</param>
        public Body(int size) : base()
        {
            this.size = size;
            Image image = Image.CreateCircle(size / 2);
            AddGraphic(image);
            image.CenterOrigin();
        }


        /// <summary>
        /// This method moves the worms head and starts a recursive call that moves its tail
        /// </summary>
        /// <param name="xDelta">horizontal movement</param>
        /// <param name="yDelta">vertical movement</param>
        public void MoveWorm(float xDelta, float yDelta)
        {
            nextBody.TailFollow(GetTarget());
            SetTarget(GetTarget() + new Vector2(xDelta, yDelta));
        }


        /// <summary>
        /// Recursive method that makes every part of the tail follow the head
        /// </summary>
        /// <param name="newPosition">position to move to</param>
        public void TailFollow(Vector2 newPosition)
        {
            if (nextBody != null)
                nextBody.TailFollow(GetTarget());
            targetPosition = newPosition;
        }


        /// <summary>
        /// Makes sure the entity is where it's supposed to be.
        /// TODO: This is expensive? Not necessary to always calculate?
        /// </summary>
        public override void Update()
        {
            base.Update();
            Position += (GetTarget() - Position) * 0.1f * GetSpeed();
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
        /// <param name="newPosition">New target position</param>
        public void SetTarget(float x, float y)
        {
            SetTarget(new Vector2(x, y));
        }


        /// <summary>
        /// Sets the nextBody field. Required to make the tail follow with a recursive method.
        /// </summary>
        /// <param name="nextBody">The body that is after this one</param>
        public void SetNextBody(Body nextBody)
        {
            this.nextBody = nextBody;
        }


        public void SetColor(Color color)
        {
            Graphic.Color = color;
        }


        /// <summary>
        /// Required to set new target positions and to calculate velocity for transitions between positions
        /// </summary>
        /// <returns>Size / thickness (diameter of the circle)</returns>
        public int GetSize()
        {
            return size;
        }


        /// <summary>
        /// Required to calculate velocity for transitions between positions
        /// </summary>
        /// <returns>Worms speed</returns>
        public float GetSpeed()
        {
            return speed;
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
        public Body GetNextBody()
        {
            return nextBody;
        }
    }
}
