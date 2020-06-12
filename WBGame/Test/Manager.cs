using Otter;

namespace WBGame.Test
{
    class Manager : Otter.Entity
    {
        private Square[] squares;
        private Color[] colors = { Color.Red, Color.Green, Color.Blue, Color.Yellow };
        private int i = 0;

        public Manager(Square[] squares)
        {
            this.squares = new Square[squares.Length];
            this.squares = squares;
        }

        public override void Update()
        {
            base.Update();

            if (squares[i].GetPhase() >= colors.Length)
                squares[i].SetPhase(0);

            int p = squares[i].GetPhase();
            squares[i].SetColor(colors[squares[i].GetPhase()]);
            squares[i].IncreasePhase();

            i++;
            if (i >= squares.Length)
                i = 0;
        }
    }
}
