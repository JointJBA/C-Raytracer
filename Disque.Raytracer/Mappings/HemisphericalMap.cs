using System;
using System.Collections.Generic;

using System.Text;
using Disque.Math;

namespace Disque.Raytracer.Mappings
{
    public class HemisphericalMap : Mapping
    {
        public override void GetTexelCoordinates(Vector3 local_hit_point, int xres, int yres, ref int row, ref int column)
        {
            float theta = MathHelper.Acos(local_hit_point.Y);
            float phi = MathHelper.Atan2(local_hit_point.X, local_hit_point.Z);
            if (phi < 0.0)
                phi += MathHelper.TwoPI;
            float u = phi * MathHelper.InvTwoPI;
            float v = 1.0f - theta * MathHelper.InvPI;
            column = (int)(((float)(xres - 1)) * u);
            row = (int)(((float)(yres - 1)) * v);
        }
    }
}
