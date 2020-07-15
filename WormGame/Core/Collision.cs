using System;
using System.Text;
using Otter.Utility.MonoGame;
using WormGame.Static;
using WormGame.GameObject;

namespace WormGame.Core
{
    /// @author Antti Harju
    /// @version 08.07.2020
    /// <summary>
    /// Collision field.
    /// </summary>
    public class Collision
    {
        private readonly WormScene scene;
        private readonly Poolable[,] field;
        private readonly int leftBorder;
        private readonly int topBorder;
        private readonly int width;
        private readonly int height;
        private readonly int size;

        /// <summary>
        /// Initializes the collision field which is a 2d array of poolables used for collision.
        /// </summary>
        /// <param name="game">Required so we know the window dimensions</param>
        /// <param name="width">Field width</param>
        /// <param name="height">Field height</param>
        /// <param name="margin">Field margin</param>
        public Collision(Config config)
        {
            scene = config.scene;
            width = config.width;
            height = config.height;
            size = config.size;
            field = new Poolable[width, height];
            leftBorder = config.windowWidth / 2 - width / 2 * size + size / 2;
            topBorder = config.windowHeight / 2 + height / 2 * size - size / 2;
        }


        /// <summary>
        /// Get a cells value from the field at an entity position.
        /// </summary>
        /// <param name="target">Entity target position</param>
        /// <returns>Field value</returns>
        public ref Poolable Get(Vector2 target)
        {
            return ref Get(X(target.X), Y(target.Y));
        }

        public ref Poolable Get(float x, float y)
        {
            return ref Get(X(x), Y(y));
        }

        /// <summary>
        /// Get a cells value from the field.
        /// </summary>
        /// <param name="x">Horizontal field position</param>
        /// <param name="y">Vertical field position</param>
        /// <returns>Field value</returns>
        private ref Poolable Get(int x, int y)
        {
            return ref field[x, y];
        }


        /// <summary>
        /// Checks wheter a cell on the field is occupied.
        /// </summary>
        /// <param name="target">Entity position</param>
        /// <returns>0 if free, 1 if fruit, 2 if occupied</returns>
        public int Check(Vector2 target)
        {
            return Check(X(target.X), Y(target.Y));
        }


        /// <summary>
        /// Checks wheter a cell on the field is occupied.
        /// </summary>
        /// <param name="x">Horizontal field position</param>
        /// <param name="y">Vertical field position</param>
        /// <returns>0 if free, 1 if fruit, 2 if occupied</returns>
        public int Check(int x, int y)
        {
            if (x < 0 ||
                y < 0 ||
                x >= width ||
                y >= height)
                return 2;
            Poolable cell = Get(x, y);
            if (cell is Fruit)
            {
                Fruit fruit = (Fruit)cell;
                fruit.Respawn();
                return 1;
            }
            if (cell != null)
                return 2;
            return 0;
        }


        /// <summary>
        /// Occupy a cell from the field for an entity.
        /// </summary>
        /// <param name="wormEntity">Worm</param>
        /// <param name="target">Worm target</param>
        public void Set(Poolable entity, Vector2 target)
        {
            Get(target) = entity;
        }

        /// <summary>
        /// Occupy a cell from the field for an entity.
        /// </summary>
        /// <param name="wormEntity">Worm</param>
        /// <param name="target">Worm target</param>
        public void Set(Poolable entity, float x, float y)
        {
            Get(x, y) = entity;
        }

        /// <summary>
        /// Occupy a cell from the field for an entity.
        /// </summary>
        /// <param name="wormEntity">Worm</param>
        /// <param name="x">Horizontal field position</param>
        /// <param name="y">Vertical field position</param>
        public void Set(Poolable entity, int x, int y)
        {
            Get(x, y) = entity;
        }

        /// <summary>
        /// Get horizontal field position from an entity one.
        /// </summary>
        /// <param name="x">Horizontal entity position</param>
        /// <returns>Horizontal field position</returns>
        public int X(float x)
        {
            return ((Mathf.FastRound(x) - leftBorder) / size);
        }


        /// <summary>
        /// Get vertical field position from an entity one.
        /// </summary>
        /// <param name="y">Vertical entity position</param>
        /// <returns>Vertical field position</returns>
        public int Y(float y)
        {
            return (topBorder - Mathf.FastRound(y)) / size;
        }


        /// <summary>
        /// Get horizontal entity position from a field one.
        /// </summary>
        /// <param name="x">Horizontal field position</param>
        /// <returns>Horizontal entity position</returns>
        public int EntityX(int x)
        {
            return leftBorder + size * x;
        }


        /// <summary>
        /// Get vertical entity position from a field one.
        /// </summary>
        /// <param name="y">Vertical field position</param>
        /// <returns>Vertical entity position</returns>
        public int EntityY(int y)
        {
            return topBorder - size * y;
        }

        public void Scan()
        {
            for (int y = 0; y < height; y++)
            {
                bool full = true;
                for (int x = 0; x < width; x++)
                {
                    if (!(field[x, y] is Brick))
                        full = false;
                }
                if (full)
                {
                    // TODO
                }
            }
        }

#if DEBUG
        /// <summary>
        /// Visualises collision field in console as text.
        /// </summary>
        public void Visualize(Config config)
        {
            for (int y = 0; y < height; y++)
            {
                Console.CursorTop = height - y;
                for (int x = 0; x < width; x++)
                {
                    try
                    {
                        Console.CursorLeft = x;
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        config.visualizeCollision = false;
                        Console.CursorLeft = 0;
                        Console.CursorTop = height - y;
                        StringBuilder message = new StringBuilder("[COLLISION] Max visualizable field width is ").Append(Console.BufferWidth).Append(".");
                        message.Append(new string(' ', Console.BufferWidth - message.Length));
                        Console.WriteLine(message.ToString());
                        return;
                    }
                    Poolable current = field[x, y];
                    if (current == null)
                    {
                        Console.Write(".");
                        continue;
                    }
                    if (current is Worm)
                    {
                        Console.Write("o");
                        continue;
                    }
                    if (current is Brick)
                    {
                        Console.Write("x");
                        continue;
                    }
                    if (current is Fruit)
                    {
                        Console.Write("+");
                        continue;
                    }
                }
            }
        }
#endif
    }
}
