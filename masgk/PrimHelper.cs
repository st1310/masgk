using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace masgk
{
    public static class PrimHelper
    {
        public static void Cube(Rasterizer rast, VertexProcessor proc)
        {
            rast.Triangle(
                proc.Tr(new Float3(-1f, -1f, 1f)),
                proc.Tr(new Float3(1f, 1f, 1f)),
                proc.Tr(new Float3(1f, -1f, 1f)),
                (Float3)Color.Red, (Float3)Color.Red, (Float3)Color.Red);
            rast.Triangle(
                proc.Tr(new Float3(-1f, -1f, 1f)),
                proc.Tr(new Float3(-1f, 1f, 1f)),
                proc.Tr(new Float3(1f, 1f, 1f)),
                (Float3)Color.Red, (Float3)Color.Red, (Float3)Color.Red);
            rast.Triangle(
                proc.Tr(new Float3(1f, -1f, 1f)),
                proc.Tr(new Float3(1f, 1f, -1f)),
                proc.Tr(new Float3(1f, -1f, -1f)),
                (Float3)Color.Blue, (Float3)Color.Blue, (Float3)Color.Blue);
            rast.Triangle(
                proc.Tr(new Float3(1f, -1f, 1f)),
                proc.Tr(new Float3(1f, 1f, 1f)),
                proc.Tr(new Float3(1f, 1f, -1f)),
                (Float3)Color.Blue, (Float3)Color.Blue, (Float3)Color.Blue);
            rast.Triangle(
                proc.Tr(new Float3(1f, -1f, -1f)),
                proc.Tr(new Float3(-1f, 1f, -1f)),
                proc.Tr(new Float3(-1f, -1f, -1f)),
                (Float3)Color.Red, (Float3)Color.Red, (Float3)Color.Red);
            rast.Triangle(
                proc.Tr(new Float3(1f, -1f, -1f)),
                proc.Tr(new Float3(1f, 1f, -1f)),
                proc.Tr(new Float3(-1f, 1f, -1f)),
                (Float3)Color.Red, (Float3)Color.Red, (Float3)Color.Red);
            rast.Triangle(
                proc.Tr(new Float3(-1f, -1f, -1f)),
                proc.Tr(new Float3(-1f, 1f, 1f)),
                proc.Tr(new Float3(-1f, -1f, 1f)),
                (Float3)Color.Blue, (Float3)Color.Blue, (Float3)Color.Blue);
            rast.Triangle(
                proc.Tr(new Float3(-1f, -1f, -1f)),
                proc.Tr(new Float3(-1f, 1f, -1f)),
                proc.Tr(new Float3(-1f, 1f, 1f)),
                (Float3)Color.Blue, (Float3)Color.Blue, (Float3)Color.Blue);
            rast.Triangle(
                proc.Tr(new Float3(-1f, -1f, 1f)),
                proc.Tr(new Float3(1f, -1f, -1f)),
                proc.Tr(new Float3(-1f, -1f, -1f)),
                (Float3)Color.Green, (Float3)Color.Green, (Float3)Color.Green);
            rast.Triangle(
                proc.Tr(new Float3(-1f, -1f, 1f)),
                proc.Tr(new Float3(1f, -1f, 1f)),
                proc.Tr(new Float3(1f, -1f, -1f)),
                (Float3)Color.Green, (Float3)Color.Green, (Float3)Color.Green);
            rast.Triangle(
                proc.Tr(new Float3(1f, 1f, 1f)),
                proc.Tr(new Float3(-1f, 1f, -1f)),
                proc.Tr(new Float3(1f, 1f, -1f)),
                (Float3)Color.Green, (Float3)Color.Green, (Float3)Color.Green);
            rast.Triangle(
                proc.Tr(new Float3(1f, 1f, 1f)),
                proc.Tr(new Float3(-1f, 1f, 1f)),
                proc.Tr(new Float3(-1f, 1f, -1f)),
                (Float3)Color.Green, (Float3)Color.Green, (Float3)Color.Green);
        }

        public static void TestTriangles(Rasterizer rast, VertexProcessor proc)
        {
            rast.Triangle(
                proc.Tr(new Float3(-0.5f, -0.5f, 0f)),
                proc.Tr(new Float3(0f, 0.5f, 0f)),
                proc.Tr(new Float3(0.5f, -0.5f, 0f)),
                (Float3)Color.Red, (Float3)Color.Blue, (Float3)Color.Green);
            rast.Triangle(
                proc.Tr(new Float3(-1.1f, 0.5f, 0.9f)),
                proc.Tr(new Float3(0.6f, 0f, -0.1f)),
                proc.Tr(new Float3(-0.9f, -1.1f, 0.5f)),
                (Float3)Color.Red, (Float3)Color.Blue, (Float3)Color.Green);
            rast.Triangle(
                proc.Tr(new Float3(0f, 0.5f, 0f)),
                proc.Tr(new Float3(1.3f, 0.5f, 0f)),
                proc.Tr(new Float3(0.5f, -0.5f, 0f)),
                (Float3)Color.Red, (Float3)Color.Blue, (Float3)Color.Green);
        }
    }
}
