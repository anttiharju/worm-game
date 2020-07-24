using System;
using Otter.Utility.MonoGame;
using WormGame.Static;
using WormGame.Pooling;
using WormGame.GameObject;

namespace WormGame.Core
{
    /// @author Antti Harju
    /// @version 23.07.2020
    /// <summary>
    /// Collision field. Stays performant with large fields.
    /// </summary>
    public class Collision
    {
        private readonly PoolableEntity[,] field;
        private readonly int leftBorder;
        private readonly int topBorder;
        private readonly int size;
        private readonly Exception unknownPoolableException = new Exception("Unknown poolable entity on collision field.");

        public int Width { get; }
        public int Height { get; }

        /// <summary>
        /// Initializes the collision field which is a 2d array of poolable entities.
        /// </summary>
        /// <param name="game">Required so we know the window dimensions</param>
        /// <param name="width">Field width</param>
        /// <param name="height">Field height</param>
        /// <param name="margin">Field margin</param>
        public Collision(Config config)
        {
            Width = config.width;
            Height = config.height;
            size = config.size;
            field = new PoolableEntity[Width, Height];
            leftBorder = config.windowWidth / 2 - Width / 2 * size;
            topBorder = config.windowHeight / 2 + Height / 2 * size;
            if (Width % 2 == 0)
                leftBorder += size / 2;
            if (Height % 2 == 0)
                topBorder -= size / 2;
        }


        /// <summary>
        /// Returns a poolable entity as reference.
        /// </summary>
        /// <param name="x">Horizontal field position</param>
        /// <param name="y">Vertical field position</param>
        /// <returns>Poolable entity as reference</returns>
        public ref PoolableEntity Get(int x, int y)
        {
            return ref field[x, y];
        }


        /// <summary>
        /// Returns a poolable entity as reference.
        /// </summary>
        /// <param name="position">Entity position</param>
        /// <returns>Poolable entity as reference</returns>
        public ref PoolableEntity Get(Vector2 position)
        {
            return ref Get(X(position.X), Y(position.Y));
        }


        /// <summary>
        /// Check a cell from field. Returns numbers instead of strings so we can use > < operators.
        /// </summary>
        /// <param name="x">Horizontal field position</param>
        /// <param name="y">Vertical field position</param>
        /// <param name="consume">Consume fruits</param>
        /// <returns>0 out of bounds, 1 worm, 2 brick, 3 fruit, 4 empty</returns>
        public int Check(int x, int y, bool consume = false)
        {
            if (x < 0 ||
                y < 0 ||
                x >= Width ||
                y >= Height)
                return 0;
            PoolableEntity cell = field[x, y];
            if (cell == null)
                return 4;
            if (cell is Worm)
                return 1;
            if (cell is Block)
                return 2;
            if (cell is Fruit fruit)
            {
                if (consume)
                    fruit.Spawn();
                return 3;
            }
            throw unknownPoolableException;
        }


        /// <summary>
        /// Check a cell from field. Returns numbers instead of strings so we can use > < operators.
        /// </summary>
        /// <param name="target">Entity position</param>
        /// <param name="consume">Consume fruits</param>
        /// <returns>0 out of bounds, 1 worm, 2 brick, 3 fruit, 4 empty</returns>
        public int Check(Vector2 target, bool consume = false)
        {
            return Check(X(target.X), Y(target.Y), consume);
        }


        /// <summary>
        /// Set entity to field.
        /// </summary>
        /// <param name="entity">Worm</param>
        /// <param name="x">Horizontal field position</param>
        /// <param name="y">Vertical field position</param>
        public void Set(PoolableEntity entity, int x, int y)
        {
            field[x, y] = entity;
        }


        /// <summary>
        /// Set entity to field.
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <param name="position">Entity position</param>
        public void Set(PoolableEntity entity, Vector2 position)
        {
            Set(entity, X(position.X), Y(position.Y));
        }


        /// <summary>
        /// Set entity to field.
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <param name="x">Horizontal entity position</param>
        /// <param name="y">Vertical entity position</param>
        public void Set(PoolableEntity entity, float x, float y)
        {
            Set(entity, X(x), Y(y));
        }


        /// <summary>
        /// Translates horizontal entity position to a field one.
        /// </summary>
        /// <param name="x">Horizontal entity position</param>
        /// <returns>Horizontal field position</returns>
        public int X(float x)
        {
            return (Mathf.FastRound(x) - leftBorder) / size;
        }


        /// <summary>
        /// Translates vertical entity position to a field one.
        /// </summary>
        /// <param name="y">Vertical entity position</param>
        /// <returns>Vertical field position</returns>
        public int Y(float y)
        {
            return (topBorder - Mathf.FastRound(y)) / size;
        }


        /// <summary>
        /// Translates horizontal field position to an entity one.
        /// </summary>
        /// <param name="x">Horizontal field position</param>
        /// <returns>Horizontal entity position</returns>
        public int EntityX(int x)
        {
            return leftBorder + size * x;
        }


        /// <summary>
        /// Translates vertical field position to an entity one.
        /// </summary>
        /// <param name="y">Vertical field position</param>
        /// <returns>Vertical entity position</returns>
        public int EntityY(int y)
        {
            return topBorder - size * y;
        }
#if DEBUG
        /// <summary>
        /// Visualizes collision field in debug console as ASCII art.
        /// </summary>
        public void Visualize()
        {
            for (int y = 0; y < Height; y++)
            {
                Console.CursorTop = Height - y;
                System.Text.StringBuilder line = new System.Text.StringBuilder(Width);
                for (int x = 0; x < Width; x++)
                {
                    PoolableEntity current = field[x, y];
                    if (current == null)
                    {
                        line.Append('.');
                        continue;
                    }
                    if (current is Block)
                    {
                        line.Append('x');
                        continue;
                    }
                    if (current is Worm)
                    {
                        line.Append('o');
                        continue;
                    }
                    if (current is Fruit)
                    {
                        line.Append('f');
                        continue;
                    }
                    throw unknownPoolableException;
                }
                Console.WriteLine(line.ToString());
            }
            Console.CursorLeft = 0;
            Console.CursorTop = Height + 1;
        }
#endif
    }
}
