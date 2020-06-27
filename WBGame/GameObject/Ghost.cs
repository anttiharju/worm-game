using Otter;
using WBGame.Other;

namespace WBGame.GameObject
{
    class Ghost : Poolable
    {
        private readonly int playerNumber;
        private readonly int axisDeadZone = 10;
        private readonly float speedModifier = 0.05f;

        public Ghost(int playerNumber, int size, Color color)
        {
            this.playerNumber = playerNumber;
            Image image = Image.CreateRectangle(size / 2, size, color);
            AddGraphic(image);
            image.CenterOrigin();
        }

        public override void Update()
        {
            #region Mandatory
            base.Update();
            float leftX = Input.GetAxis(JoyAxis.X, playerNumber);
            float leftY = Input.GetAxis(JoyAxis.Y, playerNumber);
            float rightX = Input.GetAxis(JoyAxis.U, playerNumber);
            float rightY = Input.GetAxis(JoyAxis.V, playerNumber);
            float dpadX = Input.GetAxis(JoyAxis.PovX, playerNumber);
            float dpadY = Input.GetAxis(JoyAxis.PovY, playerNumber);
            float triggers = Input.GetAxis(JoyAxis.Z, playerNumber);
            #endregion

            #region Axises
            // Left stick
            if (Helper.FastAbs(leftX) > axisDeadZone)
            {
                X += leftX * speedModifier;
            }
            if (Helper.FastAbs(leftY) > axisDeadZone)
            {
                Y += leftY * speedModifier;
            }

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

            }
            if (Input.ButtonPressed(5, playerNumber)) // RB
            {

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
        }
    }
}
