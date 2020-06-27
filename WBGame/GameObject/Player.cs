using Otter;
using WBGame.Other;

namespace WBGame.GameObject
{
    class Player : Poolable
    {
        private readonly int playerNumber;
        private readonly Color playerColor;
        private readonly int axisDeadZone = 10;
        private readonly float speedModifier = 0.05f;

        private Manager manager;
        private Worm worm = null;
        private Bunch bunch = null;
        private Color oldColor;

        private float leftX;
        private float leftY;

        public Player(Manager manager, int playerNumber, Color playerColor, int size)
        {
            this.manager = manager;
            this.playerNumber = playerNumber;
            this.playerColor = playerColor;
            Image image = Image.CreateRectangle(size / 2, size, playerColor);
            AddGraphic(image);
            image.CenterOrigin();
        }

        private void Posess()
        {
            if (worm == null)
            {
                worm = manager.NearestWorm(Position, 250);
                if (worm == null) return;
                Graphic.Visible = false;
                oldColor = worm.Color;
                worm.Color = playerColor;
            }
            else
            {
                worm.Color = oldColor;
                Position = worm.Position;
                worm = null;
                Graphic.Visible = true;
            }
        }

        private void Blockify()
        {
            if (worm == null) return;
            bunch = manager.Blockify(worm);
            Worm tmp = worm;
            Posess();
            tmp.Disable();
        }

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
        }

        private void GhostControl()
        {
            if (worm != null) return;
            float deadZone = 10;
            if (Helper.FastAbs(leftX) > deadZone)
            {
                X += leftX * speedModifier;
            }
            if (Helper.FastAbs(leftY) > deadZone)
            {
                Y += leftY * speedModifier;
            }
        }

        public override void Update()
        {
            #region Mandatory
            base.Update();
            leftX = Input.GetAxis(JoyAxis.X, playerNumber);
            leftY = Input.GetAxis(JoyAxis.Y, playerNumber);
            float rightX = Input.GetAxis(JoyAxis.U, playerNumber);
            float rightY = Input.GetAxis(JoyAxis.V, playerNumber);
            float dpadX = Input.GetAxis(JoyAxis.PovX, playerNumber);
            float dpadY = Input.GetAxis(JoyAxis.PovY, playerNumber);
            float triggers = Input.GetAxis(JoyAxis.Z, playerNumber);
            #endregion

            #region Axises

            // Right stick
            if (Helper.FastAbs(rightX) > axisDeadZone)
            {

            }
            if (Helper.FastAbs(rightY) > axisDeadZone)
            {

            }

            // D-pad
            if (Helper.FastAbs(dpadX) > axisDeadZone)
            {

            }
            if (Helper.FastAbs(dpadY) > axisDeadZone)
            {

            }

            // Triggers (shared)
            if (Helper.FastAbs(triggers) > axisDeadZone)
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
                Blockify();
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

            WormControl();
            GhostControl();
        }
    }
}
