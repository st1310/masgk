using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace masgk
{
    public struct Float3
    {
        public float X;
        public float Y;
        public float Z;

        public Float3(float x, float y, float z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        // Operator overloading
        public static Float3 operator +(Float3 a) => a;
        public static Float3 operator -(Float3 a) => new Float3(-a.X, -a.Y, -a.Z);
        public static Float3 operator +(Float3 a, Float3 b) => new Float3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        public static Float3 operator -(Float3 a, Float3 b) => a + (-b);
        public static Float3 operator *(Float3 a, float b) => new Float3(a.X * b, a.Y * b, a.Z * b);
        public static Float3 operator /(Float3 a, float b) => b == 0 ? throw new DivideByZeroException() : new Float3(a.X / b, a.Y / b, a.Z / b);

        // Conversion operators 
        public static explicit operator Color(Float3 a) => Color.FromArgb((int)a.X, (int)a.Y, (int)a.Z);
        public static explicit operator Float3(Color a) => new Float3(a.R, a.G, a.B);

        public Float3 Cross(Float3 v) =>
            new Float3
            {
                X = Y * v.Z - Z * v.Y,
                Y = Z * v.X - X * v.Z,
                Z = X * v.Y - Y * v.X
            };
        public float Dot(Float3 v) => X * v.X + Y * v.Y + Z * v.Z;
        public float Length => (float)Math.Sqrt(Dot(this));
        public Float3 Normalize => this / Length;
    }
}
