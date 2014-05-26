using System;
using System.Collections.Generic;
using System.Text;
using Disque.Raytracer.BRDFs;
using Disque.Math;
using Disque.Raytracer.Samplers;

namespace Disque.Raytracer.Materials
{
    public class Reflective : Phong
    {
        PerfectSpecular reflective = new PerfectSpecular();

        public void SetReflectiveRC(float k)
        {
            reflective.ReflectionCoefficient = k;
        }

        public void SetRColor(Vector3 c)
        {
            reflective.Color = c;
        }

        public override Vector3 Shade(ShadeRec sr)
        {
            Vector3 L = base.Shade(sr);
            Vector3 wo = -sr.Ray.Direction;
            Vector3 wi = Vector3.Zero;
            Vector3 fr = reflective.Sample_F(sr, wo, ref wi);
            Ray reflected_ray = new Ray(sr.HitPoint, wi);
            reflected_ray.Depth = sr.Depth + 1;
            L += fr * sr.World.Tracer.TraceRay(reflected_ray, sr.Depth + 1) * Vector3.Dot(sr.Normal, wi);
            return (L);
        }

        public override Vector3 Area_Light_Shade(ShadeRec sr)
        {
            Vector3 L = base.Shade(sr);
            Vector3 wo = -sr.Ray.Direction;
            Vector3 wi = Vector3.Zero;
            Vector3 fr = reflective.Sample_F(sr, wo, ref wi);
            Ray reflected_ray = new Ray(sr.HitPoint, wi);
            reflected_ray.Depth = sr.Depth + 1;
            L += fr * sr.World.Tracer.TraceRay(reflected_ray, sr.Depth + 1) * (sr.Normal * wi);
            return (L);
        }

        public override Vector3 Global_Shade(ShadeRec sr)
        {
            Vector3 wo = -sr.Ray.Direction;
            Vector3 wi = Vector3.Zero;
            float pdf = 0;
            Vector3 fr = reflective.Sample_F(sr, wo, ref wi, ref pdf);
            Ray reflected_ray = new Ray(sr.HitPoint, wi);
            if (sr.Depth == 0)
                return (fr * sr.World.Tracer.TraceRay(reflected_ray, sr.Depth + 2) * (sr.Normal * wi) / pdf);
            else
                return (fr * sr.World.Tracer.TraceRay(reflected_ray, sr.Depth + 1) * (sr.Normal * wi) / pdf);
        }

        public override Vector3 Path_Shade(ShadeRec sr)
        {
            Vector3 wo = -sr.Ray.Direction;
            Vector3 wi = Vector3.Zero;
            float pdf = 0;
            Vector3 fr = reflective.Sample_F(sr, wo, ref wi, ref pdf);
            Ray reflected_ray = new Ray(sr.HitPoint, wi);
            return (fr * sr.World.Tracer.TraceRay(reflected_ray, sr.Depth + 1) * (sr.Normal * wi) / pdf);
        }

        public override void SetSampler(Sampler sampler)
        {
            reflective.SetSampler(sampler);
            base.SetSampler(sampler);
        }
    }
}
