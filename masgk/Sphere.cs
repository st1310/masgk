using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace masgk
{
    public class Sphere : Mesh, IPrimitive
    {
        private readonly float _radius;
        private readonly int _tessellationLevel;

        public Sphere(float radius, int tessellationLevel)
        {
            if (tessellationLevel < 3)
                throw new ArgumentOutOfRangeException(nameof(tessellationLevel));

            _radius = radius;
            _tessellationLevel = tessellationLevel;
        }

        public void Generate()
        {
            int verticalSegments = _tessellationLevel;
            int horizontalSegments = _tessellationLevel * 2;

            // Start with a single vertex at the bottom of the sphere.
            Positions.Add(new Float3(0.0f, -1.0f, 0.0f) * _radius); //! Float3.Down
			Normals.Add(new Float3(0.0f, -1.0f, 0.0f)); //! Float3.Down

			// Create rings of vertices at progressively higher latitudes.
			for (int i = 0; i < verticalSegments - 1; i++)
			{
				double latitude = ((i + 1) * Math.PI / verticalSegments) - Math.PI / 2.0d;

				double dy = Math.Sin(latitude);
				double dxz = Math.Cos(latitude);

				// Create a single ring of vertices at this latitude.
				for (int j = 0; j < horizontalSegments; j++)
				{
					double longitude = j * Math.PI * 2.0f / horizontalSegments;

					double dx = Math.Cos(longitude) * dxz;
					double dz = Math.Sin(longitude) * dxz;

                    Float3 normal = new((float)dx, (float)dy, (float)dz);

					Positions.Add(normal * _radius);
					Normals.Add(normal);
				}
			}

			// Finish with a single vertex at the top of the sphere.
			Positions.Add(new Float3(0.0f, 1.0f, 0.0f) * _radius); //! Float3.Up
			Normals.Add(new Float3(0.0f, 1.0f, 0.0f)); //! Float3.Up

			// Create a fan connecting the bottom vertex to the bottom latitude ring.
			for (int i = 0; i < horizontalSegments; i++)
			{
				Indices.Add(0);
				Indices.Add(1 + (i + 1) % horizontalSegments);
				Indices.Add(1 + i);
			}

			// Fill the sphere body with triangles joining each pair of latitude rings.
			for (int i = 0; i < verticalSegments - 2; i++)
			{
				for (int j = 0; j < horizontalSegments; j++)
				{
					int nextI = i + 1;
					int nextJ = (j + 1) % horizontalSegments;

					Indices.Add(1 + i * horizontalSegments + j);
					Indices.Add(1 + i * horizontalSegments + nextJ);
					Indices.Add(1 + nextI * horizontalSegments + j);

					Indices.Add(1 + i * horizontalSegments + nextJ);
					Indices.Add(1 + nextI * horizontalSegments + nextJ);
					Indices.Add(1 + nextI * horizontalSegments + j);
				}
			}

			// Create a fan connecting the top vertex to the top latitude ring.
			for (int i = 0; i < horizontalSegments; i++)
			{
				Indices.Add(Positions.Count - 1);
				Indices.Add(Positions.Count - 2 - (i + 1) % horizontalSegments);
				Indices.Add(Positions.Count - 2 - i);
			}
		}
    }
}
