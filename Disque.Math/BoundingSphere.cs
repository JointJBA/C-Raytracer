using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Disque.Math
{
    public struct BoundingSphere
    {
        Vector3 _center;
        float _radius;
        public Vector3 Center { get { return _center; } set { _center = value; } }
        public float Radius { get { return _radius; } set { _radius = value; } }
        public BoundingSphere(Vector3 center, float radius)
        {
            _center = center;
            _radius = radius;
        }
        public BoundingSphere(BoundingSphere one, BoundingSphere two)
        {
            Vector3 centreOffset = two.Center - one.Center;
            float distance = centreOffset.SquaredMagnitude;
            float radiusDiff = two.Radius - one.Radius;
            if (radiusDiff * radiusDiff >= distance)
            {
                if (one.Radius > two.Radius)
                {
                    _center = one.Center;
                    _radius = one.Radius;
                }
                else
                {
                    _center = two.Center;
                    _radius = two.Radius;
                }
            }
            else
            {
                distance = MathHelper.Sqrt(distance);
                _radius = (distance + one.Radius + two.Radius) * ((float)0.5);
                _center = one.Center;
                if (distance > 0)
                {
                    _center += centreOffset * ((_radius - one.Radius) / distance);
                }
            }
        }
        public bool Overlaps(BoundingSphere other)
        {
            float distanceSquared = (_center - other.Center).SquaredMagnitude;
            return distanceSquared < (_radius + other._radius) * (_radius + other.Radius);
        }
        public float GetGrowth(BoundingSphere other)
        {
            BoundingSphere newSphere = new BoundingSphere(this, other);
            return newSphere.Radius * newSphere.Radius - _radius * _radius;
        }
        public float GetSize()
        {
            return (1.333333f) * MathHelper.PI * Radius * Radius * Radius;
        }
    }
}
