using System;
using System.Collections.Generic;
using System.Text;
using Disque.Math;
using Disque.Raytracer.Samplers;

namespace Disque.Raytracer.GeometricObjects.Primitives
{
    public class Rectangle : GeometricObject
    {
        Vector3 p0, a, b, normal;
        float a_len_squared, b_len_squared, area, inv_area;
        Sampler sampler;

        public Rectangle(Vector3 p, Vector3 _a, Vector3 _b, string name)
            : base(name)
        {
            p0 = p;
            a = _a;
            b = _b;
            a_len_squared = a.LengthSquared();
            b_len_squared = b.LengthSquared();
            area = a.Length() * b.Length();
            inv_area = 1.0f / area;
            normal = Vector3.Normalize(Vector3.Cross(a, b));
            Shadows = false;
        }

        public Rectangle(Vector3 p, Vector3 _a, Vector3 _b, Vector3 _normal, string name)
            : base(name)
        {
            p0 = p;
            a = _a;
            b = _b;
            a_len_squared = a.LengthSquared();
            b_len_squared = b.LengthSquared();
            area = a.Length() * b.Length();
            inv_area = 1.0f / area;
            normal = _normal;
            Shadows = false;
        }

        public override void SetSampler(Sampler s)
        {
            sampler = s;
        }

        public override bool Hit(Ray ray, ref float tmin, ref ShadeRec sr)
        {
            float t = Vector3.Dot((p0 - ray.Position), normal) / Vector3.Dot(ray.Direction, normal);
            if (t <= MathHelper.Epsilon)
                return (false);
            Vector3 p = ray.Position + t * ray.Direction;
            Vector3 d = p - p0;
            float ddota = Vector3.Dot(d, a);
            if (ddota < 0.0 || ddota > a_len_squared)
                return (false);
            float ddotb = Vector3.Dot(d, b);
            if (ddotb < 0.0 || ddotb > b_len_squared)
                return (false);
            tmin = t;
            sr.Normal = normal;
            sr.LocalHitPoint = p;
            return (true);
        }

        public override Vector3 Sample()
        {
            Vector2 sample_point = sampler.SampleUnitSquare();
            return (p0 + sample_point.X * a + sample_point.Y * b);
        }

        public override Vector3 GetNormal(Vector3 p)
        {
            return normal;
        }

        public override Vector3 GetNormal()
        {
            return normal;
        }

        public override float PDF(ShadeRec sr)
        {
            return inv_area;
        }

        public override bool ShadowHit(Ray ray, ref float tmin)
        {
            if (Shadows)
            {
                float t = Vector3.Dot((p0 - ray.Position), normal) / Vector3.Dot(ray.Direction, normal);
                if (t <= MathHelper.Epsilon)
                    return (false);
                Vector3 p = ray.Position + t * ray.Direction;
                Vector3 d = p - p0;
                float ddota = Vector3.Dot(d, a);
                if (ddota < 0.0 || ddota > a_len_squared)
                    return (false);
                float ddotb = Vector3.Dot(d, b);
                if (ddotb < 0.0 || ddotb > b_len_squared)
                    return (false);
                tmin = t;
                return (true);
            }
            return false;
        }
    }
}
