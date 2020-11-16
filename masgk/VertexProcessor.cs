using System;
using System.Collections.Generic;
using System.Text;

namespace masgk
{
    public class VertexProcessor
    {
        public Float4x4 obj2world { get; set; } = new Float4x4();
        public Float4x4 world2view { get; set; } = new Float4x4();
        public Float4x4 view2proj { get; set; } = new Float4x4();

        public Float4x4 obj2proj { get; set; } = new Float4x4();

        public VertexProcessor()
        {
            SetPerspective(45f, 1.0f, 0.1f, 1000.0f);

            Float3 eye = new Float3(0f, 0f, 3f);  // camera position
            Float3 center = new Float3(0, 0, 0); // target position
            Float3 up = new Float3(0, 1, 0); // up vector in world space
            SetLookAt(eye, center, up);

            obj2proj = view2proj * world2view * obj2world;
        }

        public Float3 Tr(Float3 v)
        {
            Float4 u = obj2proj * new Float4(v, 1f);

            return new Float3(u.X, u.Y, u.Z) / u.W;
        }

        public void Lt()
        {
            obj2proj = view2proj * world2view * obj2world;
        }

        public void SetPerspective(double fovy, float aspect, float near, float far)
        {
            fovy *= Math.PI / 360; // radians
            double f = Math.Cos(fovy) / Math.Sin(fovy);

            view2proj[0] = new Float4((float)f / aspect, 0f, 0f, 0f);
            view2proj[1] = new Float4(0f, (float)f, 0f, 0f);
            view2proj[2] = new Float4(0f, 0f, (far + near) / (near - far), -1f);
            view2proj[3] = new Float4(0f, 0f, (2 * far * near) / (near - far), 0f);

            view2proj = view2proj.Transpose;
        }

        public void SetLookAt(Float3 eye, Float3 center, Float3 up)
        {
            Float3 f = center - eye; // -direction
            f = f.Normalize;
            up = up.Normalize;
            Float3 s = f.Cross(up); // right
            Float3 u = s.Cross(f); // up

            world2view[0] = new Float4(s.X, u.X, -f.X, 0);
            world2view[1] = new Float4(s.Y, u.Y, -f.Y, 0);
            world2view[2] = new Float4(s.Z, u.Z, -f.Z, 0);
            world2view[3] = new Float4(0, 0, 0, 1);

            Float4x4 m = new Float4x4();
            m[3] = new Float4(-eye, 1f);
            world2view *= m;

            world2view = world2view.Transpose;
        }

        public void MultByTrans(Float3 v)
        {
            Float4x4 m = new Float4x4();
            m[3] = new Float4(v, 1f);
            m = m.Transpose;

            obj2world = m * obj2world;
        }

        public void MultByScale(Float3 v)
        {
            Float4x4 m = new Float4x4();
            m[0] = new Float4(v.X, 0f, 0f, 0f);
            m[1] = new Float4(0f, v.Y, 0f, 0f);
            m[2] = new Float4(0f, 0f, v.Z, 0f);

            obj2world = m * obj2world;
        }

        public void MultByRot(double a, Float3 v)
        {
            float s = (float)Math.Sin(a * (Math.PI / 180));
            float c = (float)Math.Cos(a * (Math.PI / 180));
            v = v.Normalize;

            Float4x4 m = new Float4x4();
            m[0] = new Float4(v.X * v.X * (1 - c) + c,       v.Y * v.X * (1 - c) + v.Z * s, v.X * v.Z * (1 - c) - v.Y * s, 0);
            m[1] = new Float4(v.X * v.Y * (1 - c) - v.Z * s, v.Y * v.Y * (1 - c) + c,       v.Y * v.Z * (1 - c) + v.X * s, 0);
            m[2] = new Float4(v.X * v.Z * (1 - c) + v.Y * s, v.Y * v.Z * (1 - c) - v.X * s, v.Z * v.Z * (1 - c) + c,       0);
            m = m.Transpose;

            obj2world = m * obj2world;
        }
    }
}
