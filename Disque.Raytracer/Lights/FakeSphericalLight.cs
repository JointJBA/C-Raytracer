using System;
using System.Collections.Generic;
using System.Text;
using Disque.Math;

namespace Disque.Raytracer.Lights
{
    public class FakeSphericalLight : Light
    {
        public float Radiance;
        public Vector3 Color, Location;
        public bool DistanceAttenuation;
        public float Radius;

        public override Vector3 GetDirection(ShadeRec sr)
        {
            float r = Radius;
            Vector3 new_location = Vector3.Zero;
            new_location.X = Location.X + r * (2.0f * MathHelper.RandomFloat()  - 1.0f);
            new_location.Y = Location.Y + r * (2.0f * MathHelper.RandomFloat() - 1.0f);
            new_location.Z = Location.Z + r * (2.0f * MathHelper.RandomFloat() - 1.0f);
            return (Vector3.Normalize(new_location - sr.LocalHitPoint));
        }

        public override Vector3 L(ShadeRec sr)
        {
            if (!DistanceAttenuation)
                return (Radiance * Color);
            else
            {
                float d = Vector3.Distance(sr.HitPoint, (Location));
                return (Radiance * Color / (d * d));
            }
        }

        public override bool InShadow(Ray ray, ShadeRec sr)
        {
            float t = float.MaxValue;
            int num_objects = sr.World.Objects.Count;
            float d = Vector3.Distance(Location, (ray.Position));
            for (int j = 0; j < num_objects; j++)
                if (sr.World.Objects[j].ShadowHit(ray, ref t) && t < d)
                    return (true);
            return (false);
        }
    }
}
