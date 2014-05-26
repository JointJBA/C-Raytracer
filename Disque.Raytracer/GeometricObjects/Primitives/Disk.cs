using System;
using System.Collections.Generic;
using System.Text;
using Disque.Math;
using Disque.Raytracer.Samplers;

namespace Disque.Raytracer.GeometricObjects.Primitives
{
    public class Disk : GeometricObject
    {
        public Vector3 Center;
        public Vector3 Normal;
        public float Radius;
        float r_squared;
        Sampler sampler;
        float area;
        float inv_area;
        Vector3 u, v, w;

        public Disk(Vector3 center, Vector3 normal, float radius, string name)
            : base(name)
        {
            Center = center;
            Normal = normal;
            Radius = radius;
            r_squared = radius * radius;
            area = 0.5f * MathHelper.PI * r_squared;
            inv_area = 1.0f / area;
        }

        public override bool Hit(Ray ray, ref float tmin, ref ShadeRec sr)
        {
            float t = Vector3.Dot((Center - ray.Position), Normal) / Vector3.Dot(ray.Direction, Normal);
            if (t <= MathHelper.Epsilon)
                return (false);
            Vector3 p = ray.Position + t * ray.Direction;
            if (Vector3.DistanceSquared(Center, p) < r_squared)
            {
                tmin = t;
                sr.Normal = Normal;
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
                float t = Vector3.Dot((Center - ray.Position), Normal) / Vector3.Dot(ray.Direction, Normal);
                if (t <= MathHelper.Epsilon)
                    return (false);
                Vector3 p = ray.Position + t * ray.Direction;
                if (Vector3.DistanceSquared(Center, p) < r_squared)
                {
                    tmin = t;
                    return (true);
                }
                else
                    return (false);
            }
            return false;
        }

        public override void SetSampler(Sampler s)
        {
            sampler = s;
            sampler.MapSamplesToDisk();
        }

        public virtual void ComputeUVW()
        {
            w = Normal;
            u = new Vector3(1000, 1000, 0);
            u.Z = Vector3.Dot(Center, Normal) - u.X * Normal.X - u.Y * Normal.Y;
            u = u - Center;
            u.Normalize();
            v = Vector3.Cross(w, u);
            v.Normalize();
        }

        public override Vector3 Sample()
        {
            Vector2 sample_point = sampler.SampleUnitDisk();
            return (Center + sample_point.X * u + sample_point.Y * v);
        }

        public override float PDF(ShadeRec sr)
        {
            return inv_area;
        }

        public override Vector3 GetNormal()
        {
            return Normal;
        }

        public override Vector3 GetNormal(Vector3 p)
        {
            return Normal;
        }
    }
}