using System;
using System.Collections.Generic;
using System.Text;
using Disque.Raytracer.Samplers;
using Disque.Raytracer.Materials;
using Disque.Math;

namespace Disque.Raytracer.Lights
{
    public class EnvironmentLight : Light
    {
        Sampler sampler;
        public Material Material;
        Vector3 u, v, w, wi;

        public override Light Clone()
        {
            throw new NotImplementedException();
        }

        public void SetSampler(Sampler s)
        {
            sampler = s;
            sampler.MapSamplesToHemisphere(1);
        }

        public override Vector3 GetDirection(ShadeRec sr)
        {
            w = sr.Normal;
            v = Vector3.Cross(new Vector3(0.0034f, 1, 0.0071f), w);
            v.Normalize();
            u = Vector3.Cross(v, w);
            Vector3 sp = sampler.SampleHemisphere();
            wi = sp.X * u + sp.Y * v + sp.Z * w;
            return (wi);
        }

        public override Vector3 L(ShadeRec sr)
        {
            return (Material.Get_Le(sr));
        }

        public override float PDF(ShadeRec sr)
        {
            return (Vector3.Dot(sr.Normal, wi) * MathHelper.InvPI);
        }

        public override bool InShadow(Ray ray, ShadeRec sr)
        {
            float t = float.MaxValue;
            int num_objects = sr.World.Objects.Count;
            double d = MathHelper.Epsilon;

            for (int j = 0; j < num_objects; j++)
                if (sr.World.Objects[j].ShadowHit(ray, ref t) && t > d)
                    return (true);
            return (false);
        }
    }
}
