using System;
using System.Collections.Generic;
using System.Text;
using Disque.Math;

namespace Disque.Raytracer.GeometricObjects.Primitives
{
    public class OpenCone : GeometricObject
    {
        float r, h;

        public OpenCone(float radius, float height, string name)
            : base(name)
        {
            r = radius;
            h = height;
        }

        public override bool Hit(Ray ray, ref float tmin, ref ShadeRec sr)
        {
            float t = 0;
            float ox = ray.Position.X;
            float oy = ray.Position.Y;
            float oz = ray.Position.Z;
            float dx = ray.Direction.X;
            float dy = ray.Direction.Y;
            float dz = ray.Direction.Z;
            float h2r2 = h * h / (r * r);
            float a = h2r2 * dx * dx + h2r2 * dz * dz - dy * dy;
            float b = 2.0f * (h2r2 * ox * dx - oy * dy + h * dy + h2r2 * oz * dz);
            float c = h2r2 * ox * ox - oy * oy + 2 * h * oy - h * h + h2r2 * oz * oz;
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
                    Vector3 hitpoint = ray.Position + t * ray.Direction;
                    if (hitpoint.Y > 0 && hitpoint.Y < h)
                    {
                        tmin = t;
                        sr.Normal = new Vector3(2.0f * h * hitpoint.X / r, -2.0f * (hitpoint.Y - h), 2.0f * h * hitpoint.Z / r);
                        sr.Normal.Normalize();
                        if (Vector3.Dot(-ray.Direction, sr.Normal) < 0.0)
                            sr.Normal = -sr.Normal;
                        sr.LocalHitPoint = ray.Position + tmin * ray.Direction;
                        return (true);
                    }
                }
                t = (-b + e) / denom;    // larger root
                if (t > MathHelper.Epsilon)
                {
                    Vector3 hitpoint = ray.Position + t * ray.Direction;
                    if (hitpoint.Y > 0 && hitpoint.Y < h)
                    {
                        tmin = t;
                        sr.Normal = new Vector3(2.0f * h * hitpoint.X / r, -2.0f * (hitpoint.Y - h), 2.0f * h * hitpoint.Z / r);
                        sr.Normal.Normalize();
                        if (Vector3.Dot(-ray.Direction, sr.Normal) < 0.0)
                            sr.Normal = -sr.Normal;
                        sr.LocalHitPoint = ray.Position + tmin * ray.Direction;
                        return (true);
                    }
                }
            }
            return (false);
        }

        public override bool ShadowHit(Ray ray, ref float tmin)
        {
            if (!Shadows)
                return false;
            float t = 0;
            float ox = ray.Position.X;
            float oy = ray.Position.Y;
            float oz = ray.Position.Z;
            float dx = ray.Direction.X;
            float dy = ray.Direction.Y;
            float dz = ray.Direction.Z;
            float h2r2 = h * h / (r * r);
            float a = h2r2 * dx * dx + h2r2 * dz * dz - dy * dy;
            float b = 2.0f * (h2r2 * ox * dx - oy * dy + h * dy + h2r2 * oz * dz);
            float c = h2r2 * ox * ox - oy * oy + 2 * h * oy - h * h + h2r2 * oz * oz;
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
                    float yhit = oy + t * dy;
                    if (t < tmin && yhit < h && yhit > 0)
                    {
                        tmin = t;
                        return (true);
                    }
                }
                t = (-b + e) / denom;

                if (t > MathHelper.Epsilon)
                {
                    float yhit = oy + t * dy;
                    if (t < tmin && yhit < h && yhit > 0)
                    {
                        tmin = t;
                        return (true);
                    }
                }
            }
            return (false);
        }
    }
}
