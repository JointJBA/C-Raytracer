using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Disque.Math;

namespace Disque.Raytracer.GeometricObjects.Primitives
{
    public class ConcaveSphere : GeometricObject
    {
        Vector3 center = Vector3.Zero;
        float radius = 1;

        public ConcaveSphere(Vector3 c, float r, string name)
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
                if (t > MathHelper.BigEpsilon)
                {
                    tmin = t;
                    sr.Normal = -(temp + t * ray.Direction) / radius;
                    sr.LocalHitPoint = ray.Position + t * ray.Direction;
                    return (true);
                }
                t = (-b + e) / denom;
                if (t > MathHelper.BigEpsilon)
                {
                    tmin = t;
                    sr.Normal = -(temp + t * ray.Direction) / radius;
                    sr.LocalHitPoint = ray.Position + t * ray.Direction;
                    return (true);
                }
            }
            return (false);
        }

        public override bool ShadowHit(Ray ray, ref float tmin)
        {
            if (!Shadows)
                return false;
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
                if (t > MathHelper.BigEpsilon)
                {
                    if (t < tmin)
                    {
                        tmin = t;
                        return (true);
                    }
                }
                t = (-b + e) / denom;    // larger root
                if (t > MathHelper.BigEpsilon)
                {
                    if (t < tmin)
                    {
                        tmin = t;
                        return (true);
                    }
                }
            }
            return false;
        }
    }
}
