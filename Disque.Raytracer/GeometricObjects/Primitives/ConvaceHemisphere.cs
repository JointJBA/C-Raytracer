using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Disque.Math;
using Disque.Raytracer.Math;

namespace Disque.Raytracer.GeometricObjects.Primitives
{
    public class ConvaceHemisphere : GeometricObject
    {
        Vector3 center;
        float radius;

        public ConcaveHemisphere(string name)
        {
        }

        public void SetPosition(Vector3 c)
        {
            center = c;
        }

        public void SetRadius(float r)
        {
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
                t = (-b - e) / denom;    // smaller root
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
                t = (-b + e) / denom;
                if (t > MathHelper.Epsilon)
                {
                    Vector3 hit = ray.Position + t * ray.Direction - center;
                    if (hit.Y > center.Y)
                    {
                        tmin = t;
                        sr.Normal = -(temp + t * ray.Direction) / radius;
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
            }
        }
    }
}
