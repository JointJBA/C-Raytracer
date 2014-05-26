using System;
using System.Collections.Generic;
using System.Text;
using Disque.Math;

namespace Disque.Raytracer.GeometricObjects.Primitives
{
    public class Annulus : GeometricObject
    {
        Vector3 center, normal;
        float wall_thickness, w_squared;
        float inner_radius, i_squared;
        BBox bbox;

        Vector3 compute_normal(Vector3 p)
        {
            return normal;
        }

        public Annulus(Vector3 cen, Vector3 norm, float i, float w, string name)
            : base(name)
        {
            center = cen;
            normal = norm;
            inner_radius = i;
            wall_thickness = w;
            bbox = new BBox(center.X - i - w, center.X + i + w, center.Y - MathHelper.Epsilon, center.Y + MathHelper.Epsilon, center.Z - i - w, center.Z + i + w);
            i_squared = i * i;
            w_squared = (i + w) * (i + w);
        }

        public override bool Hit(Ray ray, ref float tmin, ref ShadeRec sr)
        {
            if (!bbox.Hit(ray))
                return (false);
            float t = Vector3.Dot((center - ray.Position), normal) / Vector3.Dot(ray.Direction, normal);
            if (t <= MathHelper.Epsilon)
                return (false);
            Vector3 p = ray.Position + t * ray.Direction;
            float v = Vector3.DistanceSquared(center, p);
            if (v < w_squared && v > i_squared)
            {
                tmin = t;
                sr.Normal = normal;
                sr.LocalHitPoint = p;
                return (true);
            }
            else
                return (false);
        }

        public override bool ShadowHit(Ray ray, ref float tmin)
        {
            if (Shadows)
            {
                if (!bbox.Hit(ray))
                    return (false);
                float t = Vector3.Dot((center - ray.Position), normal) / Vector3.Dot(ray.Direction, normal);
                if (t <= MathHelper.Epsilon)
                    return (false);
                Vector3 p = ray.Position + t * ray.Direction;
                float v = Vector3.DistanceSquared(center, p);
                if (v < w_squared && v > i_squared)
                {
                    tmin = t;
                    return (true);
                }
                else
                    return (false);
            }
            return false;
        }
    }
}