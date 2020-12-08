using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;

namespace masgk
{
    class Program
    {
        static void Main(string[] args)
        {
            var timer1 = new Stopwatch();
            var timer2 = new Stopwatch();
            var timer3 = new Stopwatch();

            timer1.Start();
            using Buffer buff = new Buffer(600, 600, Color.Aqua);
            using DepthBuffer depth = new DepthBuffer(600, 600, 1.0f);

            Rasterizer rast = new Rasterizer(buff, depth);
            timer1.Stop();

            VertexProcessor proc = new VertexProcessor();

            Cube cube = new(1.0f);
            cube.Generate();

            timer2.Start();
            proc.MultByScale(new Float3(0.5f, 0.5f, 0.5f));
            proc.MultByRot(45f, new Float3(1f, 0f, 0f));
            proc.MultByTrans(new Float3(-0.5f, 0f, -1.0f));
            proc.Lt();

            cube.Draw(ref rast, ref proc);
            //PrimHelper.Cube(rast, proc);

            timer2.Stop();
            VertexProcessor proc1 = new VertexProcessor();
            timer2.Start();
            proc1.MultByScale(new Float3(0.5f, 0.5f, 0.5f));
            proc1.MultByRot(0f, new Float3(0f, 1f, 0f));
            proc1.MultByTrans(new Float3(1.0f, 0f, -1.0f));
            proc1.Lt();

            //PrimHelper.Cube(rast, proc1);
            timer2.Stop();

            timer3.Start();
            buff.Save("buffer");
            depth.Save("depth");
            timer3.Stop();

            Console.WriteLine($"Buffer creation:   {timer1.Elapsed.TotalMilliseconds} ms");
            Console.WriteLine($"Rendering:         {timer2.Elapsed.TotalMilliseconds} ms");
            Console.WriteLine($"Buffer saving:     {timer3.Elapsed.TotalMilliseconds} ms");
        }
    }
}
