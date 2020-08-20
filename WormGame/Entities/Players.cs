using Otter.Core;
using Otter.Graphics;
using Otter.Graphics.Drawables;
using WormGame.Static;
using WormGame.Pooling;
using WormGame.Core;
using Otter.Utility.MonoGame;

namespace WormGame.Entities
{
    /// @author anvemaha
    /// @version 17.08.2020
    /// <summary>
    /// Custom pooler for players. Players are never really disabled, but I just bend the pooler to my will.
    /// </summary>
    public class Players : Pooler<Player>
    {
        /// <summary>
        /// Custom constructor.
        /// </summary>
        /// <param name="config">Configuration</param>
        /// <param name="scene">Worm scene</param>
        public Players(Config config, WormScene scene) : base(5)
        {
            for (int i = 0; i < 5; i++)
            {
                Player player = new Player(config, scene, i);
                player.Disable(false);
                player.Add(scene);
                pool[i] = player;
                Enable();
            }
        }


        /// <summary>
        /// Players aren't reset the same way other entities are, it's kind of hacky, but it works.
        /// </summary>
        public override void Reset()
        {
            foreach (Player player in pool)
                player.Disable();
        }
    }

    /// @author Antti Harju
    /// @version 30.07.2020
    /// <summary>
    /// Player class.
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
        /// <param name="config">Configuration</param>
        public Player(Config config, WormScene scene, int playerNumber)
        {
            this.scene = scene;
            this.playerNumber = playerNumber;
            spawnPosition = new Vector2(config.windowWidth / 2, config.windowHeight / 2);
            playerSpeed = 144.0f / config.refreshRate * 0.05f;
            Image image = Image.CreateCircle(config.size / 3);
            image.OutlineThickness = config.size / 15;
            image.OutlineColor = Color.Black;
            image.CenterOrigin();
            AddGraphic(image);
            int color = playerNumber;
            if (color >= Help.colors.Length)
                color = Help.colors.Length - 1;
            Graphic.Color = Help.colors[color];
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
        /// Listens to input.
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
            else
            {
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
            Position = worm.Target;
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
        /// <param name="recursive">Disable recursively. False only when disabling is done by pooler.</param>
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
