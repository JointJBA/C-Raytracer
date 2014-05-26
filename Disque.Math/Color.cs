using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Disque.Math
{
    public class Color
    {
        public float R;
        public float G;
        public float B;
        public Color(Vector3 v)
        {
            R = v.X;
            G = v.Y;
            B = v.Z;
        }
    }
}
