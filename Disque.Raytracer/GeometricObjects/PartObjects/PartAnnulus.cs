using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Disque.Math;

namespace Disque.Raytracer.GeometricObjects.PartObjects
{
    public class PartAnnulus : GeometricObject
    {
        Vector3 center, normal;
        float inner_radius, wall_thickness, i_squared, w_squared, min_azimuth, max_azimuth;
        BBox bbox;

        public PartAnnulus(Vector3 pos, Vector3 nor, float i_radius, float w_thickness, float m_azimuth, float mx_azimuth, string name)
            : base(name)
        {
            center = pos;
            normal = nor;
            inner_radius = i_radius;
            wall_thickness = w_thickness;
            min_azimuth = MathHelper.ToDegrees(m_azimuth);
            max_azimuth = MathHelper.ToDegrees(mx_azimuth);
            bbox = new BBox(center.X - inner_radius - wall_thickness, center.X + inner_radius + wall_thickness, center.Y - MathHelper.Epsilon, center.Y + MathHelper.Epsilon, center.Z - inner_radius - wall_thickness, center.Z + inner_radius + wall_thickness);
            i_squared = inner_radius * inner_radius;
            w_squared = (inner_radius + wall_thickness) * (inner_radius + wall_thickness);
        }

        public override Vector3 GetNormal(Vector3 p)
        {
            return normal;
        }

        public override bool Hit(Ray ray, ref float tmin, ref ShadeRec sr)
        {
            if (!bbox.Hit(ray))
                return (false);
            float t = Vector3.Dot((center - ray.Position), normal) / Vector3.Dot(ray.Direction, normal);
            if (t <= MathHelper.Epsilon)
                return (false);

            Vector3 p = ray.Position + t * ray.Direction;
            float v = center.DistanceSquared(p);
            if (v < w_squared && v > i_squared)
            {

                double phi =MathHelper.Atan2(p.X, p.Z);
                if (phi < 0.0)
                    phi += MathHelper.TwoPI;
                if (phi <= max_azimuth && phi >= min_azimuth)
                {
                    tmin = t;
                    sr.Normal = normal;
                    sr.LocalHitPoint = p;
                    return (true);
                }
                else
                    return false;
            }
            else
                return (false);
        }

        public override bool ShadowHit(Ray ray, ref float tmin)
        {
            if (!Shadows)
                return false;
            if (!bbox.Hit(ray))
                return (false);
            float t = Vector3.Dot((center - ray.Position), normal) / Vector3.Dot(ray.Direction, normal);
            if (t <= MathHelper.Epsilon)
                return (false);

            Vector3 p = ray.Position + t * ray.Direction;
            float v = center.DistanceSquared(p);
            if (v < w_squared && v > i_squared)
            {

                double phi = MathHelper.Atan2(p.X, p.Z);
                if (phi < 0.0)
                    phi += MathHelper.TwoPI;
                if (phi <= max_azimuth && phi >= min_azimuth)
                {
                    tmin = t;
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }

        public override BBox GetBoundingBox()
        {
            return bbox;
        }
    }
}
