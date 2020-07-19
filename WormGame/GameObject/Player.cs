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
    /// Player class. Work in progress.
    /// </summary>
    /// TODO: Embrace pooling, separate player and ghost to separate classes.
    public class Player : PoolableEntity
    {
        public readonly WormScene scene;
        private readonly float speedModifier = 0.05f;
        private readonly int playerNumber;
        private readonly int axisDeadZone = 10;

        private Worm worm;
        private float leftX;
        private float leftY;
        private float dpadX;
        private float dpadY;
        private bool dropAction = true;


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="wormScene">Scene. Required because of NearestWorm()</param>
        /// <param name="playerNumber">Player number, 0 - 3</param>
        /// <param name="x">Horizontal position</param>
        /// <param name="y">Vertical position</param>
        /// <param name="color">Posessed entity color</param>
        /// <param name="size">Entity size</param>
        public Player(WormScene wormScene, int playerNumber, float x, float y, Color color, int size)
        {
            scene = wormScene;
            this.playerNumber = playerNumber;
            Image border = Image.CreateCircle(size / 3 + size / 15, Color.Black);
            Image image = Image.CreateCircle(size / 3, color);
            border.CenterOrigin();
            image.CenterOrigin();
            AddGraphic(border);
            AddGraphic(image);
            X = x;
            Y = y;
        }


        /// <summary>
        /// Handles player controlled posessing.
        /// </summary>
        private void Posess()
        {
            if (worm != null)
            {
                //worm.Color = oldColor;
                //Position = worm.Position;
                worm.Player = null;
                worm = null;
                //Graphic.Visible = true;
            }
            else
            {
                worm = scene.NearestWorm(Position, 250);
                if (worm == null) return;
                worm.Player = this;
                //Graphic.Visible = false;
                //oldColor = worm.Color;
                //worm.Color = playerColor;
            }
        }


        /// <summary>
        /// Worms controls.
        /// </summary>
        private void WormControl()
        {
            if (worm == null) return;
            Position = worm.Position;
            float deadZone = 90;
            if (leftY < -deadZone)
                worm.Direction = Help.directions[0]; // UP
            if (leftX < -deadZone)
                worm.Direction = Help.directions[1]; // LEFT
            if (leftY > deadZone)
                worm.Direction = Help.directions[2]; // DOWN
            if (leftX > deadZone)
                worm.Direction = Help.directions[3]; // RIGHT
        }


        /// <summary>
        /// Ghosts controls.
        /// </summary>
        private void GhostControl()
        {
            if (worm != null) return;
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
            Timers();
        }


        /// <summary>
        /// Timers to slow down inputs when holding dpad down.
        /// </summary>
        private void Timers()
        {
            if (-10 < dpadY && dpadY < 10 && -10 < dpadX && dpadX < 10)
                dropAction = true;
        }
    }
}
