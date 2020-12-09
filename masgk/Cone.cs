using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace masgk
{
    public class Cone : Cylinder
    {
        public Cone(
            float radiusBottom = 1.0f,
            float height = 1.0f,
            int radialSegments = 8,
            int heightSegments = 1,
            bool openEnded = false,
            double thetaStart = 0.0d,
            double thetaLength = Math.PI * 2.0d)
            : base(0, radiusBottom, height, radialSegments, heightSegments, openEnded, thetaStart, thetaLength)
        {
        }
    }
}
