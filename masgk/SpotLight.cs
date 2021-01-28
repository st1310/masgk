using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace masgk
{
    public class SpotLight : Light
    {
        private float angle;
        private float cutOff;

        public Float3 Direction { get; set; }
        public float Angle
        {
            get => angle;
            set
            {
                angle = value;
                cutOff = 1f - (value / 180f);
            }
        }

        public SpotLight(Float3 position, Float3 direction, float angle, Float3 ambient, Float3 diffuse, Float3 specular, float shininess)
            : base(position, ambient, diffuse, specular, shininess)
        {
            Direction = direction;
            Angle = angle;
        }

        public override Float3 Calculate(in Float3 fragPosition, in Float3 fragNormal, ref VertexProcessor proc)
        {
            Float3 N = proc.Tr_obj2view3(fragNormal).Normalize;
            Float3 V = proc.Tr_obj2view4(-fragPosition);
            Float3 L = (Position - V).Normalize;
            V = V.Normalize;

            Float3 D = Position - fragPosition;
            float theta = L.Dot(D.Normalize);

            if (theta > cutOff)
            {
                Float3 R = Float3.Reflect(L, N).Normalize;

                float diff = Saturate(L.Dot(N));
                float spec = (float)Math.Pow(Saturate(R.Dot(V)), Shininess);

                return Saturate(Ambient + Diffuse * diff + Specular * spec);
            }
            else
                return Saturate(Ambient);

        }
    }
}
