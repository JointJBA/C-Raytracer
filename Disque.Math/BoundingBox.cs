using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Disque.Math
{
    public struct BoundingBox
    {
        Vector3 _center;
        Vector3 _hsize;
        public Vector3 Center { get { return _center; } set { _center = value; } }
        public Vector3 HalfSize { get { return _hsize; } set { _hsize = value; } }
        public BoundingBox(Vector3 center, Vector3 halfSize)
        {
            _center = center;
            _hsize = halfSize;
        }
    }
}
