using System;
using System.Collections.Generic;
using System.Text;
using Disque.Math;

namespace Disque.Raytracer.GeometricObjects.Primitives
{
    public class SmoothTriangle : GeometricObject
    {
        Vector3 v0, v1, v2, n0, n1, n2;

        public SmoothTriangle(Vector3 a, Vector3 b, Vector3 c, string name)
            : base(name)
        {
            v0 = a;
            v1 = b;
            v2 = c;
            n0 = n1 = n2 = Vector3.Up;
        }

        public override BBox GetBoundingBox()
        {
            float delta = 0.0001f;
            return (new BBox(MathHelper.Min(MathHelper.Min(v0.X, v1.X), v2.X) - delta, MathHelper.Max(MathHelper.Max(v0.X, v1.X), v2.X) + delta,
                        MathHelper.Min(MathHelper.Min(v0.Y, v1.Y), v2.Y) - delta, MathHelper.Max(MathHelper.Max(v0.Y, v1.Y), v2.Y) + delta,
                        MathHelper.Min(MathHelper.Min(v0.Z, v1.Z), v2.Z) - delta, MathHelper.Max(MathHelper.Max(v0.Z, v1.Z), v2.Z) + delta));
        }

        protected Vector3 interpolate_normal(float beta, float gamma)
        {
            Vector3 normal = ((1.0f - beta - gamma) * n0 + beta * n1 + gamma * n2);
            normal.Normalize();
            return (normal);
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
            float r = r = e * l - h * i;
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
            sr.Normal = interpolate_normal(beta, gamma);
            sr.LocalHitPoint = ray.Position + t * ray.Direction;
            return (true);
        }

        public override bool ShadowHit(Ray ray, ref float tmin)
        {
            if (!Shadows)
                return false;
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
            float r = r = e * l - h * i;
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
            return (true);
        }
    }
}
