// Copyright (c) 2021 Emil Forslund
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
using System;
using SFML.Graphics;
using SFML.System;

namespace Platformer {
    
    /// <summary>
    /// Lightweight collision/intersection library for SFML. Not overly
    /// optimized, but suitable for simple 2D games. This class also adds some
    /// utility functions as extensions to the <c>Vector2f</c>-struct in SFML,
    /// like <c>Length()</c>, <c>Normalized()</c>, etc.
    /// </summary><remarks>
    /// <para>Copyright (c) 2021 Emil Forslund. All rights reserved.</para>
    /// <para>Licensed under the MIT-License.</para>
    /// </remarks>
    public static class Collision {
        
        /// <summary>
        /// Stores information about how to resolve an intersection between two
        /// shapes in 2D.
        /// </summary>
        public struct Hit {
            
            /// <summary>
            /// Vector with length 1 that represents the direction for the first
            /// shape to move to resolve the intersection.
            /// </summary>
            public Vector2f Normal;
            
            /// <summary>
            /// The distance to move in the direction of the normal to resolve
            /// the intersection. This is always a positive number or zero.
            /// </summary>
            public float Overlap;
        }

        /// <summary>
        /// Perform an intersection test between two rectangles, returning
        /// <c>true</c> if they intersect and <c>false</c>
        /// otherwise. The output variable will contain the normal of the
        /// collision as well as the minimum distance to move in that direction
        /// to resolve the collision if there is an intersection.
        /// </summary>
        /// <param name="lhs">first rectangle</param>
        /// <param name="rhs">second rectangle</param>
        /// <param name="hit">direction and distance for the first rectangle to
        ///                   move to resolve the collision</param>
        /// <returns>true if the rectangles intersect</returns>
        public static bool RectangleRectangle(FloatRect lhs, FloatRect rhs, out Hit hit) {
            hit = new Hit();

            // Compute Minkowski Difference of both rectangles
            var center    = Center(rhs) - Center(lhs);
            var centerAbs = Absolute(center);
            var halfSize  = 0.5f * (Size(lhs) + Size(rhs));
            var difference = centerAbs - halfSize;
            
            // If shape doesn't contain (0, 0), the rectangles don't intersect.
            if (MathF.Max(difference.X, difference.Y) >= 0.0f) {
                return false;
            }

            // If center == (0, 0), normal can't be determined so assume it is
            // either right or down.
            if (LengthSqr(centerAbs) <= float.Epsilon) {
                if (MathF.Abs(halfSize.X) > MathF.Abs(halfSize.Y)) {
                    hit.Normal  = new Vector2f(1.0f, 0.0f);
                    hit.Overlap = halfSize.X;
                } else {
                    hit.Normal  = new Vector2f(0.0f, 1.0f);
                    hit.Overlap = halfSize.Y;
                }
                return true;
            }
            
            // Set the normal to the most dominant direction (X or Y)
            hit.Normal = difference.X > difference.Y
                ? new Vector2f(MathF.Sign(center.X) * difference.X, 0.0f) 
                : new Vector2f(0.0f, MathF.Sign(center.Y) * difference.Y);
            
            // Normalize normal and solve for the overlap
            hit.Overlap = Length(hit.Normal);
            hit.Normal /= hit.Overlap;
            return true;
        }
        
        /// <summary>
        /// Returns the size of the rectangle as a vector.
        /// </summary>
        /// <param name="rect">the input rectangle</param>
        /// <returns>the size (width and height)</returns>
        private static Vector2f Size(FloatRect rect) =>
            new Vector2f(rect.Width, rect.Height);

        /// <summary>
        /// The top-left corner of the rectangle.
        /// </summary>
        /// <param name="rect">the input rectangle</param>
        /// <returns>the top-left corner</returns>
        private static Vector2f Min(FloatRect rect) => 
            new Vector2f(rect.Left, rect.Top);

        /// <summary>
        /// The lower-right corner of the rectangle.
        /// </summary>
        /// <param name="rect">the input rectangle</param>
        /// <returns>the lower-right corner</returns>
        private static Vector2f Max(FloatRect rect) => 
            Min(rect) + Size(rect);

        /// <summary>
        /// Returns the center position of the rectangle (assumes that the size
        /// is not negative).
        /// </summary>
        /// <param name="rect">the input rectangle</param>
        /// <returns>the center point</returns>
        private static Vector2f Center(FloatRect rect) =>
            Min(rect) + 0.5f * Size(rect);
        
        /// <summary>
        /// Returns a new vector where both components are set to the absolute
        /// value of those from the input vector.
        /// </summary>
        /// <param name="v">the input vector</param>
        /// <returns>the absolute vector</returns>
        private static Vector2f Absolute(Vector2f v) =>
            new Vector2f(
                MathF.Abs(v.X),
                MathF.Abs(v.Y)
            );
        
        /// <summary>
        /// Returns the dot-product (scalar product/inner product) between this
        /// and another vector. The dot-product represents the cosine of the
        /// angle between two vectors, multiplied with the product of their
        /// lengths. It can be used to efficiently compare the direction of two
        /// vectors without using trigonometry.
        /// </summary>
        /// <param name="a">the first vector</param>
        /// <param name="b">the second vector</param>
        /// <returns>the dot-product</returns>
        private static float Dot(Vector2f a, Vector2f b) => 
            a.X * b.X + a.Y * b.Y;
        
        /// <summary>
        /// Returns the length of this vector.
        /// </summary>
        /// <param name="v">the vector to compute the length of</param>
        /// <returns>the length</returns>
        private static float Length(Vector2f v) => MathF.Sqrt(Dot(v, v));
        
        /// <summary>
        /// Returns the squared length (length^2) of this vector. This is more
        /// efficient than computing the length.
        /// </summary>
        /// <param name="v">the vector to compute the squared length of</param>
        /// <returns>the length squared</returns>
        private static float LengthSqr(Vector2f v) => Dot(v, v);
    }
}