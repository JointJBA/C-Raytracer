using System;
using System.Collections.Generic;
using System.Text;
using Disque.Math;

namespace Disque.Raytracer.GeometricObjects.Primitives
{
    public class Triangle : GeometricObject
    {
        Vector3 v0, v1, v2, Normal;
        BBox bbox;

        public Triangle(Vector3 p0, Vector3 p1, Vector3 p2, string name)
            : base(name)
        {
            v0 = p0;
            v1 = p1; 
            v2 = p2;
            compute_normal();
            SetBoundingBox();
        }

        void compute_normal()
        {
            Normal = Vector3.Cross((v1 - v0), (v2 - v0));
            Normal.Normalize();
        }

        public override BBox GetBoundingBox()
        {
            return bbox;
        }

        public override void SetBoundingBox()
        {
            float delta = 0.000001f;
            bbox = (new BBox(MathHelper.Min(MathHelper.Min(v0.X, v1.X), v2.X) - delta, MathHelper.Max(MathHelper.Max(v0.X, v1.X), v2.X) + delta,
                         MathHelper.Min(MathHelper.Min(v0.Y, v1.Y), v2.Y) - delta, MathHelper.Max(MathHelper.Max(v0.Y, v1.Y), v2.Y) + delta,
                         MathHelper.Min(MathHelper.Min(v0.Z, v1.Z), v2.Z) - delta, MathHelper.Max(MathHelper.Max(v0.Z, v1.Z), v2.Z) + delta));
        }

        public override bool Hit(Ray ray, ref float tmin, ref ShadeRec sr)
        {
            float a = v0.X - v1.X, b = v0.X - v2.X, c = ray.Direction.X, d = v0.X - ray.Position.X;
            float e = v0.Y - v1.Y, f = v0.Y - v2.Y, g = ray.Direction.Y, h = v0.Y - ray.Position.Y;
            float i = v0.Z - v1.Z, j = v0.Z - v2.Z, k = ray.Direction.Z, l = v0.Z - ray.Position.Z;
            float m = f * k - g * j, n = h * k - g * l, p = f * l - h * j;
            float q = g * i - e * k, s = e * j - f * i;
            float inv_denom = 1.0f / (a * m + b * q + c * s);
            float e1 = d * m - b * n - c * p;
            float beta = e1 * inv_denom;
            if (beta < 0.0)
                return (false);
            float r = e * l - h * i;
            float e2 = a * n + d * q + c * r;
            float gamma = e2 * inv_denom;
            if (gamma < 0.0)
                return (false);
            if (beta + gamma > 1.0)
                return (false);
            float e3 = a * p - b * r + d * s;
            float t = e3 * inv_denom;
            if (t < MathHelper.Epsilon)
                return (false);
            tmin = t;
            sr.Normal = Normal;
            sr.LocalHitPoint = ray.Position + t * ray.Direction;

            return (true);
        }

        public override bool ShadowHit(Ray ray, ref float tmin)
        {
            if (Shadows)
            {
                float a = v0.X - v1.X, b = v0.X - v2.X, c = ray.Direction.X, d = v0.X - ray.Position.X;
                float e = v0.Y - v1.Y, f = v0.Y - v2.Y, g = ray.Direction.Y, h = v0.Y - ray.Position.Y;
                float i = v0.Z - v1.Z, j = v0.Z - v2.Z, k = ray.Direction.Z, l = v0.Z - ray.Position.Z;
                float m = f * k - g * j, n = h * k - g * l, p = f * l - h * j;
                float q = g * i - e * k, s = e * j - f * i;
                float inv_denom = 1.0f / (a * m + b * q + c * s);
                float e1 = d * m - b * n - c * p;
                float beta = e1 * inv_denom;
                if (beta < 0.0f)
                    return (false);
                float r = e * l - h * i;
                float e2 = a * n + d * q + c * r;
                float gamma = e2 * inv_denom;
                if (gamma < 0.0f)
                    return (false);
                if (beta + gamma > 1.0f)
                    return (false);
                float e3 = a * p - b * r + d * s;
                float t = e3 * inv_denom;
                if (t < MathHelper.Epsilon)
                    return (false);
                tmin = t;
                return true;
            }
            return false;
        }
    }
}
