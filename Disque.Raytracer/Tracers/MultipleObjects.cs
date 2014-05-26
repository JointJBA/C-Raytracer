using System;
using System.Collections.Generic;
using System.Text;
using Disque.Raytracer.Worlds;
using Disque.Math;

namespace Disque.Raytracer.Tracers
{
    public class MultipleObjects : Tracer
    {
        public MultipleObjects(World world)
            : base(world)
        {
        }

        public override Vector3 TraceRay(Ray ray)
        {
            ShadeRec sr = World.HitObjects(ray);
            if (sr.Hit_an_object)
                return sr.Color;
            else
                return World.Background;
        }

        internal static ShadeRec trace(Tracer tr, Ray ray)
        {
            ShadeRec sr = new ShadeRec(tr.World);
            float t = 0;
            Vector3 normal = Vector3.Zero;
            Vector3 local_hit_point = Vector3.Zero;
            float tmin = float.MaxValue;
            int num_objects = tr.World.Objects.Count;
            for (int j = 0; j < num_objects; j++)
                if (tr.World.Objects[j].Hit(ray, ref t, ref sr) && (t < tmin))
                {
                    sr.Hit_an_object = true;
                    tmin = t;
                    sr.Material = tr.World.Objects[j].Material;
                    sr.HitPoint = ray.Position + t * ray.Direction;
                    normal = sr.Normal;
                    local_hit_point = sr.LocalHitPoint;
                }

            if (sr.Hit_an_object)
            {
                sr.T = tmin;
                sr.Normal = normal;
                sr.LocalHitPoint = local_hit_point;
            }
            return sr;
        }
    }
}
