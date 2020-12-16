using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace masgk
{
    public class DirectionalLight : Light
    {
        public DirectionalLight(Float3 position, Float3 ambient, Float3 diffuse, Float3 specular, float shininess)
            : base(position, ambient, diffuse, specular, shininess)
        {
        }

        public override Float3 Calculate(Float3 fragPosition, Float3 fragNormal, ref VertexProcessor proc)
        {
            Float3 L = Position;
            Float3 N = proc.Tr_obj2view3(fragNormal).Normalize;
            Float3 V = proc.Tr_obj2view4(-fragPosition).Normalize;
            Float3 R = Float3.Reflect(L, N).Normalize;

            float diff = Saturate(L.Dot(N));
            float spec = (float)Math.Pow(Saturate(R.Dot(V)), Shininess);

            return Saturate(Ambient + Diffuse * diff + Specular * spec);
        }
    }
}
