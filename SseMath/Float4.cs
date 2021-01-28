using System;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace SseMath
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

        // Operator overloading
        public static Float4 operator +(Float4 a) => a;
        public static unsafe Float4 operator -(Float4 a)
        {
            Sse.Store(&a.X, Sse.Xor(Sse.LoadVector128(&a.X), Vector128.Create(-0.0f)));

            return a;
        }
        public static unsafe Float4 operator +(Float4 a, Float4 b)
        {
            Sse.Store(&a.X, Sse.Add(Sse.LoadVector128(&a.X), Sse.LoadVector128(&b.X)));

            return a;
        }
        public static unsafe Float4 operator -(Float4 a, Float4 b)
        {
            Sse.Store(&a.X, Sse.Subtract(Sse.LoadVector128(&a.X), Sse.LoadVector128(&b.X)));

            return a;
        }
        public static unsafe Float4 operator +(Float4 a, float b)
        {
            Sse.Store(&a.X, Sse.Add(Sse.LoadVector128(&a.X), Vector128.Create(b)));

            return a;
        }
        public static unsafe Float4 operator -(Float4 a, float b)
        {
            Sse.Store(&a.X, Sse.Subtract(Sse.LoadVector128(&a.X), Vector128.Create(b)));

            return a;
        }
        public static unsafe Float4 operator *(Float4 a, Float4 b)
        {
            Sse.Store(&a.X, Sse.Multiply(Sse.LoadVector128(&a.X), Sse.LoadVector128(&b.X)));

            return a;
        }
        public static unsafe Float4 operator /(Float4 a, Float4 b)
        {
            Sse.Store(&a.X, Sse.Divide(Sse.LoadVector128(&a.X), Sse.LoadVector128(&b.X)));

            return a;
        }
        public static unsafe Float4 operator *(Float4 a, float b)
        {
            Sse.Store(&a.X, Sse.Multiply(Sse.LoadVector128(&a.X), Vector128.Create(b)));

            return a;
        }
        public static unsafe Float4 operator /(Float4 a, float b)
        {
            Sse.Store(&a.X, Sse.Divide(Sse.LoadVector128(&a.X), Vector128.Create(b)));

            return a;
        }

        public static unsafe Float4 operator *(Float4x4 a, Float4 b) =>
            new Float4
            {
                X = Sse41.DotProduct(Sse.LoadVector128(&a.M11), Sse.LoadVector128(&b.X), 0xFF).ToScalar(),
                Y = Sse41.DotProduct(Sse.LoadVector128(&a.M21), Sse.LoadVector128(&b.X), 0xFF).ToScalar(),
                Z = Sse41.DotProduct(Sse.LoadVector128(&a.M31), Sse.LoadVector128(&b.X), 0xFF).ToScalar(),
                W = Sse41.DotProduct(Sse.LoadVector128(&a.M41), Sse.LoadVector128(&b.X), 0xFF).ToScalar()
            };

        public static unsafe float Dot(Float4 a, Float4 b) => Sse41.DotProduct(Sse.LoadVector128(&a.X), Sse.LoadVector128(&b.X), 0xFF).ToScalar();

        // 0x7F = 0111 1111 ~ means we don't want the w-component multiplied
        // and the result written to all 4 components
        public static unsafe Float4 Dot(Float4 a, Float4 b, byte control = 0x7F)
        {
            Sse.Store(&a.X, Sse41.DotProduct(Sse.LoadVector128(&a.X), Sse.LoadVector128(&b.X), control));

            return a;
        }
        public static unsafe float Length(Float4 a, Float4 b) => Sse.SqrtScalar(Sse41.DotProduct(Sse.LoadVector128(&a.X), Sse.LoadVector128(&b.X), 0xFF)).ToScalar();
        public static unsafe Float4 Normalize(Float4 a)
        {
            Vector128<float> v = Sse.LoadVector128(&a.X);
            Sse.Store(&a.X,
                Sse.Multiply(Sse.LoadVector128(&a.X),
                            Sse.ReciprocalSqrt(Sse41.DotProduct(v, v, 0x7F))));

            return a;
        }

        public override string ToString()
        {
            return $"[{X}, {Y}, {Z}, {W}]";
        }
    }
}
