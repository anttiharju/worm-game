using Otter;
using System;
using WormGame.GameObject;

namespace WormGame.Other
{
    /// @author anvemaha
    /// @version 29.06.2020
    /// <summary>
    /// Takes care of collision
    /// </summary>
    class Collision
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int Size { get; private set; }

        private readonly Game game;
        private readonly Poolable[,] field;
        private readonly int margin;
        private readonly int xPreCalc;
        private readonly int yPreCalc;

        public Collision(Game game, int width, int height, int margin)
        {
            this.game = game;
            this.margin = margin;
            Width = width;
            Height = height;
            Size = CalculateSize();
            field = new Poolable[Width, Height];
            xPreCalc = game.WindowWidth / 2 - Width / 2 * Size + Size / 2;
            yPreCalc = game.WindowHeight / 2 + Height / 2 * Size - Size / 2;
        }


        /// <summary>
        /// Collision.
        /// </summary>
        /// <param name="newPosition">Where the worm wants to move</param>
        /// <returns>If it can or not move to the new position</returns>
        public bool WormCheck(Worm worm, Vector2 position, int deltaX, int deltaY)
        {
            Vector2 next = position + new Vector2(deltaX, deltaY);
            int nextReverseX = ReverseX(next.X);
            int nextReverseY = ReverseY(next.Y);
            if (nextReverseX <= -1 ||
                nextReverseX >= Width ||
                nextReverseY <= -1 ||
                nextReverseY >= Height ||
                Get(next) != null)
                return false;

            Console.CursorLeft = 0;
            Console.Write(nextReverseX.ToString("00") + " " + nextReverseY.ToString("00"));

            // Update worm collision data
            Tail[] wholeWorm = worm.GetWorm();
            SetNull(wholeWorm[^1].Position);
            for (int i = wholeWorm.Length - 1; i > 0; i--)
                Set(wholeWorm[i], wholeWorm[i - 1].Position);
            Set(wholeWorm[0], next);

            return true;
        }

        private int CalculateSize()
        {
            int xSize = game.WindowWidth / (Width + margin * 2);
            int ySize = game.WindowHeight / (Height + margin * 2);
            int size = Helper.Smaller(xSize, ySize);
            if (size % 2 != 0) size--;
            return size;
        }

        private ref Poolable Get(Vector2 position)
        {
            return ref field[ReverseX(position.X), ReverseY(position.Y)];
        }

        public void Set(Poolable entity)
        {
            Get(entity.Position) = entity;
        }

        private void Set(Poolable entity, Vector2 position)
        {
            Get(position) = entity;
        }

        private void SetNull(Vector2 position)
        {
            Get(position) = null;
        }


        public int X(int x)
        {
            return xPreCalc + Size * x;
        }


        public int Y(int y)
        {
            return yPreCalc - Size * y;
        }


        public int ReverseX(float x)
        {
            return (((int)x - xPreCalc) / Size);
        }


        public int ReverseY(float y)
        {
            return (yPreCalc - (int)y) / Size;
        }

        public void VisualizeField()
        {
            Console.Clear();
            for (int y = Height - 1; y >= 0; y--)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (field[x, y] == null)
                        Console.Write(".");
                    else
                        Console.Write("o");
                }
                Console.WriteLine();
            }
        }
    }
}
