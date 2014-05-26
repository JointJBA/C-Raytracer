using System;
using System.Collections.Generic;
using System.Text;
using Disque.Math;
using Disque.Raytracer.Samplers;

namespace Disque.Raytracer.BRDFs
{
    public class GlossySpecular : BRDF
    {
        public float Exponent = 1;
        public Vector3 SpecularColor = Colors.White;
        public float ReflectionCoefficient = 0;

        public override Vector3 F(ShadeRec sr, Vector3 wo, Vector3 wi)
        {
            Vector3 L = Vector3.Zero;
            float ndotwi = Vector3.Dot(sr.Normal, wi);
            Vector3 r = -wi + 2.0f * sr.Normal * ndotwi;
            float rdotwo = Vector3.Dot(r, wo);
            if (rdotwo > 0.0)
                L = ReflectionCoefficient * SpecularColor * MathHelper.Pow(rdotwo, Exponent);
            return (L);
        }

        public override Vector3 Sample_F(ShadeRec sr, Vector3 wo, ref Vector3 wi, ref float pdf)
        {
            float ndotwo = Vector3.Dot(sr.Normal, wo);
            Vector3 r = -wo + 2.0f * sr.Normal * ndotwo;     // direction of mirror reflection
            Vector3 w = r;
            Vector3 u = Vector3.Cross(new Vector3(0.00424f, 1, 0.00764f), w);
            u.Normalize();
            Vector3 v = Vector3.Cross(u, w);
            Vector3 sp = Sampler.SampleHemisphere();
            wi = sp.X * u + sp.Y * v + sp.Z * w;			// reflected ray direction
            if (Vector3.Dot(sr.Normal, wi) < 0.0) 						// reflected ray is below tangent plane
                wi = -sp.X * u - sp.Y * v + sp.Z * w;
            float phong_lobe = MathHelper.Pow(Vector3.Dot(r, wi), Exponent);
            pdf = phong_lobe * (Vector3.Dot(sr.Normal, wi));
            return (ReflectionCoefficient * SpecularColor * phong_lobe);
        }

        public override Vector3 Sample_F(ShadeRec sr, Vector3 wo, ref Vector3 wi)
        {
            throw new NotImplementedException();
        }

        public override Vector3 RHO(ShadeRec sr, Vector3 wo)
        {
            return new Vector3(0);
        }

        public void SetSampler(Sampler sp, float exp)
        {
            Sampler = sp;
            Sampler.MapSamplesToHemisphere(exp);
        }

        public void SetSamples(int num_samples, float exp)
        {
            Sampler = new MultiJittered(num_samples);
            Sampler.MapSamplesToHemisphere(exp);
        }
    }
}
