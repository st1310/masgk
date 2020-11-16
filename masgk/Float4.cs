﻿using System;
using System.Collections.Generic;
using System.Text;

namespace masgk
{
    public class Float4
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float W { get; set; }

        public Float4()
        {
            X = Y = Z = W = 0.0f;
        }

        public Float4(float x, float y, float z, float w)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.W = w;
        }

        public Float4(Float3 v, float w) : this(v.X, v.Y, v.Z, w)
        { }

        // Operator overloading
        public static Float4 operator +(Float4 a) => a;
        public static Float4 operator -(Float4 a) => new Float4(-a.X, -a.Y, -a.Z, -a.W);
        public static Float4 operator +(Float4 a, Float4 b) => new Float4(a.X + b.X, a.Y + b.Y, a.Z + b.Z, a.W + b.W);
        public static Float4 operator -(Float4 a, Float4 b) => a + (-b);
        public static Float4 operator *(Float4 a, float b) => new Float4(a.X * b, a.Y * b, a.Z * b, a.W * b);
        public static Float4 operator /(Float4 a, float b) => b == 0 ? throw new DivideByZeroException() : new Float4(a.X / b, a.Y / b, a.Z / b, a.W / b);

        public static Float4 operator *(Float4x4 a, Float4 b) =>
            new Float4
            {
                X = a[0, 0] * b.X + a[0, 1] * b.Y + a[0, 2] * b.Z + a[0, 3] * b.W,
                Y = a[1, 0] * b.X + a[1, 1] * b.Y + a[1, 2] * b.Z + a[1, 3] * b.W,
                Z = a[2, 0] * b.X + a[2, 1] * b.Y + a[2, 2] * b.Z + a[2, 3] * b.W,
                W = a[3, 0] * b.X + a[3, 1] * b.Y + a[3, 2] * b.Z + a[3, 3] * b.W
            };

        public float Dot(Float4 v) => X * v.X + Y * v.Y + Z * v.Z + W * v.W;
        public float Length => (float)Math.Sqrt(Dot(this));
        public Float4 Normalize => this / Length;
    }
}