using System;
using System.Collections.Generic;
using System.Text;
using Disque.Math;
using Disque.Raytracer.Samplers;

namespace Disque.Raytracer.GeometricObjects.Primitives
{
    public class Sphere : GeometricObject
    {
        BBox bbox;
        Vector3 Center = Vector3.Zero;
        float Radius = 1;
        Sampler sampler;
        int num_samples;
        float inv_area;
        Vector3 u, v, w;

        public Sphere(Vector3 c, float r, string name)
            : base(name)
        {
            Center = c;
            Radius = r;
            num_samples = (0);
            inv_area = (0.25f * MathHelper.InvPI / (r * r));
            SetBoundingBox();
        }

        public override bool Hit(Ray ray, ref float tmin, ref ShadeRec sr)
        {
            float t;
            Vector3 temp = ray.Position - Center;
            float a = Vector3.Dot(ray.Direction, ray.Direction);
            float b = 2.0f * Vector3.Dot(temp, ray.Direction);
            float c = Vector3.Dot(temp, temp) - Radius * Radius;
            float disc = b * b - 4.0f * a * c;

            if (disc < 0.0)
                return (false);
            else
            {
                float e = MathHelper.Sqrt(disc);
                float denom = 2.0f * a;
                t = (-b - e) / denom;    // smaller root

                if (t > MathHelper.BigEpsilon)
                {
                    tmin = t;
                    sr.Normal = Vector3.Normalize((temp + t * ray.Direction) / Radius);
                    sr.LocalHitPoint = ray.Position + t * ray.Direction;
                    return (true);
                }

                t = (-b + e) / denom;    // larger root

                if (t > MathHelper.BigEpsilon)
                {
                    tmin = t;
                    sr.Normal = Vector3.Normalize((temp + t * ray.Direction) / Radius);
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
            Vector3 temp = ray.Position - Center;
            float a = Vector3.Dot(ray.Direction, ray.Direction);
            float b = 2.0f * Vector3.Dot(temp, ray.Direction);
            float c = Vector3.Dot(temp, temp) - Radius * Radius;
            float disc = b * b - 4.0f * a * c;
            if (disc < 0.0)
                return (false);
            else
            {
                float e = MathHelper.Sqrt(disc);
                float denom = 2.0f * a;
                t = (-b - e) / denom;    // smaller root

                if (t > MathHelper.BigEpsilon)
                {
                    tmin = t;
                    return (true);
                }
                t = (-b + e) / denom;    // larger root

                if (t > MathHelper.BigEpsilon)
                {
                    tmin = t;
                    return (true);
                }
            }
            return (false);
        }

        public override void SetSampler(Sampler s)
        {
            num_samples = s.NumSamples;
            sampler = s;
            sampler.MapSamplesToSphere();
            base.SetSampler(s);
        }

        public virtual void ComputeUVW()
        {
        }

        public override Vector3 Sample()
        {
            Vector3 s = sampler.SampleSphere();
            return s + Center;
        }

        public override float PDF(ShadeRec sr)
        {
            return inv_area;
        }

        public override Vector3 GetNormal(Vector3 p)
        {
            Vector3 normal = p - Center;
            normal.Normalize();
            return normal;
        }

        public override BBox GetBoundingBox()
        {
            return bbox;
        }

        public override void SetBoundingBox()
        {
            bbox = new BBox(Center + new Vector3(-Radius), Center + new Vector3(Radius));
        }
    }
}
