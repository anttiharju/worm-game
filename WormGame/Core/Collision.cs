using System;
using Otter.Utility.MonoGame;
using WormGame.Static;
using WormGame.Pooling;
using WormGame.Entities;

namespace WormGame.Core
{
    /// @author Antti Harju
    /// @version 28.07.2020
    /// <summary>
    /// Collision field. Stays performant with large fields.
    /// </summary>
    public class Collision
    {
        private readonly PoolableEntity[,] collision;
        private readonly int leftBorder;
        private readonly int topBorder;
        private readonly int size;

        public readonly int[,] blockBuffer;

        /// Collision values exist to improve code readability and maintainablility. Values >= 0 are reserved for blocks.
        #region Collision values
        public int WormValue { get { return -1; } }
        public int InvalidValue { get { return -2; } }
        public int FruitValue { get { return -3; } }
        public int FreeValue { get { return -4; } }
        #endregion


        /// <summary>
        /// Collision field width.
        /// </summary>
        public int Width { get; }


        /// <summary>
        /// Collision field height.
        /// </summary>
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
            collision = new PoolableEntity[Width, Height];
            blockBuffer = new int[Width, Height];
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
            return ref collision[x, y];
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
        /// <returns>0 out of bounds, 1 worm, 2 block, 3 fruit, 4 empty</returns>
        public int Check(int x, int y, bool consume = false)
        {
            if (x < 0 ||
                y < 0 ||
                x >= Width ||
                y >= Height)
                return 0;
            PoolableEntity current = collision[x, y];
            if (current == null)
                return 4;
            if (current is Worm)
                return 1;
            if (current is Block)
                return 2;
            if (current is Fruit fruit)
            {
                if (consume)
                    fruit.Spawn();
                return 3;
            }
            throw new Exception("Unknown collision entity.");
        }


        /// <summary>
        /// Check a cell from field. Returns numbers instead of strings so we can use > < operators.
        /// </summary>
        /// <param name="target">Entity position</param>
        /// <param name="consume">Consume fruits</param>
        /// <returns>0 out of bounds, 1 worm, 2 block, 3 fruit, 4 empty</returns>
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
            collision[x, y] = entity;
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


        /// <summary>
        /// Reset collision.
        /// </summary>
        public void Reset()
        {
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                {
                    collision[x, y] = null;
                }
        }
#if DEBUG
        /// <summary>
        /// Visualizes collision field in debug console as ASCII art.
        /// </summary>
        public void VisualizeCollision()
        {
            for (int y = 0; y < Height; y++)
            {
                Console.CursorTop = Height - y;
                System.Text.StringBuilder line = new System.Text.StringBuilder(Width);
                for (int x = 0; x < Width; x++)
                {
                    PoolableEntity current = collision[x, y];
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
                    throw new Exception("Unknown collision entity.");
                }
                Console.WriteLine(line.ToString());
            }
            Console.CursorLeft = 0;
            Console.CursorTop = Height + 1;
        }
#endif
    }
}
