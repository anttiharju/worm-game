using Otter;

namespace WBGame.Entity
{
    class Square : Otter.Entity
    {
        private int phase = 0;

        public Square(float x, float y, int size = 32) : base(x, y)
        {
            Image image = Image.CreateRectangle(size);
            AddGraphic(image);
            image.CenterOrigin();
        }


        public void SetColor(Color color)
        {
            Graphic.Color = color;
        }


        public int GetPhase()
        {
            return phase;
        }


        public void SetPhase(int phase)
        {
            this.phase = phase;
        }


        public void IncreasePhase()
        {
            this.phase++;
        }
    }
}
