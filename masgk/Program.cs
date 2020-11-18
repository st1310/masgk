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
            using DepthBuffer depth = new DepthBuffer(300, 300, 1.0f);

            Rasterizer rast = new Rasterizer(buff, depth);

            VertexProcessor proc = new VertexProcessor();
            proc.MultByScale(new Float3(0.5f, 0.5f, 0.5f));
            proc.MultByRot(45f, new Float3(1f, 0f, 0f));
            proc.MultByTrans(new Float3(-0.5f, 0f, -5.0f));
            proc.Lt();

            PrimHelper.Cube(rast, proc);

            VertexProcessor proc1 = new VertexProcessor();
            proc1.MultByScale(new Float3(0.5f, 0.5f, 0.5f));
            proc1.MultByRot(0f, new Float3(0f, 1f, 0f));
            proc1.MultByTrans(new Float3(1.0f, 0f, -5.0f));
            proc1.Lt();

            PrimHelper.Cube(rast, proc1);

            buff.Save("buffer");
            depth.Save("depth");

#if DEBUG
            timer.Stop();
            Console.WriteLine($"Time: {timer.Elapsed.TotalMilliseconds} ms");
#endif
        }
    }
}
