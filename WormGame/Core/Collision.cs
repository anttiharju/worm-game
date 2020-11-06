using Otter.Utility.MonoGame;
using System;
using WormGame.Entities;
using WormGame.Static;

namespace WormGame.Core
{
    /// @author Antti Harju
    /// @version v0.5
    /// <summary>
    /// Collision system.
    /// </summary>
    public class Collision
    {
        /// Collision types. Use these instead of raw ints.
        public readonly int invalid = 0;
        public readonly int worm = 1;
        public readonly int block = 2;
        public readonly int fruit = 3;
        public readonly int empty = 4;

        private readonly object[,] grid;
        private readonly int leftBorder;
        private readonly int topBorder;
        private readonly int size;
        private readonly int width;
        private readonly int height;


        /// <summary>
        /// Constructor, ínitializes grid.
        /// </summary>
        /// <param name="settings">Settings</param>
        public Collision(Settings settings)
        {
            width = settings.width;
            height = settings.height;
            size = settings.size;
            grid = new object[width, height];
            leftBorder = settings.leftBorder;
            topBorder = settings.topBorder;
        }


        /// <summary>
        /// Get object from grid.
        /// </summary>
        /// <param name="x">Horizontal grid position</param>
        /// <param name="y">Vertical grid position</param>
        /// <returns>Object</returns>
        public ref object Get(int x, int y)
        {
            return ref grid[x, y];
        }


        /// <summary>
        /// Get object from grid.
        /// </summary>
        /// <param name="position">Entity position</param>
        /// <returns>Object</returns>
        public ref object Get(Vector2 position)
        {
            return ref Get(X(position.X), Y(position.Y));
        }


        /// <summary>
        /// Get object type from grid.
        /// </summary>
        /// <param name="x">Horizontal grid position</param>
        /// <param name="y">Vertical grid position</param>
        /// <param name="consume">Consume fruit</param>
        /// <returns>Object type</returns>
        public int GetType(int x, int y, bool consume = false)
        {
            if (x < 0 ||
                y < 0 ||
                x >= width ||
                y >= height)
                return invalid;
            object current = grid[x, y];
            if (current == null)
                return empty;
            if (current is BlockModule)
                return block;
            if (current is Worm)
                return worm;
            if (current is Fruits currentFruit)
            {
                if (consume)
                {
                    currentFruit.Disable(x, y);
                    currentFruit.Spawn(); // The number or fruits stays constant
                }
                return fruit;
            }
            throw new CollisionException();
        }


        /// <summary>
        /// Get object type from grid.
        /// </summary>
        /// <param name="position">Entity position</param>
        /// <param name="consume">Consume fruit</param>
        /// <returns>Object type</returns>
        public int GetType(Vector2 position, bool consume = false)
        {
            return GetType(X(position.X), Y(position.Y), consume);
        }



        /// <summary>
        /// Set object to grid.
        /// </summary>
        /// <param name="obj">Object</param>
        /// <param name="x">Horizontal grid position</param>
        /// <param name="y">Vertical grid position</param>
        public void Set(object obj, int x, int y)
        {
            grid[x, y] = obj;
        }



        /// <summary>
        /// Set object to grid.
        /// </summary>
        /// <param name="obj">Object</param>
        /// <param name="position">Entity position</param>
        public void Set(object obj, Vector2 position)
        {
            Set(obj, X(position.X), Y(position.Y));
        }


        /// <summary>
        /// Set entity to grid.
        /// </summary>
        /// <param name="obj">Object</param>
        /// <param name="x">Horizontal entity position</param>
        /// <param name="y">Vertical entity position</param>
        public void Set(object obj, float x, float y)
        {
            Set(obj, X(x), Y(y));
        }


        /// <summary>
        /// Set block module to grid.
        /// </summary>
        /// <param name="module">Block module or null</param>
        /// <param name="startX">module.X</param>
        /// <param name="startY">module.Y</param>
        /// <param name="width">module.Width</param>
        /// <param name="height">module.Height</param>
        public void Set(object module, int startX, int startY, int width, int height)
        {
            for (int x = startX; x < startX + width; x++)
                for (int y = startY; y < startY + height; y++)
                    Set(module, x, y);
        }


        /// <summary>
        /// Translate horizontal entity position to a grid one.
        /// </summary>
        /// <param name="x">Horizontal entity position</param>
        /// <returns>Horizontal grid position</returns>
        public int X(float x)
        {
            return (SimpleMath.Round(x) - leftBorder) / size;
        }


        /// <summary>
        /// Translate vertical entity position to a grid one.
        /// </summary>
        /// <param name="y">Vertical entity position</param>
        /// <returns>Vertical grid position</returns>
        public int Y(float y)
        {
            return (SimpleMath.Round(y) - topBorder) / size;
        }


        /// <summary>
        /// Translate horizontal grid position to an entity one.
        /// </summary>
        /// <param name="x">Horizontal grid position</param>
        /// <returns>Horizontal entity position</returns>
        public int EntityX(int x)
        {
            return leftBorder + size * x;
        }


        /// <summary>
        /// Translates vertical grid position to an entity one.
        /// </summary>
        /// <param name="y">Vertical grid position</param>
        /// <returns>Vertical entity position</returns>
        public int EntityY(int y)
        {
            return topBorder + size * y;
        }


        /// <summary>
        /// Clear grid.
        /// </summary>
        public void Reset()
        {
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    grid[x, y] = null;
                }
        }
#if DEBUG
        /// <summary>
        /// Visualize collision grid in debug console as ASCII.
        /// </summary>
        public void Visualize()
        {
            System.Text.StringBuilder visualization = new System.Text.StringBuilder((width + 1) * height);
            for (int y = 0; y < height; y++)
            {
                visualization.Append("\n");
                for (int x = 0; x < width; x++)
                {
                    object current = grid[x, y];
                    if (current == null)
                    {
                        visualization.Append('.');
                        continue;
                    }
                    if (current is BlockModule)
                    {
                        visualization.Append('x');
                        continue;
                    }
                    if (current is Worm)
                    {
                        visualization.Append('o');
                        continue;
                    }
                    if (current is Fruits)
                    {
                        visualization.Append('f');
                        continue;
                    }
                    throw new CollisionException();
                }
            }
            Console.CursorTop = 0;
            Console.WriteLine(visualization.ToString());
        }
#endif
    }


    /// @author Antti Harju
    /// @version v0.5
    /// <summary>
    /// Exception for collision.
    /// </summary>
    public class CollisionException : Exception
    {
        /// <summary>
        /// Add custom exception message.
        /// </summary>
        public CollisionException() : base("Unknown collision object.") { }
    }
}
