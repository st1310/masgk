using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace masgk
{
    public class Cube : Mesh, IPrimitive
    {
        private readonly float _size;

        public Cube(float size)
        {
            _size = size;
        }

        public void Generate()
        {
            // A cube has six faces, each one pointing in a different direction.
            Float3[] normals =
                {
                    new Float3(0, 0, 1),
                    new Float3(0, 0, -1),
                    new Float3(1, 0, 0),
                    new Float3(-1, 0, 0),
                    new Float3(0, 1, 0),
                    new Float3(0, -1, 0),
                };

            // Create each face in turn.
            foreach (Float3 normal in normals)
            {
                // Get two vectors perpendicular to the face normal and to each other.
                Float3 side1 = new Float3(normal.Y, normal.Z, normal.X);
                Float3 side2 = normal.Cross(side1);

                // Six indices (two triangles) per face.
                Indices.Add(Positions.Count + 0);
                Indices.Add(Positions.Count + 1);
                Indices.Add(Positions.Count + 2);

                Indices.Add(Positions.Count + 0);
                Indices.Add(Positions.Count + 2);
                Indices.Add(Positions.Count + 3);

                // Four vertices per face.
                Positions.Add((normal - side1 - side2) * _size / 2);
                Positions.Add((normal - side1 + side2) * _size / 2);
                Positions.Add((normal + side1 + side2) * _size / 2);
                Positions.Add((normal + side1 - side2) * _size / 2);

                Normals.Add(normal);
                Normals.Add(normal);
                Normals.Add(normal);
                Normals.Add(normal);
            }
        }
    }
}
