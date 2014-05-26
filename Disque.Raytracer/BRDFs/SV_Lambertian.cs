using System;
using System.Collections.Generic;
using System.Text;
using Disque.Math;
using Disque.Raytracer.Textures;

namespace Disque.Raytracer.BRDFs
{
    public class SV_Lambertian : BRDF
    {
        public Texture Texture;
        public float ReflectionCoefficient;

        public override Vector3 F(ShadeRec sr, Vector3 wo, Vector3 wi)
        {
            return ReflectionCoefficient * Texture.GetColor(sr) * MathHelper.InvPI;
        }

        public override Vector3 Sample_F(ShadeRec sr, Vector3 wo, ref Vector3 wi)
        {
            Vector3 w = sr.Normal;
            Vector3 v = Vector3.Cross(new Vector3(0.0034f, 1, 0.0071f), w);
            v.Normalize();
            Vector3 u = Vector3.Cross(v, w);
            Vector3 sp = Sampler.SampleHemisphere();
            wi = sp.X * u + sp.Y * v + sp.Z * w;
            wi.Normalize();
            return (ReflectionCoefficient * Texture.GetColor(sr) * MathHelper.InvPI);
        }

        public override Vector3 RHO(ShadeRec sr, Vector3 wo)
        {
            return ReflectionCoefficient * Texture.GetColor(sr);
        }
    }
}
