using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace SseMath
{
    public struct Float4x4
    {
        private static readonly Float4x4 _identity = new Float4x4
        (
            1f, 0f, 0f, 0f,
            0f, 1f, 0f, 0f,
            0f, 0f, 1f, 0f,
            0f, 0f, 0f, 1f
        );

        /// <summary>Value at row 1, column 1 of the matrix.</summary>
        public float M11;

        /// <summary>Value at row 1, column 2 of the matrix.</summary>
        public float M12;

        /// <summary>Value at row 1, column 3 of the matrix.</summary>
        public float M13;

        /// <summary>Value at row 1, column 4 of the matrix.</summary>
        public float M14;

        /// <summary>Value at row 2, column 1 of the matrix.</summary>
        public float M21;

        /// <summary>Value at row 2, column 2 of the matrix.</summary>
        public float M22;

        /// <summary>Value at row 2, column 3 of the matrix.</summary>
        public float M23;

        /// <summary>Value at row 2, column 4 of the matrix.</summary>
        public float M24;

        /// <summary>Value at row 3, column 1 of the matrix.</summary>
        public float M31;

        /// <summary>Value at row 3, column 2 of the matrix.</summary>
        public float M32;

        /// <summary>Value at row 3, column 3 of the matrix.</summary>
        public float M33;

        /// <summary>Value at row 3, column 4 of the matrix.</summary>
        public float M34;

        /// <summary>Value at row 4, column 1 of the matrix.</summary>
        public float M41;

        /// <summary>Value at row 4, column 2 of the matrix.</summary>
        public float M42;

        /// <summary>Value at row 4, column 3 of the matrix.</summary>
        public float M43;

        /// <summary>Value at row 4, column 4 of the matrix.</summary>
        public float M44;

        /// <summary>Constructs a Matrix4x4 from the given components.</summary>
        public Float4x4(float m11, float m12, float m13, float m14,
                         float m21, float m22, float m23, float m24,
                         float m31, float m32, float m33, float m34,
                         float m41, float m42, float m43, float m44)
        {
            M11 = m11;
            M12 = m12;
            M13 = m13;
            M14 = m14;

            M21 = m21;
            M22 = m22;
            M23 = m23;
            M24 = m24;

            M31 = m31;
            M32 = m32;
            M33 = m33;
            M34 = m34;

            M41 = m41;
            M42 = m42;
            M43 = m43;
            M44 = m44;
        }

        public static Float4x4 Identity
        {
            get => _identity;
        }

        public Float4 this[int i]
        {
            get => new Float4 { X = this[i, 0], Y = this[i, 1], Z = this[i, 2], W = this[i, 3] };
            set
            {
                this[i, 0] = value.X;
                this[i, 1] = value.Y;
                this[i, 2] = value.Z;
                this[i, 3] = value.W;
            }
        }

        public float this[int i, int j]
        {
            get
            {
                return (i * 4 + j) switch
                {
                    0 => M11,
                    1 => M12,
                    2 => M13,
                    3 => M14,
                    4 => M21,
                    5 => M22,
                    6 => M23,
                    7 => M24,
                    8 => M31,
                    9 => M32,
                    10 => M33,
                    11 => M34,
                    12 => M41,
                    13 => M42,
                    14 => M43,
                    15 => M44,
                    _ => throw new IndexOutOfRangeException("Invalid matrix index!"),
                };
            }

            set
            {
                switch (i * 4 + j)
                {
                    case 0: M11 = value; break;
                    case 1: M12 = value; break;
                    case 2: M13 = value; break;
                    case 3: M14 = value; break;
                    case 4: M21 = value; break;
                    case 5: M22 = value; break;
                    case 6: M23 = value; break;
                    case 7: M24 = value; break;
                    case 8: M31 = value; break;
                    case 9: M32 = value; break;
                    case 10: M33 = value; break;
                    case 11: M34 = value; break;
                    case 12: M41 = value; break;
                    case 13: M42 = value; break;
                    case 14: M43 = value; break;
                    case 15: M44 = value; break;
                    default:
                        throw new IndexOutOfRangeException("Invalid matrix index!");
                }
            }
        }

        public static unsafe Float4x4 operator *(Float4x4 rhs1, Float4x4 rhs2)
        {
            Vector128<float> row = Sse.LoadVector128(&rhs1.M11);
            Sse.Store(&rhs1.M11,
                Sse.Add(Sse.Add(Sse.Multiply(Sse.Shuffle(row, row, 0x00), Sse.LoadVector128(&rhs2.M11)),
                                Sse.Multiply(Sse.Shuffle(row, row, 0x55), Sse.LoadVector128(&rhs2.M21))),
                        Sse.Add(Sse.Multiply(Sse.Shuffle(row, row, 0xAA), Sse.LoadVector128(&rhs2.M31)),
                                Sse.Multiply(Sse.Shuffle(row, row, 0xFF), Sse.LoadVector128(&rhs2.M41)))));

            row = Sse.LoadVector128(&rhs1.M21);
            Sse.Store(&rhs1.M21,
                Sse.Add(Sse.Add(Sse.Multiply(Sse.Shuffle(row, row, 0x00), Sse.LoadVector128(&rhs2.M11)),
                                Sse.Multiply(Sse.Shuffle(row, row, 0x55), Sse.LoadVector128(&rhs2.M21))),
                        Sse.Add(Sse.Multiply(Sse.Shuffle(row, row, 0xAA), Sse.LoadVector128(&rhs2.M31)),
                                Sse.Multiply(Sse.Shuffle(row, row, 0xFF), Sse.LoadVector128(&rhs2.M41)))));

            row = Sse.LoadVector128(&rhs1.M31);
            Sse.Store(&rhs1.M31,
                Sse.Add(Sse.Add(Sse.Multiply(Sse.Shuffle(row, row, 0x00), Sse.LoadVector128(&rhs2.M11)),
                                Sse.Multiply(Sse.Shuffle(row, row, 0x55), Sse.LoadVector128(&rhs2.M21))),
                        Sse.Add(Sse.Multiply(Sse.Shuffle(row, row, 0xAA), Sse.LoadVector128(&rhs2.M31)),
                                Sse.Multiply(Sse.Shuffle(row, row, 0xFF), Sse.LoadVector128(&rhs2.M41)))));

            row = Sse.LoadVector128(&rhs1.M41);
            Sse.Store(&rhs1.M41,
                Sse.Add(Sse.Add(Sse.Multiply(Sse.Shuffle(row, row, 0x00), Sse.LoadVector128(&rhs2.M11)),
                                Sse.Multiply(Sse.Shuffle(row, row, 0x55), Sse.LoadVector128(&rhs2.M21))),
                        Sse.Add(Sse.Multiply(Sse.Shuffle(row, row, 0xAA), Sse.LoadVector128(&rhs2.M31)),
                                Sse.Multiply(Sse.Shuffle(row, row, 0xFF), Sse.LoadVector128(&rhs2.M41)))));
            return rhs1;
        }

        public static unsafe Float4x4 Transpose(Float4x4 matrix)
        {
            Vector128<float> row1 = Sse.LoadVector128(&matrix.M11);
            Vector128<float> row2 = Sse.LoadVector128(&matrix.M21);
            Vector128<float> row3 = Sse.LoadVector128(&matrix.M31);
            Vector128<float> row4 = Sse.LoadVector128(&matrix.M41);

            Vector128<float> l12 = Sse.UnpackLow(row1, row2);
            Vector128<float> l34 = Sse.UnpackLow(row3, row4);
            Vector128<float> h12 = Sse.UnpackHigh(row1, row2);
            Vector128<float> h34 = Sse.UnpackHigh(row3, row4);

            Sse.Store(&matrix.M11, Sse.MoveLowToHigh(l12, l34));
            Sse.Store(&matrix.M21, Sse.MoveHighToLow(l34, l12));
            Sse.Store(&matrix.M31, Sse.MoveLowToHigh(h12, h34));
            Sse.Store(&matrix.M41, Sse.MoveHighToLow(h34, h12));

            return matrix;
        }

        public override string ToString()
        {
            return $"[{M11}, {M12}, {M13}, {M14}]\n[{M21}, {M22}, {M23}, {M24}]\n[{M31}, {M32}, {M33}, {M34}]\n[{M41}, {M42}, {M43}, {M44}]";
        }
    }
}
