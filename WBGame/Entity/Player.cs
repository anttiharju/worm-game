using Otter;

namespace WBGame.Entity
{
    class Player : Otter.Entity
    {
        public float MoveSpeed;
        public const float MoveSpeedSlow = 5;
        public const float MoveSpeedFast = 10;

        public Player(float x, float y) : base(x, y)
        {
            var image = Image.CreateRectangle(32);
            AddGraphic(image);
            image.CenterOrigin();
            MoveSpeed = MoveSpeedSlow;
        }


        public override void Update()
        {
            base.Update();

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
