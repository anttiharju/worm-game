using Otter.Core;
using Otter.Graphics;
using Otter.Graphics.Drawables;
using WormGame.Static;
using WormGame.Pooling;
using WormGame.Core;

namespace WormGame.Entity
{
    /// @author Antti Harju
    /// @version 30.07.2020
    /// <summary>
    /// Player class.
    /// </summary>
    public class Player : PoolableEntity
    {
        private readonly float playerSpeed = 0.05f;

        private Worm worm;
        private float xMovement;
        private float yMovement;
        private int playerNumber;

        public WormScene scene;


        /// <summary>
        /// Overriding default behaviour due to how player joining works
        /// </summary>
        public override bool Enabled { get; set; }


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="config">Configuration</param>
        public Player(Config config)
        {
            Image image = Image.CreateCircle(config.size / 3);
            image.OutlineThickness = config.size / 15;
            image.OutlineColor = Color.Black;
            image.CenterOrigin();
            AddGraphic(image);
        }


        /// <summary>
        /// Spawns player.
        /// </summary>
        /// <param name="x">Horizontal entity position.</param>
        /// <param name="y">Vertical entity position</param>
        /// <returns>Player</returns>
        public Player Spawn(int playerNumber, float x, float y)
        {
            scene = (WormScene)Scene;
            this.playerNumber = playerNumber;
            int color = playerNumber;
            if (color >= Help.colors.Length)
                color = Help.colors.Length - 1;
            Graphic.Color = Help.colors[color];
            SetPosition(x, y);
            Visible = false;
            return this;
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
            if (!Visible && ((Input.ButtonPressed(5, playerNumber)) || // Join game
                (playerNumber == 4 && Input.KeyPressed(Key.Space))))
            {
                Visible = true;
                return;
            }
            if (!Visible) return;

            if (playerNumber == 4) // Keyboard
            {
                xMovement = 0;
                yMovement = 0;
                if (Input.KeyDown(Key.W))
                    yMovement -= 100;
                if (Input.KeyDown(Key.A))
                    xMovement -= 100;
                if (Input.KeyDown(Key.S))
                    yMovement += 100;
                if (Input.KeyDown(Key.D))
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


        /// <summary>
        /// Disable player.
        /// </summary>
        public override void Disable()
        {
            Enabled = false;
            Visible = false;
            worm = null;
        }
    }
}
