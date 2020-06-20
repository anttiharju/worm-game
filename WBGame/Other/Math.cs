using System;
using System.Collections.Generic;
using System.Text;
using Otter;

namespace WBGame.Other
{
    static class Math
    {
        /// <summary>
        /// Area two Vector2s roughly equal (collision)
        /// </summary>
        /// <param name="a">First Vector2</param>
        /// <param name="b">Second Vector2</param>
        /// <param name="errorMargin">Accuracy</param>
        /// <returns></returns>
        public static bool RoughlyEquals(Vector2 a, Vector2 b, float errorMargin)
        {
            if (RoughlyEquals(a.X, b.X, errorMargin))
                if (RoughlyEquals(a.Y, b.Y, errorMargin))
                    return true;
            return false;
        }


        /// <summary>
        /// Are two floats roughly equal
        /// </summary>
        /// <param name="a">First float</param>
        /// <param name="b">Second float</param>
        /// <param name="errorMargin">Accuracy</param>
        /// <returns></returns>
        public static bool RoughlyEquals(float a, float b, float errorMargin)
        {
            if (b - errorMargin < a && b + errorMargin > a)
                return true;
            return false;
        }
    }
}
