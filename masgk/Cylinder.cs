using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace masgk
{
    public class Cylinder : Mesh, IPrimitive
    {
        private readonly float radiusTop;
        private readonly float radiusBottom;
        private readonly float height;
        private readonly int radialSegments;
        private readonly int heightSegments;
        private readonly bool openEnded;
        private readonly double thetaStart;
        private readonly double thetaLength;

        public Cylinder(
            float radiusTop = 1.0f,
            float radiusBottom = 1.0f,
            float height = 1.0f,
            int radialSegments = 8,
            int heightSegments = 1,
            bool openEnded = false,
            double thetaStart = 0.0d,
            double thetaLength = Math.PI * 2)
        {
            this.radiusTop = radiusTop;
            this.radiusBottom = radiusBottom;
            this.height = height;
            this.radialSegments = radialSegments;
            this.heightSegments = heightSegments;
            this.openEnded = openEnded;
            this.thetaStart = thetaStart;
            this.thetaLength = thetaLength;
        }

        public void Generate()
        {
            int index = 0;
            List<List<int>> indexArray = new();
            float halfHeight = height / 2;

            GenerateTorso(ref index, ref indexArray, ref halfHeight);

            if (!openEnded)
            {
                if (radiusTop > 0)
                    GenerateCap(true, ref index, ref indexArray, ref halfHeight);
                if (radiusBottom > 0)
                    GenerateCap(false, ref index, ref indexArray, ref halfHeight);
            }
        }

        private void GenerateTorso(ref int index, ref List<List<int>> indexArray, ref float halfHeight)
        {
            Float3 normal = new();
            Float3 vertex = new();

            // this will be used to calculate the normal
            float slope = (radiusBottom - radiusTop) / height;

            // generate vertices, normals and uvs
            for (int y = 0; y <= heightSegments; y++)
            {
                List<int> indexRow = new();

                float v = (float)y / heightSegments;

                // calculate the radius of the current row
                float radius = v * (radiusBottom - radiusTop) + radiusTop;

                for (int x = 0; x <= radialSegments; x++)
                {
                    float u = (float)x / radialSegments;

                    double theta = u * thetaLength + thetaStart;

                    double sinTheta = Math.Sin(theta);
                    double cosTheta = Math.Cos(theta);

                    // vertex
                    vertex.X = (float)(radius * sinTheta);
                    vertex.Y = -v * height + halfHeight;
                    vertex.Z = (float)(radius * cosTheta);
                    Positions.Add(vertex);

                    // normal
                    normal = new((float)sinTheta, slope, (float)cosTheta);
                    Normals.Add(normal.Normalize);

                    // uv
                    TextureCoordinates.Add(new(u, 1.0f - v, 0.0f));

                    // save index of vertex in respective row
                    indexRow.Add(index++);
                }

                // now save vertices of the row in our index array
                indexArray.Add(indexRow);
            }

            // generate indices
            for (int x = 0; x < radialSegments; x++)
            {
                for (int y = 0; y < heightSegments; y++)
                {
                    // we use the index array to access the correct indices
                    int a = indexArray[y][x];
                    int b = indexArray[y + 1][x];
                    int c = indexArray[y + 1][x + 1];
                    int d = indexArray[y][x + 1];

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

        private void GenerateCap(bool top, ref int index, ref List<List<int>> indexArray, ref float halfHeight)
        {
            // save the index of the first center vertex
            int centerIndexStart = index;

            Float3 uv = new();
            Float3 vertex = new();

            float radius = top ? radiusTop : radiusBottom;
            float sign = top ? 1.0f : -1.0f;

            // first we generate the center vertex data of the cap.
            // because the geometry needs one set of uvs per face,
            // we must generate a center vertex per face/segment
            for (int x = 1; x <= radialSegments; x++)
            {
                // vertex
                Positions.Add(new(0.0f, halfHeight * sign, 0.0f));

                // normal
                Normals.Add(new(0.0f, sign, 0.0f));

                // uv
                TextureCoordinates.Add(new(0.5f, 0.5f, 0.0f));

                // increase index
                index++;
            }

            // save the index of the last center vertex
            int centerIndexEnd = index;

            // now we generate the surrounding vertices, normals and uvs
            for (int x = 0; x <= radialSegments; x++)
            {
                float u = (float)x / radialSegments;
                double theta = u * thetaLength + thetaStart;

                double cosTheta = Math.Cos(theta);
                double sinTheta = Math.Sin(theta);

                // vertex
                vertex.X = (float)(radius * sinTheta);
                vertex.Y = halfHeight * sign;
                vertex.Z = (float)(radius * cosTheta);
                Positions.Add(vertex);

                // normal
                Normals.Add(new(0.0f, sign, 0.0f));

                // uv
                uv.X = (float)((cosTheta * 0.5) + 0.5);
                uv.Y = (float)((sinTheta * 0.5 * sign) + 0.5);
                TextureCoordinates.Add(uv);

                // increase index
                index++;
            }

            // generate indices
            for (int x = 0; x < radialSegments; x++)
            {
                int c = centerIndexStart + x;
                int i = centerIndexEnd + x;

                if (top)
                {
                    // face top
                    Indices.Add(c);
                    Indices.Add(i + 1);
                    Indices.Add(i);
                }
                else
                {
                    // face bottom
                    Indices.Add(c);
                    Indices.Add(i);
                    Indices.Add(i + 1);
                }
            }
        }
    }
}
