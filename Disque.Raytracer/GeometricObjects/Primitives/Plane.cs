using System;
using System.Collections.Generic;
using System.Text;
using Disque.Math;

namespace Disque.Raytracer.GeometricObjects.Primitives
{
    public class Plane : GeometricObject
    {
        public Vector3 Center, Normal;

        public Plane(Vector3 c, Vector3 n, string name)
            : base(name)
        {
            Center = c;
            Normal = n;
        }

        public override bool Hit(Ray ray, ref float tmin, ref ShadeRec sr)
        {
            float t = Vector3.Dot((Center - ray.Position), Normal) / Vector3.Dot(ray.Direction, Normal);

            if (t > MathHelper.Epsilon)
            {
                tmin = t;
                sr.Normal = Normal;
                sr.LocalHitPoint = ray.Position + t * ray.Direction;
                return (true);
            }
            return (false);
        }

        public override bool ShadowHit(Ray ray, ref float tmin)
        {
            if (!Shadows)
                return false;
            float t = Vector3.Dot((Center - ray.Position), Normal) / Vector3.Dot(ray.Direction, Normal);
            if (t > MathHelper.Epsilon)
            {
                tmin = t;
                return (true);
            }
            else
                return (false);
        }
    }
}
