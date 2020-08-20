using Otter.Core;
using Otter.Graphics;
using Otter.Graphics.Drawables;
using Otter.Utility.MonoGame;
using WormGame.Static;
using WormGame.Pooling;
using WormGame.Core;

namespace WormGame.Entities
{
    /// @author Antti Harju
    /// @version v0.5
    /// <summary>
    /// Custom pooler for players.
    /// </summary>
    public class Players : Pooler<Player>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="settings">Settings</param>
        /// <param name="scene">Scene</param>
        public Players(Settings settings, WormScene scene) : base(5)
        {
            for (int i = 0; i < 5; i++)
            {
                Player player = new Player(settings, scene, i + 1);
                player.Disable(false);
                player.Add(scene);
                pool[i] = player;
                Enable();
            }
        }
    }

    /// @author Antti Harju
    /// @version v0.5
    /// <summary>
    /// Player. Sort of a hacky pooled object as it's never really disabled outside of resets.
    /// </summary>
    public class Player : PoolableEntity
    {
        public readonly int playerNumber;

        private readonly WormScene scene;
        private readonly Vector2 spawnPosition;
        private readonly float playerSpeed;

        private Worm worm;
        private float xMovement;
        private float yMovement;


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="settings">Settings</param>
        /// <param name="scene">Scene</param>
        /// <param name="playerNumber">Player number (1-5)</param>
        public Player(Settings settings, WormScene scene, int playerNumber)
        {
            this.scene = scene;
            this.playerNumber = playerNumber;
            spawnPosition = new Vector2(settings.windowWidth / 2, settings.windowHeight / 2);
            playerSpeed = 144.0f / settings.refreshRate * 0.05f;
            int outlineThickness = settings.size / 7;
            Image image = Image.CreateCircle(settings.size / 2 - outlineThickness - 2);
            image.OutlineThickness = outlineThickness;
            image.OutlineColor = Colors.background; // or foregroundColor, depending on color palette
            image.CenterOrigin();
            AddGraphic(image);
            int color = playerNumber;
            if (color >= Colors.palette.Length)
                color = Colors.palette.Length - 1;
            Graphic.Color = Colors.palette[color];
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
                if (worm != null)
                    worm.Player = this;
            }
        }


        /// <summary>
        /// Listens to input and applies it to either player or worm.
        /// </summary>
        public override void Update()
        {
            #region Input
            if (!Visible && ((Input.ButtonPressed(5, playerNumber)) || // Join game
                (playerNumber == 5 && Input.KeyPressed(Key.Space))))
            {
                Visible = true;
                return;
            }
            else
            {
                if (playerNumber == 5) // Keyboard
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
            }
            #endregion

            #region Player
            if (worm != null) goto Playerskip;
            float deadZone = 10;
            if (FastMath.Abs(xMovement) > deadZone)
                X += xMovement * playerSpeed;
            if (FastMath.Abs(yMovement) > deadZone)
                Y += yMovement * playerSpeed;
            Playerskip:;
            #endregion

            #region Worm
            if (worm == null) goto Wormskip;
            Position = worm.firstModule.Position;
            deadZone = 90;
            if (yMovement < -deadZone)
                worm.Direction = Colors.directions[0]; // UP
            if (xMovement < -deadZone)
                worm.Direction = Colors.directions[1]; // LEFT
            if (yMovement > deadZone)
                worm.Direction = Colors.directions[2]; // DOWN
            if (xMovement > deadZone)
                worm.Direction = Colors.directions[3]; // RIGHT
            Wormskip:;
            #endregion;
        }


        /// <summary>
        /// Disable player.
        /// </summary>
        /// <param name="recursive">Disable recursively. Not relevant here.</param>
        public override void Disable(bool recursive = true)
        {
            SetPosition(spawnPosition);
            Visible = false;
            if (worm != null)
                worm.Player = null;
            worm = null;
        }
    }
}
