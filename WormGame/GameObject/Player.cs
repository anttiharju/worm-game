using Otter.Core;
using Otter.Graphics;
using Otter.Graphics.Drawables;
using WormGame.Static;
using WormGame.Pooling;

namespace WormGame.GameObject
{
    /// @author Antti Harju
    /// @version 28.07.2020
    /// <summary>
    /// Player class.
    /// </summary>
    public class Player : PoolableEntity
    {
        public readonly WormScene scene;

        private readonly int playerNumber;
        private readonly float playerSpeed = 0.05f;

        private Worm worm;
        private float xMovement;
        private float yMovement;


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
        /// Controls.
        /// </summary>
        public override void Update()
        {
            #region Input
            if (Input.ButtonPressed(7, playerNumber) || // Join game
                (playerNumber == 4 && Input.KeyPressed(Key.Space))) 
                Visible = true;
            if (!Visible) return;

            if (playerNumber == 4) // Keyboard
            {
                xMovement = 0;
                yMovement = 0;
                if (Input.KeyDown(Key.Up))
                    yMovement -= 100;
                if (Input.KeyDown(Key.Left))
                    xMovement -= 100;
                if (Input.KeyDown(Key.Down))
                    yMovement += 100;
                if (Input.KeyDown(Key.Right))
                    xMovement += 100;
                if (Input.KeyPressed(Key.Space))
                    Posess();
            }
            else // Gamepad
            {
                xMovement = Input.GetAxis(JoyAxis.X, playerNumber);
                yMovement = Input.GetAxis(JoyAxis.Y, playerNumber);

                if (Input.ButtonPressed(5, playerNumber)) // RB
                    Posess();
            }
            #endregion

            #region Player
            if (worm != null) goto Playerskip;
            float deadZone = 10;
            if (Mathf.FastAbs(xMovement) > deadZone)
                X += xMovement * playerSpeed;
            if (Mathf.FastAbs(yMovement) > deadZone)
                Y += yMovement * playerSpeed;
            Playerskip:;
            #endregion

            #region Worm
            if (worm == null) goto Wormskip;
            Position = worm.Position;
            deadZone = 90;
            if (yMovement < -deadZone)
                worm.Direction = Help.directions[0]; // UP
            if (xMovement < -deadZone)
                worm.Direction = Help.directions[1]; // LEFT
            if (yMovement > deadZone)
                worm.Direction = Help.directions[2]; // DOWN
            if (xMovement > deadZone)
                worm.Direction = Help.directions[3]; // RIGHT
            Wormskip:;
            #endregion;
        }
    }
}
