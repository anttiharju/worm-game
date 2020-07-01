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
        private readonly int fieldLeft;
        private readonly int fieldTop;

        public Collision(Game game, int width, int height, int margin)
        {
            this.game = game;
            this.margin = margin;
            Width = width;
            Height = height;
            Size = CalculateSize();
            field = new Poolable[Width, Height];
            fieldLeft = game.WindowWidth / 2 - Width / 2 * Size + Size / 2;
            fieldTop = game.WindowHeight / 2 + Height / 2 * Size - Size / 2;
        }


        /// <summary>
        /// Collision.
        /// </summary>
        /// <param name="newPosition">Where the worm wants to move</param>
        /// <returns>If it can or not move to the new position</returns>
        public bool WormCheck(Worm worm, Vector2 target, int deltaX, int deltaY, bool noclip)
        {
            // Actual collision
            Vector2 next = target + new Vector2(deltaX, deltaY);
            int nextReverseX = ReverseX(next.X);
            int nextReverseY = ReverseY(next.Y);
            if (nextReverseX <= -1 ||
                nextReverseX >= Width ||
                nextReverseY <= -1 ||
                nextReverseY >= Height ||
                (Get(next) != null && !noclip))
                return false;

            // Update worm collision data
            SetField(next, worm[0]);
            SetField(next, worm[^1]);
            SetField(worm[^1].Target, null);
            for (int i = worm.Length - 1; i > 0; i--)
                SetField(worm[i - 1].Target, worm[i]);
            SetField(next, worm[0]);
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

        private ref Poolable Get(Vector2 target)
        {
            return ref field[ReverseX(target.X), ReverseY(target.Y)];
        }

        public void SetField(Vector2 target, Poolable entity)
        {
            Get(target) = entity;
        }


        public int X(int x)
        {
            return fieldLeft + Size * x;
        }


        public int Y(int y)
        {
            return fieldTop - Size * y;
        }


        public int ReverseX(float x)
        {
            return (((int)x - fieldLeft) / Size);
        }


        public int ReverseY(float y)
        {
            return (fieldTop - (int)y) / Size;
        }
    }
}
