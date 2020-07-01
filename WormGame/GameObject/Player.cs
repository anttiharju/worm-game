using Otter;
using WormGame.Other;
using WormGame.Help;

namespace WormGame.GameObject
{
    /// <summary>
    /// Player class (Ghost class). Very much work in progress.
    /// </summary>
    class Player : Poolable
    {
        private readonly WormScene wormScene;
        private readonly Color playerColor;
        private readonly int playerNumber;
        private readonly int axisDeadZone = 10;
        private readonly float speedModifier = 0.05f;
        private readonly float dropTimerReset = 0.3f;

        private Worm worm;
        private Bricks bricks;
        private Color oldColor;

        private float leftX;
        private float leftY;
        private float dpadX;
        private float dpadY;
        private float dropTimer;
        private bool dropAction = true;


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="wormScene">Scene where the player should be</param>
        /// <param name="playerNumber">1-4</param>
        /// <param name="x">Ghost horizontal position</param>
        /// <param name="y">Ghost vertical position</param>
        /// <param name="playerColor">Ghost color</param>
        /// <param name="size">Ghost size</param>
        public Player(WormScene wormScene, int playerNumber, float x, float y, Color playerColor, int size)
        {
            this.wormScene = wormScene;
            this.playerNumber = playerNumber;
            this.playerColor = playerColor;
            Image image = Image.CreateRectangle(size / 2, size, playerColor);
            AddGraphic(image);
            image.CenterOrigin();
            X = x;
            Y = y;
        }


        /// <summary>
        /// Handles posessing at different states of being
        /// </summary>
        private void Posess()
        {
            if (bricks != null)
            {
                bricks.Color = oldColor;
                Position = bricks.Position;
                bricks = null;
                Graphic.Visible = true;
            }
            if (worm != null)
            {
                worm.Color = oldColor;
                Position = worm.Position;
                worm.Posessed = false;
                worm = null;
                Graphic.Visible = true;
            }
            else
            {
                worm = wormScene.NearestWorm(Position, 250);
                if (worm == null) return;
                worm.Posessed = true;
                Graphic.Visible = false;
                oldColor = worm.Color;
                worm.Color = playerColor;
            }
        }


        /// <summary>
        /// Kills the posessed worm
        /// </summary>
        private void KillWorm()
        {
            worm.Disable();
            worm = null;
        }


        /// <summary>
        /// Turns a worm into a collection of bricks
        /// </summary>
        private void Brickify()
        {
            if (worm == null) return;
            worm.Disable();
            bricks = wormScene.Brickify(worm);
            bricks.Color = worm.Color;
            worm = null;
        }


        /// <summary>
        /// Bricks' controls
        /// </summary>
        private void BrickControl()
        {
            if (bricks == null) return;

            float dpadDeadZone = 80;

            if (dropAction)
                if (Mathf.FastAbs(dpadY) > dpadDeadZone) //up hard, down soft, horizontal move
                {
                    if (dpadY > 0)
                    {
                        bricks.SoftDrop();
                        dropAction = false;
                    }
                    if (dpadY < 0)
                    {
                        bricks.HardDrop();
                        dropAction = false;
                    }
                }

            if (Input.ButtonPressed(0, playerNumber)) // A
                bricks.Rotate();
            if (Input.ButtonPressed(1, playerNumber)) // B
                bricks.Rotate(true);

            if (Input.ButtonPressed(4, playerNumber)) // LB
                Brickify();

            if (Input.ButtonPressed(5, playerNumber)) // RB
                Posess();
        }


        /// <summary>
        /// Worms controls
        /// </summary>
        private void WormControl()
        {
            if (worm == null) return;
            float deadZone = 90;
            if (leftX < -deadZone)
                worm.Direction = "LEFT";
            if (leftX > deadZone)
                worm.Direction = "RIGHT";
            if (leftY < -deadZone)
                worm.Direction = "UP";
            if (leftY > deadZone)
                worm.Direction = "DOWN";

            if (Input.ButtonPressed(3, playerNumber)) // Y
                KillWorm();
        }


        /// <summary>
        /// Ghosts controls
        /// </summary>
        private void GhostControl()
        {
            if (worm != null || bricks != null) return;
            float deadZone = 10;
            if (Mathf.FastAbs(leftX) > deadZone)
                X += leftX * speedModifier;
            if (Mathf.FastAbs(leftY) > deadZone)
                Y += leftY * speedModifier;
        }


        /// <summary>
        /// Examples of all kinds of Input. Work in progress.
        /// </summary>
        public override void Update()
        {
            #region Mandatory
            base.Update();
            leftX = Input.GetAxis(JoyAxis.X, playerNumber);
            leftY = Input.GetAxis(JoyAxis.Y, playerNumber);
            float rightX = Input.GetAxis(JoyAxis.U, playerNumber);
            float rightY = Input.GetAxis(JoyAxis.V, playerNumber);
            dpadX = Input.GetAxis(JoyAxis.PovX, playerNumber);
            dpadY = Input.GetAxis(JoyAxis.PovY, playerNumber);
            float triggers = Input.GetAxis(JoyAxis.Z, playerNumber);
            #endregion

            #region Axises
            // Right stick
            if (Mathf.FastAbs(rightX) > axisDeadZone)
            {

            }
            if (Mathf.FastAbs(rightY) > axisDeadZone)
            {

            }
            // Triggers (shared)
            if (Mathf.FastAbs(triggers) > axisDeadZone)
            {
                if (triggers > 0) // Left
                {

                }
                if (triggers < 0) // Right
                {

                }
            }
            #endregion

            #region Buttons
            if (Input.ButtonPressed(0, playerNumber)) // A
            {

            }
            if (Input.ButtonPressed(1, playerNumber)) // B
            {

            }
            if (Input.ButtonPressed(2, playerNumber)) // X
            {

            }
            if (Input.ButtonPressed(3, playerNumber)) // Y
            {

            }
            if (Input.ButtonPressed(4, playerNumber)) // LB
            {
                Brickify();
            }
            if (Input.ButtonPressed(5, playerNumber)) // RB
            {
                Posess();
            }
            if (Input.ButtonPressed(6, playerNumber)) // Back
            {

            }
            if (Input.ButtonPressed(7, playerNumber)) // Start
            {

            }
            if (Input.ButtonPressed(8, playerNumber)) // L3
            {

            }
            if (Input.ButtonPressed(9, playerNumber)) // R3
            {

            }
            #endregion

            GhostControl();
            WormControl();
            BrickControl();
            Timers();
        }


        /// <summary>
        /// Timers to slow down inputs when holding dpad down
        /// </summary>
        private void Timers()
        {
            if (-10 < dpadY && dpadY < 10)
                dropAction = true;
            if (!dropAction)
                dropTimer += Scene.Game.DeltaTime;
            if (dropTimer >= dropTimerReset)
            {
                dropAction = true;
                dropTimer = 0;
            }
        }
    }
}
