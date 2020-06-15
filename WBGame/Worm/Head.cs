using Otter;
using WBGame.Pooling;

namespace WBGame.Worm
{
    /// @author Antti Harju
    /// @version 15.06.2020
    /// <summary>
    /// The worm class
    /// </summary>
    class Head : Body
    {
        private int currentLength;
        private bool posessed = false;
        private Pooler<Body> bodyPool;
        private Pooler<Head> headPool;

        /// <summary>
        /// Head constructor. The x and y don't really matter because we have Spawn()
        /// </summary>
        /// <param name="size">Diameter of the circle graphic</param>
        public Head(int size) : base(size) { }

        /// <summary>
        /// Spawns the worm with the desired configuration. Can't be done in constructor because of how I decided to implement the pooling system.
        /// </summary>
        /// <param name="posessed">Is the worm controlled by a player or not</param>
        /// <param name="x">Horizontal position</param>
        /// <param name="y">Vertical position</param>
        /// <param name="wantedLength">How long the worm should be</param>
        /// <param name="bodyPool">Required for spawning the tail and collision</param>
        /// <param name="headPool">Required for collision</param>
        /// <param name="color">Worms color</param>
        public void Spawn(bool posessed, float x, float y, int wantedLength, Pooler<Body> bodyPool, Pooler<Head> headPool, Color color)
        {
            this.bodyPool = bodyPool;
            this.headPool = headPool;
            this.posessed = posessed;
            Position = new Vector2(x, y);
            SetTarget(x, y);
            SetColor(color);
            currentLength = --wantedLength; // minus because head is already 1

            Body currentBody = this;
            for (int i = 0; i < currentLength; i++)
            {
                Body tmpBody = bodyPool.Next();
                tmpBody.Position = new Vector2(x, y);
                tmpBody.SetTarget(x, y);
                tmpBody.SetColor(color);
                currentBody.SetNextBody(tmpBody);
                currentBody = tmpBody;
            }
        }

        /// <summary>
        /// Worm controls are set up here
        /// </summary>
        public override void Update()
        {
            if (!Enabled() || !posessed) return;
            base.Update();

            Move(Key.W, 0, -GetSize());
            Move(Key.S, 0, GetSize());
            Move(Key.A, -GetSize(), 0);
            Move(Key.D, GetSize(), 0);
        }


        /// <summary>
        /// Moves the worm when a key is pressed and changes color of the worms head
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="x">horizontal movement</param>
        /// <param name="y">vertical movement</param>
        /// <param name="color">color of the worms head when the key is pressed</param>
        private void Move(Key key, float x, float y)
        {
            Vector2 newPosition = Position + new Vector2(x, y);
            foreach (Body body in bodyPool)
                if (RoughlyEquals(body.Position, newPosition, GetSize() * 0.9f))
                    return;
            foreach (Head head in headPool)
                if (RoughlyEquals(head.Position, newPosition, GetSize() * 0.9f))
                    return;

            if (Input.KeyPressed(key))
                MoveWorm(x, y);
        }


        /// <summary>
        /// Area two Vector2s roughly equal (collision)
        /// TODO: Move to a helper class
        /// </summary>
        /// <param name="a">First Vector2</param>
        /// <param name="b">Second Vector2</param>
        /// <param name="errorMargin">Accuracy</param>
        /// <returns></returns>
        private bool RoughlyEquals(Vector2 a, Vector2 b, float errorMargin)
        {
            if (RoughlyEquals(a.X, b.X, errorMargin))
                if (RoughlyEquals(a.Y, b.Y, errorMargin))
                    return true;
            return false;
        }


        /// <summary>
        /// Are two floats roughly equal
        /// </summary>
        /// <param name="a">First float</param>
        /// <param name="b">Second float</param>
        /// <param name="errorMargin">Accuracy</param>
        /// <returns></returns>
        private bool RoughlyEquals(float a, float b, float errorMargin)
        {
            if (b - errorMargin < a && b + errorMargin > a)
                return true;
            return false;
        }
    }
}
