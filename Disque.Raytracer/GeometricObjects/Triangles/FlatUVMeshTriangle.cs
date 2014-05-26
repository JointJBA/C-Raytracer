using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Disque.Math;

namespace Disque.Raytracer.GeometricObjects.Triangles
{
    public class FlatUVMeshTriangle : FlatMeshTriangle
    {
        public FlatUVMeshTriangle(int i0, int i1, int i2, Mesh m, string name)
            : base(i0, i1, i2, m, name)
        {
        }

        public override bool Hit(Ray ray, ref float tmin, ref ShadeRec sr)
        {
            Vector3 v0 = (Mesh.Vertices[index0]);
            Vector3 v1 = (Mesh.Vertices[index1]);
            Vector3 v2 = (Mesh.Vertices[index2]);
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
            sr.Normal = Normal;
            sr.U = interpolate_u(beta, gamma);
            sr.V = interpolate_v(beta, gamma);
            return (true);
        }

        public override bool ShadowHit(Ray ray, ref float tmin)
        {
            Vector3 v0 = (Mesh.Vertices[index0]);
            Vector3 v1 = (Mesh.Vertices[index1]);
            Vector3 v2 = (Mesh.Vertices[index2]);
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
            return (true);
        }

    }
}
