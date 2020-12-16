using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace masgk
{
    public abstract class Mesh
    {
        public List<Float3> Positions { get; private set; } //! Add Positions[]
        public List<Float3> Normals { get; private set; }
        public List<int> Indices { get; private set; }
        public List<Float3> TextureCoordinates { get; private set; } //! Float2

        protected Mesh()
        {
            Positions = new List<Float3>();
            Normals = new List<Float3>();
            Indices = new List<int>();
            TextureCoordinates = new List<Float3>();
        }

        public void Draw(ref Rasterizer rast, ref VertexProcessor proc)
        {
            for (int i = 0; i < Indices.Count; i += 3) //! make Indices.Count a variable
            {
                rast.Triangle(
                proc.Tr(Positions[Indices[i]]),
                proc.Tr(Positions[Indices[i + 1]]),
                proc.Tr(Positions[Indices[i + 2]]),
                (Float3)Color.Coral, (Float3)Color.Bisque, (Float3)Color.DarkCyan);
            }
        }

        public void Draw(ref Rasterizer rast, ref VertexProcessor proc, ref Light light)
        {
            for (int i = 0; i < Indices.Count; i += 3) //! make Indices.Count a variable
            {
                rast.Triangle(
                proc.Tr(Positions[Indices[i]]),
                proc.Tr(Positions[Indices[i + 1]]),
                proc.Tr(Positions[Indices[i + 2]]),
                light.Calculate(Positions[Indices[i]], Normals[Indices[i]], ref proc),
                light.Calculate(Positions[Indices[i + 1]], Normals[Indices[i + 1]], ref proc),
                light.Calculate(Positions[Indices[i + 2]], Normals[Indices[i + 2]], ref proc));
            }
        }
    }
}
