using Otter;
using System;
using WormGame.Help;

namespace WormGame.Other
{
    /// @author Antti Harju
    /// @version 01.07.2020
    /// <summary>
    /// Class for play area. Has close ties to collision.
    /// </summary>
    class PlayArea
    {
        private readonly Game game;
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int Size { get; private set; }

        private readonly Poolable[,] playArea;
        private readonly int playAreaMargin;
        private readonly int playAreaLeft;
        private readonly int playAreaTop;

        /// <summary>
        /// Initializes the play area which is basically a 2d array of poolables used for collision.
        /// </summary>
        /// <param name="game">Required so we know the window dimensions</param>
        /// <param name="width">Play area width</param>
        /// <param name="height">Play area height</param>
        /// <param name="margin">Play area margin</param>
        public PlayArea(Game game, int width, int height, int margin)
        {
            this.game = game;
            this.playAreaMargin = margin;
            Width = width;
            Height = height;
            Size = CalculateSize();
            playArea = new Poolable[Width, Height];
            playAreaLeft = game.WindowWidth / 2 - Width / 2 * Size + Size / 2;
            playAreaTop = game.WindowHeight / 2 + Height / 2 * Size - Size / 2;
        }


        /// <summary>
        /// Calculates entity size based on window and play area dimensions.
        /// </summary>
        /// <returns>Entity size</returns>
        private int CalculateSize()
        {
            int xSize = game.WindowWidth / (Width + playAreaMargin * 2);
            int ySize = game.WindowHeight / (Height + playAreaMargin * 2);
            int size = Mathf.Smaller(xSize, ySize);
            if (size % 2 != 0) size--;
            return size;
        }


        /// <summary>
        /// Get the play area value at an entity position
        /// </summary>
        /// <param name="target">Entitys target position</param>
        /// <returns>Play area value</returns>
        public ref Poolable Get(Vector2 target)
        {
            return ref playArea[X(target.X), Y(target.Y)];
        }


        /// <summary>
        /// Update play area through this method.
        /// </summary>
        /// <param name="target">Entity position</param>
        /// <param name="entity">Entity</param>
        public void Update(Vector2 target, Poolable entity = null)
        {
            Get(target) = entity;
        }


        /// <summary>
        /// Translates a horizontal entity position to a play area one.
        /// </summary>
        /// <param name="x">Entitys horizontal position</param>
        /// <returns>Play areas horizontal position</returns>
        public int X(float x)
        {
            return (((int)x - playAreaLeft) / Size);
        }


        /// <summary>
        /// Translates a vertical entity position to a play area one.
        /// </summary>
        /// <param name="y">Entitys vertical position</param>
        /// <returns>Play areas vertical position</returns>
        public int Y(float y)
        {
            return (playAreaTop - (int)y) / Size;
        }


        /// <summary>
        /// Translates a horizontal play area position to an entity one.
        /// </summary>
        /// <param name="x">Horizontal play area position</param>
        /// <returns>Horizontal entity position</returns>
        public int EntityX(int x)
        {
            return playAreaLeft + Size * x;
        }


        /// <summary>
        /// Translates a vertical play area position to an entity one.
        /// </summary>
        /// <param name="y">Vertical play area position</param>
        /// <returns>Vertical entity position</returns>
        public int EntityY(int y)
        {
            return playAreaTop - Size * y;
        }


        /// <summary>
        /// Mainly used for debugging, but also looks kind of cool
        /// </summary>
        public void Visualize()
        {
            for (int y = 0; y < Height; y++)
            {
                Console.CursorTop = Height - y;
                for (int x = 0; x < Width; x++)
                {
                    Console.CursorLeft = x;
                    if (playArea[x, y] == null)
                        Console.Write(".");
                    else
                        Console.Write("o");
                }
            }
        }
    }
}
