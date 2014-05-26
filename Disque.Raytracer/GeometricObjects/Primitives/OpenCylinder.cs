using System;
using System.Collections.Generic;
using System.Text;
using Disque.Math;

namespace Disque.Raytracer.GeometricObjects.Primitives
{
    public class OpenCylinder : GeometricObject
    {
        float y0, y1, radius, inv_radius;

        public OpenCylinder(float bottom, float top, float r, string name)
            : base(name)
        {
            y0 = bottom;
            y1 = top;
            radius = r;
            inv_radius = 1.0f / radius;
        }

        public override bool Hit(Ray ray, ref float tmin, ref ShadeRec sr)
        {
            float t;
            float ox = ray.Position.X;
            float oy = ray.Position.Y;
            float oz = ray.Position.Z;
            float dx = ray.Direction.X;
            float dy = ray.Direction.Y;
            float dz = ray.Direction.Z;
            float a = dx * dx + dz * dz;
            float b = 2.0f * (ox * dx + oz * dz);
            float c = ox * ox + oz * oz - radius * radius;
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

                    if (yhit > y0 && yhit < y1)
                    {
                        tmin = t;
                        sr.Normal = new Vector3((ox + t * dx) * inv_radius, 0.0f, (oz + t * dz) * inv_radius);
                        sr.Normal.Normalize();
                        if (Vector3.Dot(-ray.Direction, sr.Normal) < 0.0f)
                            sr.Normal = -sr.Normal;
                        sr.LocalHitPoint = ray.Position + tmin * ray.Direction;
                        return (true);
                    }
                }
                t = (-b + e) / denom;    // larger root
                if (t > MathHelper.Epsilon)
                {
                    float yhit = oy + t * dy;
                    if (yhit > y0 && yhit < y1)
                    {
                        tmin = t;
                        sr.Normal = new Vector3((ox + t * dx) * inv_radius, 0.0f, (oz + t * dz) * inv_radius);
                        sr.Normal.Normalize();
                        if (Vector3.Dot(-ray.Direction, sr.Normal) < 0.0f)
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
            if (Shadows)
            {
                float t;
                float ox = ray.Position.X;
                float oy = ray.Position.Y;
                float oz = ray.Position.Z;
                float dx = ray.Direction.X;
                float dy = ray.Direction.Y;
                float dz = ray.Direction.Z;
                float a = dx * dx + dz * dz;
                float b = 2.0f * (ox * dx + oz * dz);
                float c = ox * ox + oz * oz - radius * radius;
                float disc = b * b - 4.0f * a * c;
                if (disc < 0.0f)
                    return (false);
                else
                {
                    float e = MathHelper.Sqrt(disc);
                    float denom = 2.0f * a;
                    t = (-b - e) / denom;    // smaller root

                    if (t > MathHelper.Epsilon)
                    {
                        float yhit = oy + t * dy;

                        if (yhit > y0 && yhit < y1)
                        {
                            if (t < tmin)
                            {
                                tmin = t;
                                return (true);
                            }
                        }
                    }
                    t = (-b + e) / denom;    // larger root
                    if (t > MathHelper.Epsilon)
                    {
                        float yhit = oy + t * dy;
                        if (yhit > y0 && yhit < y1)
                        {
                            if (t < tmin)
                            {
                                tmin = t;		// this is very important
                                return (true);
                            }
                        }
                    }
                }
                return (false);
            }
            return false;
        }
    }
}
