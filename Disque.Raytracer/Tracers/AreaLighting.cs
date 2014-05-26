using System;
using System.Collections.Generic;
using System.Text;
using Disque.Math;
using Disque.Raytracer.Worlds;

namespace Disque.Raytracer.Tracers
{
    public class AreaLighting : Tracer
    {
        public AreaLighting(World world)
            : base(world)
        {
        }

        public override Vector3 TraceRay(Ray ray, int depth)
        {
            if (depth > World.ViewPlane.MaxDepth)
                return Vector3.Zero;
            ShadeRec sr = World.HitObjects(ray);
            if (sr.Hit_an_object)
            {
                sr.Ray = ray;
                sr.Depth = depth;
                return sr.Material.Area_Light_Shade(sr);
            }
            else
            {
                return World.Background;
            }
        }
    }
}
