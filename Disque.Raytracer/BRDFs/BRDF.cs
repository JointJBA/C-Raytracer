using System;
using System.Collections.Generic;
using System.Text;
using Disque.Raytracer.Samplers;
using Disque.Math;

namespace Disque.Raytracer.BRDFs
{
    public class BRDF
    {
        protected Sampler Sampler;
        public virtual Vector3 F(ShadeRec sr, Vector3 wo, Vector3 wi) { return Colors.Black; }
        public virtual Vector3 Sample_F(ShadeRec sr, Vector3 wo, ref Vector3 wi) { return Colors.Black; }
        public virtual Vector3 Sample_F(ShadeRec sr, Vector3 wo, ref Vector3 wi, ref float pdf) { return Colors.Black; }
        public virtual Vector3 RHO(ShadeRec sr, Vector3 wo) { return Colors.Black; }
        public virtual void SetSampler(Sampler s)
        {
            Sampler = s;
            Sampler.MapSamplesToHemisphere(1);
        }
    }
}
