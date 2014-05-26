using System;
using System.Collections.Generic;

using System.Text;
using Disque.Math;

namespace Disque.Raytracer.Mappings
{
    public class Mapping
    {
        public virtual void GetTexelCoordinates(Vector3 hit_point, int xres, int yres, ref int row, ref int column)
        {
        }
    }
}
