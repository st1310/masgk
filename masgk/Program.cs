using System;
using System.Diagnostics;
using System.Drawing;

namespace masgk
{
    class Program
    {
        static void Main(string[] args)
        {
#if DEBUG
            var timer = new Stopwatch();
            timer.Start();
#endif
            using Buffer buff = new Buffer(300, 300, Color.Aqua);
            using DepthBuffer depth = new DepthBuffer(300, 300, -1.0f);

            Rasterizer rast = new Rasterizer(buff, depth);

            VertexProcessor proc = new VertexProcessor();
            proc.MultByTrans(new Float3(-1f, 0f, 0f));
            proc.MultByRot(45f, new Float3(0f, 0f, 1f));
            proc.MultByScale(new Float3(0.5f, 0.5f, 0.5f));
            proc.Lt();

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

            buff.Save("buffer");
            depth.Save("depth");

#if DEBUG
            timer.Stop();
            Console.WriteLine($"Time: {timer.Elapsed.TotalMilliseconds} ms");
#endif
        }
    }
}
