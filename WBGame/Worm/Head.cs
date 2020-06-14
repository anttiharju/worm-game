using Otter;
using SFML.System;

namespace WBGame.Worm
{
    /// @author Antti Harju
    /// @version 12.06.2020
    /// <summary>
    /// Worm class, creates as long a tail as needed and controls the head
    /// </summary>
    class Head : Body
    {
        private int wormLength;
        private int i = 0;

        #region Constructor
        /// <summary>
        /// Constructor. Creates the head of the worm (this entity) and the entities for its tail. Also calls the body constructor.
        /// </summary>
        /// <param name="scene">scene where the worm exists, required for tail creation</param>
        /// <param name="x">horizontal position</param>
        /// <param name="y">vertical position</param>
        /// <param name="size">how thick the worm is (worm is made out of circles and this the diameter of the circle)</param>
        /// <param name="speed">how fast the worm is</param>
        /// <param name="wormLength">how long a worm is</param>
        public Head(Scene scene, float x, float y, int size, float speed, int wormLength) : base(x, y, size, speed)
        {
            this.wormLength = --wormLength; //-- because head is already 1

            Body currentBody = this;
            for (int i = 0; i < this.wormLength; i++)
            {
                Body nextBody = new Body(x, y, size, speed);
                currentBody.SetNextBody(nextBody);
                scene.Add(nextBody);
                currentBody = nextBody;
            }
        }
        #endregion


        #region Movement
        /// <summary>
        /// Worm controls are set up here
        /// </summary>
        public override void Update()
        {
            base.Update();
            /*
            Move(Key.W, 0, -GetSize(), Color.Green);
            Move(Key.S, 0, GetSize(), Color.Red);
            Move(Key.A, -GetSize(), 0, Color.Blue);
            Move(Key.D, GetSize(), 0, Color.Yellow);
            */
            string route = "00000222221111133333";
            if (i >= route.Length) i = 0;
            char c = route[i];
            if (c == '0')
            {
                MoveWorm(0, -GetSize());
                SetColor(Color.Green);
            }
            if (c == '1')
            {
                MoveWorm(0, GetSize());
                SetColor(Color.Red);
            }
            if (c == '2')
            {
                MoveWorm(-GetSize(), 0);
                SetColor(Color.Blue);
            }
            if (c == '3')
            {
                MoveWorm(GetSize(), 0);
                SetColor(Color.Yellow);
            }
            i++;
        }


        /// <summary>
        /// Moves the worm when a key is pressed and changes color of the worms head
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="x">horizontal movement</param>
        /// <param name="y">vertical movement</param>
        /// <param name="color">color of the worms head when the key is pressed</param>
        private void Move(Key key, float x, float y, Color color)
        {
            if (Input.KeyPressed(key))
            {
                SetColor(color);
                MoveWorm(x, y);
            }
        }
        #endregion
    }
}
