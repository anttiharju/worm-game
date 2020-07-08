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
        private readonly int width;
        private readonly int height;
        private readonly int size;

        private readonly Poolable[,] field;
        private readonly int leftBorder;
        private readonly int topBorder;

        /// <summary>
        /// Initializes the field which is a 2d array of poolables used for collision.
        /// </summary>
        /// <param name="game">Required so we know the window dimensions</param>
        /// <param name="width">Field width</param>
        /// <param name="height">Field height</param>
        /// <param name="margin">Field margin</param>
        public Collision(int windowWidth, int windowHeight, Config config)
        {
            width = config.width;
            height = config.height;
            size = config.size;
            field = new Poolable[width, height];
            leftBorder = windowWidth / 2 - width / 2 * size + size / 2;
            topBorder = windowHeight / 2 + height / 2 * size - size / 2;
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
        /// <returns>Wheter or not position is occupied</returns>
        public bool Check(Vector2 target)
        {
            return Check(X(target.X), Y(target.Y));
        }


        /// <summary>
        /// Checks wheter a cell on the field is occupied.
        /// </summary>
        /// <param name="x">Horizontal field position</param>
        /// <param name="y">Vertical field position</param>
        /// <returns></returns>
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
        /// Occupy a cell from the field.
        /// </summary>
        /// <param name="entity">Entity</param>
        public void Set(Poolable entity)
        {
#if DEBUG
            if (entity is WormEntity)
                throw new Exception("Set(entity) is not valid for WormEntities. Use Set(wormEntity, target) instead.");
#endif
            Get(entity.Position) = entity;
        }


        /// <summary>
        /// Occupy a cell from the field for a worm.
        /// </summary>
        /// <param name="wormEntity">Worm</param>
        /// <param name="target">Worm target</param>
        public void Set(WormEntity wormEntity, Vector2 target)
        {
            Get(target) = wormEntity;
        }


        /// <summary>
        /// Set to a cell on the field to null.
        /// </summary>
        /// <param name="target">Entity position</param>
        public void Set(Vector2 target)
        {
            Get(target) = null;
        }


        /// <summary>
        /// Translates a horizontal entity position to a field one.
        /// </summary>
        /// <param name="x">Horizontal entity position</param>
        /// <returns>Horizontal field position</returns>
        public int X(float x)
        {
            return ((Mathf.FastRound(x) - leftBorder) / size);
        }


        /// <summary>
        /// Translates a vertical entity position to a field one.
        /// </summary>
        /// <param name="y">Vertical entity position</param>
        /// <returns>Vertical field position</returns>
        public int Y(float y)
        {
            return (topBorder - Mathf.FastRound(y)) / size;
        }


        /// <summary>
        /// Translates a horizontal field position to an entity one.
        /// </summary>
        /// <param name="x">Horizontal field position</param>
        /// <returns>Horizontal entity position</returns>
        public int EntityX(int x)
        {
            return leftBorder + size * x;
        }


        /// <summary>
        /// Translates a vertical field position to an entity one.
        /// </summary>
        /// <param name="y">Vertical field position</param>
        /// <returns>Vertical entity position</returns>
        public int EntityY(int y)
        {
            return topBorder - size * y;
        }

#if DEBUG
        /// <summary>
        /// Visualises the collision field as ASCII art. Used for debugging and looks cool as shit. Causes lag with large fields (above 20x10). 
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
                        if (field[x, y] is WormEntity)
                            Console.Write("o");
                        if (field[x, y] is Brick)
                            Console.Write("x");
                    }
                }
            }
        }
#endif
    }
}
