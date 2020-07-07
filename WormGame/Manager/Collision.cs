using System;
using Otter.Core;
using Otter.Utility.MonoGame;
using WormGame.GameObject;
using WormGame.Static;

namespace WormGame.Manager
{
    /// @author Antti Harju
    /// @version 01.07.2020
    /// <summary>
    /// Class for play area. Has close ties to collision.
    /// </summary>
    public class Collision
    {
        private readonly int width;
        private readonly int height;
        private readonly int size;

        private readonly Poolable[,] field;
        private readonly int leftBorder;
        private readonly int topBorder;

        /// <summary>
        /// Initializes the play area which is basically a 2d array of poolables used for collision.
        /// </summary>
        /// <param name="game">Required so we know the window dimensions</param>
        /// <param name="width">Play area width</param>
        /// <param name="height">Play area height</param>
        /// <param name="margin">Play area margin</param>
        public Collision(Game game, Config config)
        {
            width = config.width;
            height = config.height;
            size = config.size;
            field = new Poolable[width, height];
            leftBorder = game.WindowWidth / 2 - width / 2 * size + size / 2;
            topBorder = game.WindowHeight / 2 + height / 2 * size - size / 2;
        }


        /// <summary>
        /// Get the play area value at an entity position
        /// </summary>
        /// <param name="target">Entitys target position</param>
        /// <returns>Play area value</returns>
        public ref Poolable Get(Vector2 target)
        {
            return ref Get(X(target.X), Y(target.Y));
        }

        private ref Poolable Get(int x, int y)
        {
            return ref field[x, y];
        }

        /// <summary>
        /// Checks worms collision
        /// </summary>
        /// <param name="next"></param>
        /// <returns></returns>
        public bool Check(Vector2 next)
        {
            return Check(X(next.X), Y(next.Y));
        }

        public bool Check(int x, int y)
        {
            if (x <= -1 ||
                x >= width ||
                y <= -1 ||
                y >= height ||
                Get(x, y) != null)
                return false;
            return true;
        }

        /// <summary>
        /// Update play area through this method.
        /// </summary>
        /// <param name="entity">Entity</param>
        public void Update(Poolable entity)
        {
#if DEBUG
            if (entity is WormBase)
                throw new Exception("Update(entity) is not valid for worms. Use Update(target, entity) instead.");
#endif
            Update(entity.Position, entity);
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
            return ((Mathf.FastRound(x) - leftBorder) / size);
        }


        /// <summary>
        /// Translates a vertical entity position to a play area one.
        /// </summary>
        /// <param name="y">Entitys vertical position</param>
        /// <returns>Play areas vertical position</returns>
        public int Y(float y)
        {
            return (topBorder - Mathf.FastRound(y)) / size;
        }


        /// <summary>
        /// Translates a horizontal play area position to an entity one.
        /// </summary>
        /// <param name="x">Horizontal play area position</param>
        /// <returns>Horizontal entity position</returns>
        public int EntityX(int x)
        {
            return leftBorder + size * x;
        }


        /// <summary>
        /// Translates a vertical play area position to an entity one.
        /// </summary>
        /// <param name="y">Vertical play area position</param>
        /// <returns>Vertical entity position</returns>
        public int EntityY(int y)
        {
            return topBorder - size * y;
        }


        /// <summary>
        /// Mainly used for debugging, but also looks kind of cool
        /// </summary>
        public void Visualize()
        {
            for (int y = 0; y < height; y++)
            {
                Console.CursorTop = height - y;
                for (int x = 0; x < width; x++)
                {
                    Console.CursorLeft = x;
                    if (field[x, y] == null)
                        Console.Write(".");
                    else
                    {
                        if (field[x, y] is WormBase)
                            Console.Write("o");
                        if (field[x, y] is BrickBase)
                            Console.Write("x");
                    }
                }
            }
        }
    }
}
