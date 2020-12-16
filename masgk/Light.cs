using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace masgk
{
    abstract public class Light
    {
        public Float3 Position { get; set; }
        public Float3 Ambient { get; set; }
        public Float3 Diffuse { get; set; }
        public Float3 Specular { get; set; }
        public float Shininess { get; set; }

        public Light(Float3 position, Float3 ambient, Float3 diffuse, Float3 specular, float shininess)
        {
            Position = position;
            Ambient = ambient;
            Diffuse = diffuse;
            Specular = specular;
            Shininess = shininess;
        }

        public abstract Float3 Calculate(Float3 fragPosition, Float3 fragNormal, ref VertexProcessor proc); //! in

        public static float Saturate(float value) => Math.Clamp(value, 0.0f, 1.0f); //! move to math helper class
        public static Float3 Saturate(Float3 value) => //! move to math helper class
            new(Math.Clamp(value.X, 0.0f, 1.0f),
                Math.Clamp(value.Y, 0.0f, 1.0f),
                Math.Clamp(value.Z, 0.0f, 1.0f));
    }
}
