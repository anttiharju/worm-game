﻿using System;
using Otter.Utility.MonoGame;
using WormGame.Static;
using WormGame.Entities;

namespace WormGame.Core
{
    /// @author Antti Harju
    /// @version 14.08.2020
    /// <summary>
    /// Collision system.
    /// </summary>
    public class Collision
    {
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
        /// Initialize collision grid.
        /// </summary>
        /// <param name="game">Required so we know the window dimensions</param>
        /// <param name="width">Grid width</param>
        /// <param name="height">Grid height</param>
        /// <param name="margin">Grid margin</param>
        public Collision(Config config)
        {
            width = config.width;
            height = config.height;
            size = config.size;
            grid = new object[width, height];
            leftBorder = config.leftBorder;
            topBorder = config.topBorder;
        }


        /// <summary>
        /// Returns a poolable entity as reference.
        /// </summary>
        /// <param name="x">Horizontal field position</param>
        /// <param name="y">Vertical field position</param>
        /// <returns>Poolable entity as reference</returns>
        public ref object Get(int x, int y)
        {
            return ref grid[x, y];
        }


        /// <summary>
        /// Returns a poolable entity as reference.
        /// </summary>
        /// <param name="position">Entity position</param>
        /// <returns>Poolable entity as reference</returns>
        public ref object Get(Vector2 position)
        {
            return ref Get(X(position.X), Y(position.Y));
        }


        /// <summary>
        /// Check a cell from field. Returns numbers instead of strings so we can use > < operators.
        /// </summary>
        /// <param name="x">Horizontal field position</param>
        /// <param name="y">Vertical field position</param>
        /// <param name="consume">Consume fruit</param>
        /// <returns>invalid 0, worm 1, block 2, fruit 3, empty 4</returns>
        public int Check(int x, int y, bool consume = false)
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
            if (current is Fruits fruits)
            {
                if (consume)
                {
                    fruits.Remove(x, y);
                    fruits.Spawn();
                }
                return fruit;
            }
            throw new UnknownCollisionException();
        }


        /// <summary>
        /// Check a cell from field. Returns numbers instead of strings so we can use > < operators.
        /// </summary>
        /// <param name="target">Entity position</param>
        /// <param name="consume">Consume fruit</param>
        /// <returns>out of bounds is 0, worm is 1, block is 2, fruit is 3, empty is 4</returns>
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
        public void Add(object entity, int x, int y)
        {
            grid[x, y] = entity;
        }


        /// <summary>
        /// Set entity to field.
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <param name="position">Entity position</param>
        public void Add(object entity, Vector2 position)
        {
            Add(entity, X(position.X), Y(position.Y));
        }


        /// <summary>
        /// Set entity to field.
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <param name="x">Horizontal entity position</param>
        /// <param name="y">Vertical entity position</param>
        public void Add(object entity, float x, float y)
        {
            Add(entity, X(x), Y(y));
        }


        /// <summary>
        /// Add block module to collision.
        /// </summary>
        /// <param name="module">Block module or null</param>
        /// <param name="startX">module.X</param>
        /// <param name="startY">module.Y</param>
        /// <param name="width">module.Width</param>
        /// <param name="height">module.Height</param>
        public void Add(object module, int startX, int startY, int width, int height)
        {
            for (int x = startX; x < startX + width; x++)
                for (int y = startY; y < startY + height; y++)
                    Add(module, x, y);
        }


        /// <summary>
        /// Translates horizontal entity position to a field one.
        /// </summary>
        /// <param name="x">Horizontal entity position</param>
        /// <returns>Horizontal field position</returns>
        public int X(float x)
        {
            return (FastMath.Round(x) - leftBorder) / size;
        }


        /// <summary>
        /// Translates vertical entity position to a field one.
        /// </summary>
        /// <param name="y">Vertical entity position</param>
        /// <returns>Vertical field position</returns>
        public int Y(float y)
        {
            return (FastMath.Round(y) - topBorder) / size;
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
            return topBorder + size * y;
        }


        /// <summary>
        /// Reset collision.
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
        /// Visualizes collision field in debug console as ASCII art.
        /// </summary>
        public void Visualize()
        {
            System.Text.StringBuilder visualization = new System.Text.StringBuilder((width + 1) * height);
            Console.CursorTop = 1;
            for (int y = 0; y < height; y++)
            {
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
                    throw new UnknownCollisionException();
                }
                visualization.Append("\n");
            }
            Console.WriteLine(visualization.ToString());
            Console.CursorTop = height + 1;
        }
#endif
    }


    /// <summary>
    /// Exception to use when entity is unknown.
    /// </summary>
    public class UnknownCollisionException : Exception
    {
        /// <summary>
        /// Constructor. Creates custom exception message.
        /// </summary>
        public UnknownCollisionException() : base("Unknown collision object.") { }
    }
}
