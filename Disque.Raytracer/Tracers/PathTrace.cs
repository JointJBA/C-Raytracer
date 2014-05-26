using System;
using System.Collections.Generic;
using System.Text;
using Disque.Raytracer.Worlds;
using Disque.Math;

namespace Disque.Raytracer.Tracers
{
    public class PathTrace : Tracer
    {
        public PathTrace(World world)
            : base(world)
        {
        }

        public override Vector3 TraceRay(Ray ray, int depth)
        {
            if (depth > World.ViewPlane.MaxDepth)
                return (Vector3.Zero);
            else
            {
                ShadeRec sr = (World.HitObjects(ray));
                if (sr.Hit_an_object)
                {
                    sr.Depth = depth;
                    sr.Ray = ray;
                    return (sr.Material.Path_Shade(sr));
                }
                else
                    return (World.Background);
            }
        }

        public override Vector3 TraceRay(Ray ray, ref float tmin, int depth)
        {
            if (depth > World.ViewPlane.MaxDepth)
                return Vector3.Zero;
            else
            {
                ShadeRec sr = (World.HitObjects(ray));
                if (sr.Hit_an_object)
                {
                    sr.Depth = depth;
                    sr.Ray = ray;
                    tmin = sr.T;
                    return (sr.Material.Path_Shade(sr));
                }
                else
                {
                    tmin = float.MaxValue;
                    return (World.Background);
                }
            }
        }
    }
}
