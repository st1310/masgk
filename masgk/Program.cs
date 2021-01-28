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
            Buffer tex1 = new("tiles.png");
            Buffer tex2 = new("genetica.jpg");

            Rasterizer rast = new(buff, depth);
            timer1.Stop();

            VertexProcessor proc = new();

            Cube cube = new(1.0f);
            cube.Generate();

            Sphere sphere = new(1.0f, 5);
            sphere.Generate();

            Torus torus = new(1.0f, 0.4f, 8, 8);
            torus.Generate();

            Cylinder cylinder = new(1.0f, 1.0f, 1.0f, 8, 1);
            cylinder.Generate();

            Cone cone = new();
            cone.Generate();

            VertexProcessor procL = new();

            //Light light = new PointLight(new(0.0f, 0.0f, -4.5f), (Float3)Color.Black, (Float3)Color.LightCyan, (Float3)Color.Red, 10f);
            Light light = new DirectionalLight(new(0.0f, 0.0f, 1.0f), (Float3)Color.Black, (Float3)Color.LightCyan, (Float3)Color.Red, 10f);
            Light light1 = new DirectionalLight(new(0.0f, 0.0f, 1.0f), (Float3)Color.White, (Float3)Color.Black, (Float3)Color.Black, 10f);
            //Light light = new SpotLight(new(0.0f, 0.0f, 3.1f), new(0.0f, 0.0f, 1.0f), 5.0f, (Float3)Color.Black, (Float3)Color.LightCyan, (Float3)Color.Red, 10f);

            timer2.Start();
            proc.MultByScale(new(0.5f, 0.5f, 0.5f));
            proc.MultByRot(90f, new(0f, 1f, 1f));
            proc.MultByTrans(new(-0.5f, -1.0f, -1.0f));
            proc.Lt();

            //cube.Draw(ref rast, ref proc);
            cylinder.DrawUV(ref rast, ref proc, ref light, ref tex1);
            //sphere.Draw(ref rast, ref proc, ref light);
            timer2.Stop();

            VertexProcessor proc1 = new();
            timer2.Start();
            proc1.MultByScale(new(0.5f, 0.5f, 0.5f));
            proc1.MultByRot(45f, new(1f, 1f, 0f));
            proc1.MultByTrans(new(1.0f, -1.0f, -1.0f));
            proc1.Lt();

            //sphere.DrawNormals(ref rast, ref proc1);
            //sphere.Draw(ref rast, ref proc1, ref light);
            cylinder.DrawUV(ref rast, ref proc1, ref light1, ref tex1);
            timer2.Stop();

            VertexProcessor proc2 = new();
            timer2.Start();
            proc2.MultByScale(new(0.5f, 0.5f, 0.5f));
            proc2.MultByRot(90f, new(1f, 0f, 0f));
            proc2.MultByTrans(new(1.0f, 1.0f, -1.0f));
            proc2.Lt();

            //cylinder.DrawNormals(ref rast, ref proc2);
            //sphere.Draw(ref rast, ref proc2, ref light);
            cylinder.DrawUV(ref rast, ref proc2, ref light, ref tex2);
            timer2.Stop();

            VertexProcessor proc3 = new();
            timer2.Start();
            proc3.MultByScale(new(0.5f, 0.5f, 0.5f));
            proc3.MultByRot(0f, new(0f, 1f, 0f));
            proc3.MultByTrans(new(-0.5f, 1.0f, -1.0f));
            proc3.Lt();

            //cone.DrawNormals(ref rast, ref proc3);
            //sphere.Draw(ref rast, ref proc3, ref light);
            timer2.Stop();

            timer3.Start();
            buff.Save("buffer");
            depth.Save("depth");
            tex1.Save("tex1");
            tex2.Save("tex2");
            timer3.Stop();

            Console.WriteLine($"Buffer creation:   {timer1.Elapsed.TotalMilliseconds} ms");
            Console.WriteLine($"Rendering:         {timer2.Elapsed.TotalMilliseconds} ms");
            Console.WriteLine($"Buffer saving:     {timer3.Elapsed.TotalMilliseconds} ms");

            MathTest();
        }

        public static void MathTest()
        {
            Random rand = new Random();

            float x = (float)rand.NextDouble();
            float y = (float)rand.NextDouble();
            float z = (float)rand.NextDouble();
            float w = (float)rand.NextDouble();

            float v = (float)rand.NextDouble();

            Float3 fpuF3 = new(x, y, z);
            Float4 fpuF4 = new(x, y, z, w);
            Float3 fpuF31 = fpuF3 * v;
            Float4 fpuF41 = fpuF4 * v;
            Float4x4 fpuF44 = new(x, y, z, w,
                                  x, y, z, w,
                                  x, y, z, w,
                                  x, y, z, w);

            SseMath.Float3 sseF3 = new(x, y, z);
            SseMath.Float4 sseF4 = new(x, y, z, w);
            SseMath.Float3 sseF31 = sseF3 * v;
            SseMath.Float4 sseF41 = sseF4 * v;
            SseMath.Float4x4 sseF44 = new(x, y, z, w,
                                          x, y, z, w,
                                          x, y, z, w,
                                          x, y, z, w);

            Console.WriteLine($"Add VV FPU: {fpuF31 + fpuF3}");
            Console.WriteLine($"Add VV SSE: {sseF31 + sseF3}");
            Console.WriteLine();
            Console.WriteLine($"Add VS FPU: {fpuF3 + v}");
            Console.WriteLine($"Add VS SSE: {sseF3 + v}");
            Console.WriteLine();
            Console.WriteLine($"Sub VV FPU: {fpuF31 - fpuF3}");
            Console.WriteLine($"Sub VV SSE: {sseF31 - sseF3}");
            Console.WriteLine();
            Console.WriteLine($"Sub VS FPU: {fpuF3 - v}");
            Console.WriteLine($"Sub VS SSE: {sseF3 - v}");
            Console.WriteLine();
            Console.WriteLine($"Mul VV FPU: {fpuF31 * fpuF3}");
            Console.WriteLine($"Mul VV SSE: {sseF31 * sseF3}");
            Console.WriteLine();
            Console.WriteLine($"Mul VS FPU: {fpuF3 * v}");
            Console.WriteLine($"Mul VS SSE: {sseF3 * v}");
            Console.WriteLine();
            Console.WriteLine($"Div VV FPU: {fpuF31 / fpuF3}");
            Console.WriteLine($"Div VV SSE: {sseF31 / sseF3}");
            Console.WriteLine();
            Console.WriteLine($"Div VS FPU: {fpuF3 / v}");
            Console.WriteLine($"Div VS SSE: {sseF3 / v}");
            Console.WriteLine();
            Console.WriteLine($"Dot VV FPU: {fpuF31.Dot(fpuF3)}");
            Console.WriteLine($"Dot VV SSE: {SseMath.Float3.Dot(sseF31, sseF3)}");
            Console.WriteLine();
            Console.WriteLine($"Crs VV FPU: {fpuF31.Cross(fpuF3)}");
            Console.WriteLine($"Crs VV SSE: {SseMath.Float3.Cross(sseF31, sseF3)}");
            Console.WriteLine();
            Console.WriteLine($"Nor VV FPU: {fpuF3.Normalize}");
            Console.WriteLine($"Nor VV SSE: {SseMath.Float3.Normalize(sseF3)}");
            Console.WriteLine();
            Console.WriteLine($"Sat VV FPU: {Light.Saturate(fpuF3)}");
            Console.WriteLine($"Sat VV SSE: {SseMath.Float3.Saturate(sseF3)}");
            Console.WriteLine();
            Console.WriteLine($"Ref VV FPU: {Float3.Reflect(fpuF31, fpuF3)}");
            Console.WriteLine($"Ref VV SSE: {SseMath.Float3.Reflect(sseF31, sseF3)}");
            Console.WriteLine();

            Console.WriteLine($"Add VV FPU: {fpuF41 + fpuF4}");
            Console.WriteLine($"Add VV SSE: {sseF41 + sseF4}");
            Console.WriteLine();
            Console.WriteLine($"Add VS FPU: {fpuF4 + v}");
            Console.WriteLine($"Add VS SSE: {sseF4 + v}");
            Console.WriteLine();
            Console.WriteLine($"Sub VV FPU: {fpuF41 - fpuF4}");
            Console.WriteLine($"Sub VV SSE: {sseF41 - sseF4}");
            Console.WriteLine();
            Console.WriteLine($"Sub VS FPU: {fpuF4 - v}");
            Console.WriteLine($"Sub VS SSE: {sseF4 - v}");
            Console.WriteLine();
            Console.WriteLine($"Mul VV FPU: {fpuF41 * fpuF4}");
            Console.WriteLine($"Mul VV SSE: {sseF41 * sseF4}");
            Console.WriteLine();
            Console.WriteLine($"Mul VS FPU: {fpuF4 * v}");
            Console.WriteLine($"Mul VS SSE: {sseF4 * v}");
            Console.WriteLine();
            Console.WriteLine($"Div VV FPU: {fpuF41 / fpuF4}");
            Console.WriteLine($"Div VV SSE: {sseF41 / sseF4}");
            Console.WriteLine();
            Console.WriteLine($"Div VS FPU: {fpuF4 / v}");
            Console.WriteLine($"Div VS SSE: {sseF4 / v}");
            Console.WriteLine();
            Console.WriteLine($"Dot VV FPU: {fpuF41.Dot(fpuF4)}");
            Console.WriteLine($"Dot VV SSE: {SseMath.Float4.Dot(sseF41, sseF4)}");
            Console.WriteLine();
            Console.WriteLine($"Nor VV FPU: {fpuF4.Normalize}");
            Console.WriteLine($"Nor VV SSE: {SseMath.Float4.Normalize(sseF4)}");
            Console.WriteLine();
            Console.WriteLine($"Mul VM FPU: {fpuF44 * fpuF4}");
            Console.WriteLine($"Mul VM SSE: {sseF44 * sseF4}");
            Console.WriteLine();

            Console.WriteLine($"Mul MM FPU: \n{fpuF44 * fpuF44}");
            Console.WriteLine($"Mul MM SSE: \n{sseF44 * sseF44}");
            Console.WriteLine();
        }
    }
}
