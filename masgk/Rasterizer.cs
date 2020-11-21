using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace masgk
{
    public class Rasterizer
    {
        readonly Buffer buff;
        readonly DepthBuffer depthBuff;

        public Rasterizer(Buffer buff, DepthBuffer depthBuff)
        {
            this.buff = buff;
            this.depthBuff = depthBuff;
        }

        public void Triangle(Float3 v1, Float3 v2, Float3 v3, Float3 c1, Float3 c2, Float3 c3)
        {
            float x1 = (v1.X + 1) * buff.width * .5f;
            float y1 = (v1.Y + 1) * buff.height * .5f;
            float x2 = (v2.X + 1) * buff.width * .5f;
            float y2 = (v2.Y + 1) * buff.height * .5f;
            float x3 = (v3.X + 1) * buff.width * .5f;
            float y3 = (v3.Y + 1) * buff.height * .5f;

            int minx = Min(x1, x2, x3);
            int miny = Min(y1, y2, y3);
            int maxx = Max(x1, x2, x3);
            int maxy = Max(y1, y2, y3);

            minx = Math.Max(minx, 0);
            maxx = Math.Min(maxx, buff.width - 1);
            miny = Math.Max(miny, 0);
            maxy = Math.Min(maxy, buff.height - 1);

            float dy12 = y1 - y2;
            float dy23 = y2 - y3;
            float dy31 = y3 - y1;
            float dx12 = x1 - x2;
            float dx23 = x2 - x3;
            float dx31 = x3 - x1;

            bool tl1 = false;
            bool tl2 = false;
            bool tl3 = false;

            if (dy12 < 0 || (dy12 == 0 && dx12 > 0)) tl1 = true;
            if (dy23 < 0 || (dy23 == 0 && dx23 > 0)) tl2 = true;
            if (dy31 < 0 || (dy31 == 0 && dx31 > 0)) tl3 = true;

            for (int x = minx; x <= maxx; x++)
                for (int y = miny; y <= maxy; y++)
                {
                    float hs1 = dx12 * (y - y1) - dy12 * (x - x1);
                    float hs2 = dx23 * (y - y2) - dy23 * (x - x2);
                    float hs3 = dx31 * (y - y3) - dy31 * (x - x3);

                    if (((hs1 > 0 && !tl1) || (hs1 >= 0 && tl1)) &&
                        ((hs2 > 0 && !tl2) || (hs2 >= 0 && tl2)) &&
                        ((hs3 > 0 && !tl3) || (hs3 >= 0 && tl3)))
                    {
                        float l1 = (((y2 - y3) * (x - x3)) + ((x3 - x2) * (y - y3))) /
                                    (((y2 - y3) * (x1 - x3)) + ((x3 - x2) * (y1 - y3)));
                        float l2 = (((y3 - y1) * (x - x3)) + ((x1 - x3) * (y - y3))) /
                                    (((y3 - y1) * (x2 - x3)) + ((x1 - x3) * (y2 - y3)));
                        float l3 = 1 - l1 - l2;

                        float depth = l1 * v1.Z + l2 * v2.Z + l3 * v3.Z;

                        if (depth < depthBuff.GetDepth(x, y))
                        {
                            Float3 col = c1 * l1 + c2 * l2 + c3 * l3;
                            buff.SetPixel(x, y, (Color)col);
                            depthBuff.SetDepth(x, y, depth);
                        }
                    }
                }
        }

        public static int Min(float a, float b, float c) => (int)Math.Min(a, Math.Min(b, c));
        public static int Max(float a, float b, float c) => (int)Math.Max(a, Math.Max(b, c));
    }
}
