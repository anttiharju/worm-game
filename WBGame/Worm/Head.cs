using Otter;
using WBGame.Pooling;

namespace WBGame.Worm
{
    /// @author Antti Harju
    /// @version 12.06.2020
    /// <summary>
    /// Worm class, creates as long a tail as needed and controls the head
    /// </summary>
    class Head : Body
    {
        private int currentLength;

        /// <summary>
        /// Constructor. Creates the head of the worm (this entity) and the entities for its tail. Also calls the body constructor.
        /// </summary>
        /// <param name="scene">scene where the worm exists, required for tail creation</param>
        /// <param name="x">horizontal position</param>
        /// <param name="y">vertical position</param>
        /// <param name="size">how thick the worm is (worm is made out of circles and this the diameter of the circle)</param>
        /// <param name="speed">how fast the worm is</param>
        /// <param name="wantedLength">how long a worm is</param>
        public Head(int x, int y, int size) : base(x, y, size) { }

        public void Spawn(float x, float y, Pooler<Body> bodyPool, int wantedLength, Color color)
        {
            Position = new Vector2(x, y);
            SetTarget(x, y);
            SetColor(color);
            currentLength = --wantedLength; //-- because head is already 1

            Body currentBody = this;
            for (int i = 0; i < currentLength; i++)
            {
                Body tmpBody = bodyPool.TakeOne(x, y);
                tmpBody.Position = new Vector2(x, y);
                tmpBody.SetTarget(x, y);
                tmpBody.SetColor(color);
                currentBody.SetNextBody(tmpBody);
                currentBody = tmpBody;
            }
        }

        #region Movement
        /// <summary>
        /// Worm controls are set up here
        /// </summary>
        public override void Update()
        {
            if (Available()) return;
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
            if (Input.KeyPressed(key))
                MoveWorm(x, y);
        }
        #endregion
    }
}
