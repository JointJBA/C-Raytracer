using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Disque.Math;

namespace Disque.Raytracer.GeometricObjects.PartObjects
{
    public class ConvexPartSphere : GeometricObject
    {
        Vector3 center;
        float radius, phi_min, phi_max, theta_min, theta_max, cos_theta_min, cos_theta_max;

        public ConvexPartSphere(Vector3 c, float r, float azimuth_min, float azimuth_max, float polar_min, float polar_max, string name)
            : base(name)
        {
            center = c;
            radius = r;
            phi_min = MathHelper.ToRadians(azimuth_min);
            phi_max = MathHelper.ToRadians(azimuth_max);
            theta_min = MathHelper.ToRadians(polar_min);
            theta_max = MathHelper.ToRadians(polar_max);
            cos_theta_min = MathHelper.Cos(theta_min);
            cos_theta_max = MathHelper.Cos(theta_max);
        }

        public override bool Hit(Ray ray, ref float tmin, ref ShadeRec sr)
        {
            float t;
            Vector3 temp = ray.Position - center;
            float a = Vector3.Dot(ray.Direction, ray.Direction);
            float b = 2.0f * Vector3.Dot(temp, ray.Direction);
            float c = Vector3.Dot(temp, temp) - radius * radius;
            float disc = b * b - 4.0f * a * c;

            if (disc < 0.0)
                return (false);
            else
            {
                float e = MathHelper.Sqrt(disc);
                float denom = 2.0f * a;
                t = (-b - e) / denom;    // smaller root

                if (t > MathHelper.Epsilon)
                {
                    Vector3 hit = ray.Position + t * ray.Direction - center;
                    float phi = MathHelper.Atan2(hit.X, hit.Z);
                    if (phi < 0.0)
                        phi += MathHelper.TwoPI;

                    if (hit.Y <= radius * cos_theta_min &&
                        hit.Y >= radius * cos_theta_max &&
                        phi >= phi_min && phi <= phi_max)
                    {

                        tmin = t;
                        sr.Normal = (temp + t * ray.Direction) / radius;   // points outwards
                        sr.LocalHitPoint = ray.Position + tmin * ray.Direction;
                        return (true);
                    }
                }
                t = (-b + e) / denom;    // larger root
                if (t > MathHelper.Epsilon)
                {
                    Vector3 hit = ray.Position + t * ray.Direction - center;
                    float phi = MathHelper.Atan2(hit.X, hit.Z);
                    if (phi < 0.0)
                        phi += MathHelper.TwoPI;

                    if (hit.Y <= radius * cos_theta_min &&
                        hit.Y >= radius * cos_theta_max &&
                        phi >= phi_min && phi <= phi_max)
                    {
                        tmin = t;
                        sr.Normal = (temp + t * ray.Direction) / radius;   // points outwards
                        sr.LocalHitPoint = ray.Position + tmin * ray.Direction;
                        return (true);
                    }
                }
            }
            return (false);
        }

        public override bool ShadowHit(Ray ray, ref float tmin)
        {
            if (Shadows)
            {
                float t;
                Vector3 temp = ray.Position - center;
                float a = Vector3.Dot(ray.Direction, ray.Direction);
                float b = 2.0f * Vector3.Dot(temp, ray.Direction);
                float c = Vector3.Dot(temp, temp) - radius * radius;
                float disc = b * b - 4.0f * a * c;

                if (disc < 0.0)
                    return (false);
                else
                {
                    float e = MathHelper.Sqrt(disc);
                    float denom = 2.0f * a;
                    t = (-b - e) / denom;    // smaller root

                    if (t > MathHelper.Epsilon)
                    {
                        Vector3 hit = ray.Position + t * ray.Direction - center;
                        float phi = MathHelper.Atan2(hit.X, hit.Z);
                        if (phi < 0.0)
                            phi += MathHelper.TwoPI;

                        if (hit.Y <= radius * cos_theta_min &&
                            hit.Y >= radius * cos_theta_max &&
                            phi >= phi_min && phi <= phi_max)
                        {

                            tmin = t;
                            return (true);
                        }
                    }
                    t = (-b + e) / denom;    // larger root
                    if (t > MathHelper.Epsilon)
                    {
                        Vector3 hit = ray.Position + t * ray.Direction - center;
                        float phi = MathHelper.Atan2(hit.X, hit.Z);
                        if (phi < 0.0)
                            phi += MathHelper.TwoPI;

                        if (hit.Y <= radius * cos_theta_min &&
                            hit.Y >= radius * cos_theta_max &&
                            phi >= phi_min && phi <= phi_max)
                        {
                            tmin = t;
                            return (true);
                        }
                    }
                }
                return (false);
            }
            return false;
        }
    }
}
