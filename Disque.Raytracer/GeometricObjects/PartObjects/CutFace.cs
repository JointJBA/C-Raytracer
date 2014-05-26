using System;
using System.Collections.Generic;
using System.Text;
using Disque.Math;

namespace Disque.Raytracer.GeometricObjects.PartObjects
{
    public class CutFace : GeometricObject
    {
        float size, radius;

        public CutFace(float s, float r, string name)
            : base(name)
        {
            size = s;
            radius = r;
        }

        public override bool Hit(Ray ray, ref float tmin, ref ShadeRec sr)
        {
            float t = -ray.Position.Y / ray.Direction.Y;
            if (t > MathHelper.Epsilon)
            {
                double xi = ray.Position.X + t * ray.Direction.X;
                double zi = ray.Position.Z + t * ray.Direction.Z;
                double d = xi * xi + zi * zi;
                double size_on_two = 0.5 * size;
                if ((-size_on_two <= xi && xi <= size_on_two) && (-size_on_two <= zi && zi <= size_on_two)
                        && d >= radius * radius)
                {
                    tmin = t;
                    sr.Normal = new Vector3(0.0f, 1.0f, 0.0f);
                    sr.LocalHitPoint = ray.Position + t * ray.Direction;
                    return (true);
                }
                else
                    return (false);
            }

            return (false);
        }

        public override bool ShadowHit(Ray ray, ref float tmin)
        {
            if (Shadows)
            {
                float t = -ray.Position.Y / ray.Direction.Y;
                if (t > MathHelper.Epsilon)
                {
                    double xi = ray.Position.X + t * ray.Direction.X;
                    double zi = ray.Position.Z + t * ray.Direction.Z;
                    double d = xi * xi + zi * zi;
                    double size_on_two = 0.5 * size;
                    if ((-size_on_two <= xi && xi <= size_on_two) && (-size_on_two <= zi && zi <= size_on_two)
                            && d >= radius * radius)
                    {
                        tmin = t;
                        return (true);
                    }
                    else
                        return (false);
                }
                return (false);
            }
            return false;
        }
    }
}
