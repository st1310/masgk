using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace SseMath
{
    public struct Float3
    {
        public float X;
        public float Y;
        public float Z;
        private float W;

        public Float3(float x, float y, float z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.W = 0.0f;
        }

        // Operator overloading
        public static Float3 operator +(Float3 a) => a;
        public static unsafe Float3 operator -(Float3 a)
        {
            Sse.Store(&a.X, Sse.Xor(Sse.LoadVector128(&a.X), Vector128.Create(-0.0f)));

            return a;
        }
        public static unsafe Float3 operator +(Float3 a, Float3 b)
        {
            Sse.Store(&a.X, Sse.Add(Sse.LoadVector128(&a.X), Sse.LoadVector128(&b.X)));

            return a;
        }
        public static unsafe Float3 operator -(Float3 a, Float3 b)
        {
            Sse.Store(&a.X, Sse.Subtract(Sse.LoadVector128(&a.X), Sse.LoadVector128(&b.X)));

            return a;
        }
        public static unsafe Float3 operator +(Float3 a, float b)
        {
            Sse.Store(&a.X, Sse.Add(Sse.LoadVector128(&a.X), Vector128.Create(b)));

            return a;
        }
        public static unsafe Float3 operator -(Float3 a, float b)
        {
            Sse.Store(&a.X, Sse.Subtract(Sse.LoadVector128(&a.X), Vector128.Create(b)));

            return a;
        }
        public static unsafe Float3 operator *(Float3 a, Float3 b)
        {
            Sse.Store(&a.X, Sse.Multiply(Sse.LoadVector128(&a.X), Sse.LoadVector128(&b.X)));

            return a;
        }
        public static unsafe Float3 operator /(Float3 a, Float3 b)
        {
            Sse.Store(&a.X, Sse.Divide(Sse.LoadVector128(&a.X), Sse.LoadVector128(&b.X)));

            return a;
        }
        public static unsafe Float3 operator *(Float3 a, float b)
        {
            Sse.Store(&a.X, Sse.Multiply(Sse.LoadVector128(&a.X), Vector128.Create(b)));

            return a;
        }
        public static unsafe Float3 operator /(Float3 a, float b)
        {
            Sse.Store(&a.X, Sse.Divide(Sse.LoadVector128(&a.X), Vector128.Create(b)));

            return a;
        }

        public static byte _MM_SHUFFLE(int z, int y, int x, int w) => (byte)((z << 6) | (y << 4) | (x << 2) | w);

        public static unsafe Float3 Cross(Float3 a, Float3 b)
        {
            Vector128<float> va = Sse.LoadVector128(&a.X);
            Vector128<float> vb = Sse.LoadVector128(&b.X);

            Vector128<float> r =
                Sse.Subtract(Sse.Multiply(Sse.Shuffle(va, va, _MM_SHUFFLE(3, 0, 2, 1)), Sse.Shuffle(vb, vb, _MM_SHUFFLE(3, 1, 0, 2))),
                            Sse.Multiply(Sse.Shuffle(va, va, _MM_SHUFFLE(3, 1, 0, 2)), Sse.Shuffle(vb, vb, _MM_SHUFFLE(3, 0, 2, 1))));

            Sse.Store(&a.X, r);

            return a;
        }

        //public static unsafe Float3 Cross(Float3 a, Float3 b)
        //{
        //    Vector128<float> va = Sse.LoadVector128(&a.X);
        //    Vector128<float> vb = Sse.LoadVector128(&b.X);

        //    Vector128<float> r =
        //        Sse.Subtract(Sse.Multiply(vb, Sse.Shuffle(va, va, _MM_SHUFFLE(3, 0, 2, 1))),
        //                    Sse.Multiply(va, Sse.Shuffle(vb, vb, _MM_SHUFFLE(3, 0, 2, 1))));

        //    Sse.Store(&a.X, Sse.Shuffle(r, r, _MM_SHUFFLE(3, 0, 2, 1)));

        //    return a;
        //}

        public static unsafe float Dot(Float3 a, Float3 b) => Sse41.DotProduct(Sse.LoadVector128(&a.X), Sse.LoadVector128(&b.X), 0xFF).ToScalar();
        //public static unsafe float Dot(Float3 a, Float3 b)
        //{
        //    Vector128<float> va = Sse.LoadVector128(&a.X);
        //    Vector128<float> vb = Sse.LoadVector128(&b.X);

        //    Vector128<float> r = Sse.Multiply(va, vb);
        //    r = Sse3.HorizontalAdd(r, r);
        //    r = Sse3.HorizontalAdd(r, r);

        //    return r.ToScalar();
        //}

        // 0x7F = 0111 1111 ~ means we don't want the w-component multiplied
        // and the result written to all 4 components
        public static unsafe Float3 Dot(Float3 a, Float3 b, byte control = 0x7F)
        {
            Sse.Store(&a.X, Sse41.DotProduct(Sse.LoadVector128(&a.X), Sse.LoadVector128(&b.X), control));

            return a;
        }
        public static unsafe float Length(Float3 a, Float3 b) => Sse.SqrtScalar(Sse41.DotProduct(Sse.LoadVector128(&a.X), Sse.LoadVector128(&b.X), 0xFF)).ToScalar();
        public static unsafe Float3 Normalize(Float3 a)
        {
            Vector128<float> v = Sse.LoadVector128(&a.X);
            Sse.Store(&a.X,
                Sse.Multiply(Sse.LoadVector128(&a.X),
                            Sse.ReciprocalSqrt(Sse41.DotProduct(v, v, 0x7F))));

            return a;
        }

        public static unsafe Float3 Saturate(Float3 a)
        {
            Sse.Store(&a.X, Sse.Max(Sse.Min(Sse.LoadVector128(&a.X), Vector128.Create(255.0f)), Vector128.Create(0.0f)));

            return a;
        }

        public static unsafe Float3 Reflect(Float3 I, Float3 N)
        {
            Float3 Nn = Normalize(N);

            return I - Nn * Dot(Nn, I) * 2.0f;
        }

        public override string ToString()
        {
            return $"[{X}, {Y}, {Z}]";
        }
    }
}
