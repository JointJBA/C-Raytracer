using System;
using System.Collections.Generic;

using System.Text;
using Disque.Math;

namespace Disque.Raytracer.Mappings
{
    public class RectangularMap : Mapping
    {
        public override void GetTexelCoordinates(Vector3 local_hit_point, int xres, int yres, ref int row, ref int column)
        {
            column = (int)(local_hit_point.Z);
            row = (int)(local_hit_point.X);
        }
    }
}
