using System;
using System.Collections.Generic;
using System.Text;
using Disque.Math;

namespace Disque.Raytracer.BRDFs
{
    public class Lambertian : BRDF
    {
        public float ReflectionCoefficient = 0;
        public Vector3 Color = new Vector3(0);

        public override Vector3 F(ShadeRec sr, Vector3 wo, Vector3 wi)
        {
            return ReflectionCoefficient * Color * MathHelper.InvPI;
        }

        public override Vector3 Sample_F(ShadeRec sr, Vector3 wo, ref Vector3 wi)
        {
            return Vector3.Zero;
        }

        public override Vector3 RHO(ShadeRec sr, Vector3 wo)
        {
            return ReflectionCoefficient * Color;
        }

        public override Vector3 Sample_F(ShadeRec sr, Vector3 wo, ref Vector3 wi, ref float pdf)
        {
            Vector3 w = sr.Normal;
            Vector3 v = Vector3.Cross(new Vector3(0.0034f, 1, 0.0071f), w);
            v.Normalize();
            Vector3 u = Vector3.Cross(v, w);
            Vector3 sp = Sampler.SampleHemisphere();
            wi = sp.X * u + sp.Y * v + sp.Z * w;
            wi.Normalize();
            pdf = Vector3.Dot(sr.Normal, wi) * MathHelper.InvPI;
            return (ReflectionCoefficient * Color * MathHelper.InvPI);
        }
    }
}
