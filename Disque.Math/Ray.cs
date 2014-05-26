using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Disque.Math
{
    public struct Ray
    {
        public Vector3 Position;
        public Vector3 Direction;
        public int Depth;
        public Ray(Vector3 p, Vector3 d)
        {
            Position = p;
            Direction = d;
            Depth = 0;
        }
    }
}
