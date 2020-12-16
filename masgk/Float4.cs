using System;
using System.Collections.Generic;
using System.Text;

namespace masgk
{
    public struct Float4
    {
        public float X;
        public float Y;
        public float Z;
        public float W;

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
        public static Float4 operator +(Float4 a, float b) => new Float4(a.X + b, a.Y + b, a.Z + b, a.W + b);
        public static Float4 operator -(Float4 a, float b) => a + (-b);
        public static Float4 operator *(Float4 a, float b) => new Float4(a.X * b, a.Y * b, a.Z * b, a.W * b);
        public static Float4 operator /(Float4 a, float b) => b == 0 ? throw new DivideByZeroException() : new Float4(a.X / b, a.Y / b, a.Z / b, a.W / b);

        public static Float4 operator *(Float4x4 a, Float4 b) =>
            new Float4
            {
                X = a.M11 * b.X + a.M12 * b.Y + a.M13 * b.Z + a.M14 * b.W,
                Y = a.M21 * b.X + a.M22 * b.Y + a.M23 * b.Z + a.M24 * b.W,
                Z = a.M31 * b.X + a.M32 * b.Y + a.M33 * b.Z + a.M34 * b.W,
                W = a.M41 * b.X + a.M42 * b.Y + a.M43 * b.Z + a.M44 * b.W
            };

        public float Dot(Float4 v) => X * v.X + Y * v.Y + Z * v.Z + W * v.W;
        public float Length => (float)Math.Sqrt(Dot(this));
        public Float4 Normalize => this / Length;
    }
}
