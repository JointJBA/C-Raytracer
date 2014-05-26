using System;
using System.Collections.Generic;
using System.Text;
using Disque.Math;

namespace Disque.Math
{
    public struct BBox
    {
        public Vector3 Min, Max;

        public BBox(Vector3 min, Vector3 max)
        {
            Min = min;
            Max = max;
        }

        public BBox(float x0, float x1, float y0, float y1, float z0, float z1)
        {
            Min = new Vector3(x0, y0, z0);
            Max = new Vector3(x1, y1, z1);
        }

        public bool Inside(Vector3 p)
        {
            float x0 = Min.X, x1 = Max.X;
            float y0 = Min.Y, y1 = Max.Y;
            float z0 = Min.Z, z1 = Max.Z;
            return ((p.X > x0 && p.X < x1) && (p.Y > y0 && p.Y < y1) && (p.Z > z0 && p.Z < z1));
        }

        public bool Hit(Ray ray)
        {
            float ox = ray.Position.X; float oy = ray.Position.Y; float oz = ray.Position.Z;
            float dx = ray.Direction.X; float dy = ray.Direction.Y; float dz = ray.Direction.Z;
            float tx_min, ty_min, tz_min;
            float tx_max, ty_max, tz_max;
            float x0 = Min.X, x1 = Max.X;
            float y0 = Min.Y, y1 = Max.Y;
            float z0 = Min.Z, z1 = Max.Z;
            float a = 1.0f / dx;
            if (a >= 0)
            {
                tx_min = (x0 - ox) * a;
                tx_max = (x1 - ox) * a;
            }
            else
            {
                tx_min = (x1 - ox) * a;
                tx_max = (x0 - ox) * a;
            }

            float b = 1.0f / dy;
            if (b >= 0)
            {
                ty_min = (y0 - oy) * b;
                ty_max = (y1 - oy) * b;
            }
            else
            {
                ty_min = (y1 - oy) * b;
                ty_max = (y0 - oy) * b;
            }

            float c = 1.0f / dz;
            if (c >= 0)
            {
                tz_min = (z0 - oz) * c;
                tz_max = (z1 - oz) * c;
            }
            else
            {
                tz_min = (z1 - oz) * c;
                tz_max = (z0 - oz) * c;
            }
            float t0, t1;
            if (tx_min > ty_min)
                t0 = tx_min;
            else
                t0 = ty_min;

            if (tz_min > t0)
                t0 = tz_min;
            if (tx_max < ty_max)
                t1 = tx_max;
            else
                t1 = ty_max;

            if (tz_max < t1)
                t1 = tz_max;

            return (t0 < t1 && t1 > MathHelper.Epsilon);
        }

        public void Union(BBox b2)
        {
            Vector3 min = Vector3.Zero;
            min.X = MathHelper.Min(Min.X, b2.Min.X);
            min.Y = MathHelper.Min(Min.Y, b2.Min.Y);
            min.Z = MathHelper.Min(Min.Z, b2.Min.Z);
            Min = min;
            Vector3 max = Vector3.Zero;
            max.X = MathHelper.Max(Max.X, b2.Max.X);
            max.Y = MathHelper.Max(Max.Y, b2.Max.Y);
            max.Z = MathHelper.Max(Max.Z, b2.Max.Z);
            Max = max;
        }

        public bool Overlaps(BBox b)
        {
            bool x = (Max.X >= b.Min.X) && (Min.X <= b.Max.X);
            bool y = (Max.Y >= b.Min.Y) && (Min.Y <= b.Max.Y);
            bool z = (Max.Z >= b.Min.Z) && (Min.Z <= b.Max.Z);
            return (x && y && z);
        }

        public static BBox Join(BBox b1, BBox b2)
        {
            BBox b3 = b1;
            b3.Union(b2);
            return b3;
        }

        public static bool operator ==(BBox a, BBox b)
        {
            return a.Max == b.Max && a.Min == b.Min;
        }

        public static bool operator !=(BBox a, BBox b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            BBox b = (BBox)obj;
            return b == this;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
