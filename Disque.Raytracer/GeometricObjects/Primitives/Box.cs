using System;
using System.Collections.Generic;
using System.Text;
using Disque.Math;

namespace Disque.Raytracer.GeometricObjects.Primitives
{
    public class Box : GeometricObject
    {           
        public Vector3 Min, Max;
        BBox bbox;
     
        public Box(Vector3 min, Vector3 max, string name)
            : base(name)
        {
            Min = min;
            Max = max;
            bbox = GetBoundingBox();
        }

        public Vector3 GetNormal(int face_hit)
        {
            switch (face_hit)
            {
                case 0:
                    return (new Vector3(-1, 0, 0));	// -x face
                case 1:
                    return (new Vector3(0, -1, 0));	// -y face
                case 2:
                    return (new Vector3(0, 0, -1));	// -z face
                case 3:
                    return (new Vector3(1, 0, 0));	// +x face
                case 4:
                    return (new Vector3(0, 1, 0));	// +y face
                case 5:
                    return (new Vector3(0, 0, 1));	// +z face
                default:
                    return new Vector3(-1, 0, 0);
            }
        }

        public override bool Hit(Ray ray, ref float tmin, ref ShadeRec s)
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
            int face_in, face_out;
            if (tx_min > ty_min)
            {
                t0 = tx_min;
                face_in = (a >= 0.0f) ? 0 : 3;
            }
            else
            {
                t0 = ty_min;
                face_in = (b >= 0.0) ? 1 : 4;
            }

            if (tz_min > t0)
            {
                t0 = tz_min;
                face_in = (c >= 0.0) ? 2 : 5;
            }
            if (tx_max < ty_max)
            {
                t1 = tx_max;
                face_out = (a >= 0.0) ? 3 : 0;
            }
            else
            {
                t1 = ty_max;
                face_out = (b >= 0.0) ? 4 : 1;
            }
            if (tz_max < t1)
            {
                t1 = tz_max;
                face_out = (c >= 0.0) ? 5 : 2;
            }

            if (t0 < t1 && t1 > MathHelper.BigEpsilon)
            { 
                if (t0 > MathHelper.BigEpsilon)
                {
                    tmin = t0;
                    s.Normal = GetNormal(face_in);
                }
                else
                {
                    tmin = t1;
                    s.Normal = GetNormal(face_out);
                }
                s.LocalHitPoint = ray.Position + tmin * ray.Direction;
                return (true);
            }
            else
                return (false);
        }

        public override bool ShadowHit(Ray ray, ref float tmin)
        {
            if (!Shadows)
                return false;
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

            // this is the same as Listing 19.1 down to the statement float t0, t1;

            int face_in, face_out;

            // find largest entering t value

            if (tx_min > ty_min)
            {
                t0 = tx_min;
                face_in = (a >= 0.0) ? 0 : 3;
            }
            else
            {
                t0 = ty_min;
                face_in = (b >= 0.0) ? 1 : 4;
            }

            if (tz_min > t0)
            {
                t0 = tz_min;
                face_in = (c >= 0.0) ? 2 : 5;
            }
            if (tx_max < ty_max)
            {
                t1 = tx_max;
                face_out = (a >= 0.0) ? 3 : 0;
            }
            else
            {
                t1 = ty_max;
                face_out = (b >= 0.0) ? 4 : 1;
            }

            if (tz_max < t1)
            {
                t1 = tz_max;
                face_out = (c >= 0.0) ? 5 : 2;
            }
            if (t0 < t1 && t1 > MathHelper.BigEpsilon)
            {
                if (t0 > MathHelper.BigEpsilon)
                {
                    tmin = t0;
                }
                else
                {
                    tmin = t1;
                }
                return (true);
            }
            else
                return false;
        }

        public override BBox GetBoundingBox()
        {
            return new BBox(Min, Max);
        }
    }
}
