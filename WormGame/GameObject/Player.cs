using Otter.Core;
using Otter.Graphics;
using Otter.Graphics.Drawables;
using WormGame.Static;
using WormGame.Pooling;

namespace WormGame.GameObject
{
    /// @author Antti Harju
    /// @version 18.07.2020
    /// <summary>
    /// Player class.
    /// </summary>
    public class Player : PoolableEntity
    {
        public readonly WormScene scene;

        private readonly int playerNumber;
        private readonly float playerSpeed = 0.05f;

        private Worm worm;
        private float leftX;
        private float leftY;


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="wormScene">WormScene, required for NearestWorm()</param>
        /// <param name="playerNumber">Determines color and joypad</param>
        /// <param name="x">Horizontal entity position</param>
        /// <param name="y">Vertical entity position</param>
        /// <param name="size">Entity size</param>
        public Player(WormScene wormScene, int playerNumber, float x, float y, int size)
        {
            Visible = false;
            scene = wormScene;
            this.playerNumber = playerNumber;
            int color = playerNumber;
            if (color >= Help.colors.Length)
                color = Help.colors.Length - 1;
            Image image = Image.CreateCircle(size / 3, Help.colors[color]);
            image.OutlineThickness = size / 15;
            image.OutlineColor = Color.Black;
            image.CenterOrigin();
            AddGraphic(image);
            X = x;
            Y = y;
        }


        /// <summary>
        /// Posess or unposess worm.
        /// </summary>
        private void Posess()
        {
            if (worm != null)
            {
                worm.Player = null;
                worm = null;
            }
            else
            {
                worm = scene.NearestWorm(Position, 250);
                if (worm == null) return;
                worm.Player = this;
            }
        }


        /// <summary>
        /// Player controls.
        /// </summary>
        public override void Update()
        {
            #region Join game
            if (Input.ButtonPressed(7, playerNumber)) // Start
                Visible = true;
            if (!Visible) return;
            #endregion

            #region Global controls
            leftX = Input.GetAxis(JoyAxis.X, playerNumber);
            leftY = Input.GetAxis(JoyAxis.Y, playerNumber);

            if (Input.ButtonPressed(5, playerNumber)) // RB
                Posess();
            #endregion

            #region Player controls
            if (worm != null) goto Playerskip;
            float deadZone = 10;
            if (Mathf.FastAbs(leftX) > deadZone)
                X += leftX * playerSpeed;
            if (Mathf.FastAbs(leftY) > deadZone)
                Y += leftY * playerSpeed;
            Playerskip:;
            #endregion

            #region Worm controls
            if (worm == null) goto Wormskip;
            Position = worm.Position;
            deadZone = 90;
            if (leftY < -deadZone)
                worm.Direction = Help.directions[0]; // UP
            if (leftX < -deadZone)
                worm.Direction = Help.directions[1]; // LEFT
            if (leftY > deadZone)
                worm.Direction = Help.directions[2]; // DOWN
            if (leftX > deadZone)
                worm.Direction = Help.directions[3]; // RIGHT
            if (Input.ButtonPressed(0, 0))
            {
                worm.Blockify();
            }
        Wormskip:;
            #endregion;
        }
    }
}
