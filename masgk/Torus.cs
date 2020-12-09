using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace masgk
{
    public class Torus : Mesh, IPrimitive
    {
        private readonly float radius;
        private readonly float tube;
        private readonly int radialSegments;
        private readonly int tubularSegments;
        private readonly double arc;

        public Torus(float radius = 1.0f, float tube = 0.4f, int radialSegments = 8, int tubularSegments = 8, double arc = Math.PI * 2.0d)
        {
            this.radius = radius;
            this.tube = tube;
            this.radialSegments = radialSegments;
            this.tubularSegments = tubularSegments;
            this.arc = arc;
        }

        public void Generate()
        {
            Float3 center = new();
            Float3 vertex = new();
            Float3 normal = new();

            // generate vertices, normals and uvs
            for (int j = 0; j <= radialSegments; j++)
            {
                for (int i = 0; i <= tubularSegments; i++)
                {
                    double u = (double)i / tubularSegments * arc;
                    double v = (double)j / radialSegments * Math.PI * 2;

                    // vertex
                    vertex.X = (float)((radius + tube * Math.Cos(v)) * Math.Cos(u));
                    vertex.Y = (float)((radius + tube * Math.Cos(v)) * Math.Sin(u));
                    vertex.Z = (float)(tube * Math.Sin(v));

                    Positions.Add(vertex);

                    // normal
                    center.X = (float)(radius * Math.Cos(u));
                    center.Y = (float)(radius * Math.Sin(u));
                    normal = vertex - center;

                    Normals.Add(normal.Normalize);

                    // uv
                    TextureCoordinates.Add(new((float)i / tubularSegments, (float)j / radialSegments, 0.0f));
                }
            }

            for (int j = 1; j <= radialSegments; j++)
            {
                for (int i = 1; i <= tubularSegments; i++)
                {
                    // indices
                    int a = (tubularSegments + 1) * j + i - 1;
                    int b = (tubularSegments + 1) * (j - 1) + i - 1;
                    int c = (tubularSegments + 1) * (j - 1) + i;
                    int d = (tubularSegments + 1) * j + i;

                    // faces
                    Indices.Add(d);
                    Indices.Add(b);
                    Indices.Add(a);

                    Indices.Add(d);
                    Indices.Add(c);
                    Indices.Add(b);
                }
            }
        }
    }
}
