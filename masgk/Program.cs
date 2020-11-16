using System;
using System.Drawing;

namespace masgk
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Creating a color buffer...\n");
            using Buffer buff = new Buffer(300, 300, Color.Aqua);
            using DepthBuffer depth = new DepthBuffer(300, 300, 1.0f);

            Rasterizer rast = new Rasterizer(buff, depth);

            VertexProcessor proc = new VertexProcessor();

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

            Console.WriteLine("Saving a color buffer...\n");
            buff.Save("newbuffer");
            Console.WriteLine("Saving a depth buffer...\n");
            depth.Save("newdepth");
        }
    }
}
