using Otter;
using System;
using System.Timers;
using WormGame.Other;

namespace WormGame.GameObject
{
    // TODO: Generalize things, now this is a unholy mess full of if's
    // ^ i'll do that later on as I don't know how much things will evolve from this
    class Player : Poolable
    {
        private readonly WormScene manager;
        private readonly Color playerColor;
        private readonly int playerNumber;
        private readonly int axisDeadZone = 10;
        private readonly float speedModifier = 0.05f;
        private readonly float dropTimerReset = 0.3f;

        private Worm worm = null;
        private Bunch bunch = null;
        private Color oldColor;

        private float leftX;
        private float leftY;
        private float dpadX;
        private float dpadY;
        private float dropTimer;
        private bool dropAction = true;

        public Player(WormScene wormGame, int playerNumber, float x, float y, Color playerColor, int size)
        {
            this.manager = wormGame;
            this.playerNumber = playerNumber;
            this.playerColor = playerColor;
            Image image = Image.CreateRectangle(size / 2, size, playerColor);
            AddGraphic(image);
            image.CenterOrigin();
            X = x;
            Y = y;
        }

        private void Posess()
        {
            if (bunch != null)
            {
                bunch.Color = oldColor;
                Position = bunch.Position;
                bunch = null;
                Graphic.Visible = true;
            }
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
            bunch.Color = worm.Color;
            worm.Disable();
            worm = null;
        }

        private void BunchControl()
        {
            if (bunch == null) return;

            float dpadDeadZone = 80;

            // D-pad
            if (Mathf.FastAbs(dpadX) > dpadDeadZone)
            {
                if (dpadX < 0)
                {
                }
                if (dpadX > 0)
                {
                }
            }
            if (dropAction)
            {
                if (Mathf.FastAbs(dpadY) > dpadDeadZone) //up hard, down soft, horizontal move
                {
                    if (dpadY > 0)
                    {
                        bunch.SoftDrop();
                        dropAction = false;
                    }
                    if (dpadY < 0)
                    {
                        bunch.HardDrop();
                        dropAction = false;
                    }
                }
            }

            if (Input.ButtonPressed(0, playerNumber)) // A rotate counterclockwise
            {
                bunch.Rotate();
            }
            if (Input.ButtonPressed(1, playerNumber)) // B rotate clockwise
            {
                bunch.Rotate(true);
            }
            if (Input.ButtonPressed(4, playerNumber)) // LB hold
            {
                Blockify();
            }
            if (Input.ButtonPressed(5, playerNumber)) // RB hold
            {
                Posess();
            }
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
            if (worm != null || bunch != null) return;
            float deadZone = 10;
            if (Mathf.FastAbs(leftX) > deadZone)
            {
                X += leftX * speedModifier;
            }
            if (Mathf.FastAbs(leftY) > deadZone)
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

            GhostControl();
            WormControl();
            BunchControl();
            Timers();
        }

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
