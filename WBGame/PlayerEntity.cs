using Otter;

namespace WBGame
{
    class PlayerEntity : Entity
    {
        public float MoveSpeed;
        public const float MoveSpeedSlow = 5;
        public const float MoveSpeedFast = 10;

        public PlayerEntity(float x, float y) : base(x, y)
        {
            var image = Image.CreateRectangle(32);
            AddGraphic(image);
            image.CenterOrigin();
            MoveSpeed = MoveSpeedSlow;
        }

        public void SetColor()
        {
            Graphic.Color = Color.Green;
        }

        public override void Update()
        {
            base.Update();

            if (Input.KeyPressed(Key.G))
                SetColor();

            if (Input.KeyDown(Key.W))
                Y -= MoveSpeed;

            if (Input.KeyDown(Key.S))
                Y += MoveSpeed;

            if (Input.KeyDown(Key.A))
                X -= MoveSpeed;

            if (Input.KeyDown(Key.D))
                X += MoveSpeed;

            if (Input.KeyPressed(Key.Space))
                if (MoveSpeed == MoveSpeedSlow)
                {
                    MoveSpeed = MoveSpeedFast;
                    Graphic.Color = Color.Red;
                }
                else
                {
                    MoveSpeed = MoveSpeedSlow;
                    Graphic.Color = Color.White;
                }
        }
    }
}
