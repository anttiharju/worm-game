﻿using System;
using Otter.Utility.MonoGame;
using WormGame.Static;
using WormGame.Pooling;
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
        private readonly PoolableEntity[,] field;
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
            field = new PoolableEntity[width, height];
            leftBorder = config.windowWidth / 2 - width / 2 * size;
            topBorder = config.windowHeight / 2 + height / 2 * size;
            if (width % 2 == 0)
                leftBorder += size / 2;
            if (height % 2 == 0)
                topBorder -= size / 2;
        }


        /// <summary>
        /// Get a cells value from the field at an entity position.
        /// </summary>
        /// <param name="position">Entity position</param>
        /// <returns>Field cell value</returns>
        public ref PoolableEntity Get(Vector2 position)
        {
            return ref Get(X(position.X), Y(position.Y));
        }


        /// <summary>
        /// Get a cells value from the field.
        /// </summary>
        /// <param name="x">Horizontal field position</param>
        /// <param name="y">Vertical field position</param>
        /// <returns>Field cell value</returns>
        public ref PoolableEntity Get(int x, int y)
        {
            return ref field[x, y];
        }


        /// <summary>
        /// Checks wheter a cell on the field is occupied.
        /// </summary>
        /// <param name="target">Entity position</param>
        /// <param name="eatFruit">Wheter or not check should activate fruits</param>
        /// <returns>0 out of bounds, 1 worm, 2 brick, 3 fruit, 4 free</returns>
        public int Check(Vector2 target, bool eatFruit = false)
        {
            return Check(X(target.X), Y(target.Y), eatFruit);
        }


        /// <summary>
        /// Checks wheter a cell on the field is occupied.
        /// </summary>
        /// <param name="x">Horizontal field position</param>
        /// <param name="y">Vertical field position</param>
        /// <param name="eatFruit">Wheter or not check should activate fruits</param>
        /// <returns>0 out of bounds, 1 worm, 2 brick, 3 fruit, 4 free</returns>
        public int Check(int x, int y, bool eatFruit = false)
        {
            if (x < 0 ||
                y < 0 ||
                x >= width ||
                y >= height)
                return 0;
            PoolableEntity cell = Get(x, y);
            if (cell == null)
                return 4;
            if (cell is Worm)
                return 1;
            if (cell is Brick)
                return 2;
            if (cell is Fruit fruit)
            {
                if (eatFruit)
                    fruit.Spawn();
                return 3;
            }
            return 4;
        }


        /// <summary>
        /// Occupy a cell from the field for an entity.
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <param name="position">Entity position</param>
        public void Set(PoolableEntity entity, Vector2 position)
        {
            Get(position) = entity;
        }



        /// <summary>
        /// Occupy a cell from the field.
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <param name="x">Horizontal entity position</param>
        /// <param name="y">Vertical entity position</param>
        public void Set(PoolableEntity entity, float x, float y)
        {
            Get(X(x), Y(y)) = entity;
        }


        /// <summary>
        /// Occupy a cell from the field.
        /// </summary>
        /// <param name="wormEntity">Worm</param>
        /// <param name="x">Horizontal field position</param>
        /// <param name="y">Vertical field position</param>
        public void Set(PoolableEntity entity, int x, int y)
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
            return (Mathf.FastRound(x) - leftBorder) / size;
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


        /// <summary>
        /// Scan for full brick rows to destroy.
        /// </summary>
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
                    // TODO: Remove full brick rows.
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
                        Console.WriteLine(new string(' ', Console.BufferWidth));
                        Console.CursorLeft = 0;
                        Console.CursorTop = 1;
                        Console.WriteLine("[COLLISION] Can't visualize a field wider than " + Console.BufferWidth + ".");
                        return;
                    }
                    PoolableEntity current = field[x, y];
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
            Console.CursorLeft = 0;
            Console.CursorTop = height + 1;
        }
#endif
    }
}
