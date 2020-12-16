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
            using Buffer buff = new(600, 600, Color.Aqua);
            using DepthBuffer depth = new(600, 600, 1.0f);

            Rasterizer rast = new(buff, depth);
            timer1.Stop();

            VertexProcessor proc = new();

            Cube cube = new(1.0f);
            cube.Generate();

            Sphere sphere = new(1.0f, 10);
            sphere.Generate();

            Torus torus = new(1.0f, 0.4f, 4, 16);
            torus.Generate();

            Cylinder cylinder = new(1.0f, 1.0f, 1.0f, 8, 1);
            cylinder.Generate();

            Cone cone = new();
            cone.Generate();

            timer2.Start();
            proc.MultByScale(new(0.5f, 0.5f, 0.5f));
            proc.MultByRot(45f, new(0f, 1f, 0f));
            proc.MultByTrans(new(-0.5f, -1.0f, -1.0f));
            proc.Lt();

            //cube.Draw(ref rast, ref proc);
            torus.Draw(ref rast, ref proc);
            timer2.Stop();

            VertexProcessor proc1 = new();
            timer2.Start();
            proc1.MultByScale(new(0.5f, 0.5f, 0.5f));
            proc1.MultByRot(0f, new(0f, 1f, 0f));
            proc1.MultByTrans(new(1.0f, -1.0f, -1.0f));
            proc1.Lt();

            sphere.Draw(ref rast, ref proc1);
            timer2.Stop();

            VertexProcessor proc2 = new();
            timer2.Start();
            proc2.MultByScale(new(0.5f, 0.5f, 0.5f));
            proc2.MultByRot(0f, new(0f, 1f, 0f));
            proc2.MultByTrans(new(1.0f, 1.0f, -1.0f));
            proc2.Lt();

            cylinder.Draw(ref rast, ref proc2);
            timer2.Stop();

            VertexProcessor proc3 = new();
            timer2.Start();
            proc3.MultByScale(new(0.5f, 0.5f, 0.5f));
            proc3.MultByRot(0f, new(0f, 1f, 0f));
            proc3.MultByTrans(new(-0.5f, 1.0f, -1.0f));
            proc3.Lt();

            cone.Draw(ref rast, ref proc3);
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
