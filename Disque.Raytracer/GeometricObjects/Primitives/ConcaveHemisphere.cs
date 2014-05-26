using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Disque.Math;

namespace Disque.Raytracer.GeometricObjects.Primitives
{
    public class ConcaveHemisphere : GeometricObject
    {
        Vector3 center = Vector3.Zero;
        float radius = 1;

        public ConcaveHemisphere(Vector3 c, float r, string name)
            : base(name)
        {
            center = c;
            radius = r;
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
                t = (-b - e) / denom;
                if (t > MathHelper.Epsilon)
                {
                    Vector3 hit = ray.Position + t * ray.Direction - center;
                    if (hit.Y > center.Y)
                    {
                        tmin = t;
                        sr.Normal = -(temp + t * ray.Direction) / radius;   // points outwards
                        sr.LocalHitPoint = ray.Position + tmin * ray.Direction;
                        return (true);
                    }
                }
                t = (-b + e) / denom;    // larger root
                if (t > MathHelper.Epsilon)
                {

                    Vector3 hit = ray.Position + t * ray.Direction - center;
                    if (hit.Y > center.Y)
                    {
                        tmin = t;
                        sr.Normal = -(temp + t * ray.Direction) / radius;   // points outwards
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
                    t = (-b - e) / denom;
                    if (t > MathHelper.Epsilon)
                    {
                        Vector3 hit = ray.Position + t * ray.Direction - center;
                        if (t < tmin && hit.Y > 0)
                        {
                            tmin = t;
                            return (true);
                        }
                    }
                    t = (-b + e) / denom;    // larger root

                    if (t > MathHelper.Epsilon)
                    {
                        Vector3 hit = ray.Position + t * ray.Direction - center;
                        if (t < tmin && hit.Y > 0)
                        {
                            tmin = t;
                            return (true);
                        }
                    }
                }
                return false;
            }
            return false;
        }
    }
}
