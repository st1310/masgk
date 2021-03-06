﻿using System;
using System.Collections.Generic;
using System.Text;

namespace masgk
{
    public class VertexProcessor
    {
        public Float4x4 obj2world = Float4x4.Identity;
        public Float4x4 world2view = Float4x4.Identity;
        public Float4x4 view2proj = Float4x4.Identity;

        public Float4x4 obj2view = Float4x4.Identity;
        public Float4x4 obj2proj = Float4x4.Identity;

        public VertexProcessor()
        {
            SetPerspective(45f, 1.0f, 0.1f, 1000.0f);

            Float3 eye = new Float3(0f, 0f, 3f);  // camera position
            Float3 center = new Float3(0f, 0f, 0f); // target position
            Float3 up = new Float3(0f, 1f, 0f); // up vector in world space
            SetLookAt(eye, center, up);

            obj2proj = view2proj * world2view * obj2world;
        }

        public Float3 Tr(Float3 v)
        {
            Float4 u = obj2proj * new Float4(v, 1f);

            return new Float3(u.X, u.Y, u.Z) / u.W;
        }

        public Float3 Tr_obj2view4(Float3 v)
        {
            Float4 u = obj2view * new Float4(v, 1f);

            return new Float3(u.X, u.Y, u.Z) / u.W;
        }

        public Float3 Tr_obj2view3(Float3 v)
        {
            Float4 u = obj2view * new Float4(v, 0f); //! Float4x4 * Float3

            return new Float3(u.X, u.Y, u.Z);
        }

        public void Lt()
        {
            obj2view = world2view * obj2world;
            obj2proj = view2proj * world2view * obj2world;
        }

        public void SetPerspective(double fovy, float aspect, float near, float far)
        {
            fovy *= Math.PI / 360d; // radians
            double f = Math.Cos(fovy) / Math.Sin(fovy);

            view2proj[0] = new Float4((float)f / aspect, 0f, 0f, 0f);
            view2proj[1] = new Float4(0f, (float)f, 0f, 0f);
            view2proj[2] = new Float4(0f, 0f, (far + near) / (near - far), -1f);
            view2proj[3] = new Float4(0f, 0f, (2f * far * near) / (near - far), 0f);

            view2proj = Float4x4.Transpose(view2proj);
        }

        public void SetLookAt(Float3 eye, Float3 center, Float3 up)
        {
            Float3 f = center - eye; // -direction
            f = f.Normalize;
            up = up.Normalize;
            Float3 s = f.Cross(up); // right
            Float3 u = s.Cross(f); // up

            world2view[0] = new Float4(s.X, u.X, -f.X, 0f);
            world2view[1] = new Float4(s.Y, u.Y, -f.Y, 0f);
            world2view[2] = new Float4(s.Z, u.Z, -f.Z, 0f);
            world2view[3] = new Float4(0f, 0f, 0f, 1f);

            Float4x4 m = Float4x4.Identity;
            m[3] = new Float4(-eye, 1f);
            world2view *= m;

            world2view = Float4x4.Transpose(world2view);
        }

        public void MultByTrans(Float3 v)
        {
            Float4x4 m = Float4x4.Identity;
            m[3] = new Float4(v, 1f);
            m = Float4x4.Transpose(m);

            obj2world = m * obj2world;
        }

        public void MultByScale(Float3 v)
        {
            Float4x4 m = Float4x4.Identity;
            m[0] = new Float4(v.X, 0f, 0f, 0f);
            m[1] = new Float4(0f, v.Y, 0f, 0f);
            m[2] = new Float4(0f, 0f, v.Z, 0f);

            obj2world = m * obj2world;
        }

        public void MultByRot(double a, Float3 v)
        {
            float s = (float)Math.Sin(a * (Math.PI / 180d));
            float c = (float)Math.Cos(a * (Math.PI / 180d));
            v = v.Normalize;

            Float4x4 m = Float4x4.Identity;
            m[0] = new Float4(v.X * v.X * (1f - c) + c,       v.Y * v.X * (1f - c) + v.Z * s, v.X * v.Z * (1f - c) - v.Y * s, 0f);
            m[1] = new Float4(v.X * v.Y * (1f - c) - v.Z * s, v.Y * v.Y * (1f - c) + c,       v.Y * v.Z * (1f - c) + v.X * s, 0f);
            m[2] = new Float4(v.X * v.Z * (1f - c) + v.Y * s, v.Y * v.Z * (1f - c) - v.X * s, v.Z * v.Z * (1f - c) + c,       0f);
            m = Float4x4.Transpose(m);

            obj2world = m * obj2world;
        }
    }
}
