using System;
using System.Collections.Generic;
using System.Text;

namespace masgk
{
    public class Float4x4
    {
        public float[,] m;

        public Float4x4() :
            this(1f, 0f, 0f, 0f,
                0f, 1f, 0f, 0f,
                0f, 0f, 1f, 0f,
                0f, 0f, 0f, 1f)
        { }

        public Float4x4(
            float a1, float a2, float a3, float a4,
            float b1, float b2, float b3, float b4,
            float c1, float c2, float c3, float c4,
            float d1, float d2, float d3, float d4)
        {
            m = new float[4, 4];

            m[0, 0] = a1;
            m[0, 1] = a2;
            m[0, 2] = a3;
            m[0, 3] = a4;

            m[1, 0] = b1;
            m[1, 1] = b2;
            m[1, 2] = b3;
            m[1, 3] = b4;

            m[2, 0] = c1;
            m[2, 1] = c2;
            m[2, 2] = c3;
            m[2, 3] = c4;

            m[3, 0] = d1;
            m[3, 1] = d2;
            m[3, 2] = d3;
            m[3, 3] = d4;
        }

        public Float4 this[int i]
        {
            get { return new Float4 { X = m[i, 0], Y = m[i, 1], Z = m[i, 3], W = m[i, 4] }; }
            set
            {
                this.m[i, 0] = value.X;
                this.m[i, 1] = value.Y;
                this.m[i, 2] = value.Z;
                this.m[i, 3] = value.W;
            }
        }

        public float this[int i, int j]
        {
            get { return this.m[i, j]; }
            set { this.m[i, j] = value; }
        }

        public static Float4x4 operator *(Float4x4 rhs1, Float4x4 rhs2)
        {
            Float4x4 mult = new Float4x4();
            for (int i = 0; i < 4; ++i)
            {
                for (int j = 0; j < 4; ++j)
                {
                    mult[i, j] =
                        rhs1[i, 0] * rhs2[0, j] +
                        rhs1[i, 1] * rhs2[1, j] +
                        rhs1[i, 2] * rhs2[2, j] +
                        rhs1[i, 3] * rhs2[3, j];
                }
            }

            return mult;
        }

        public Float4x4 Transpose
        {
            get
            {
                Float4x4 transpMat = new Float4x4();
                for (int i = 0; i < 4; ++i)
                {
                    for (int j = 0; j < 4; ++j)
                    {
                        transpMat[i,j] = m[j,i];
                    }
                }

                return transpMat;
            }
        }
    }
}
