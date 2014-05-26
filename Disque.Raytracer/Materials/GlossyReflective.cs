using System;
using System.Collections.Generic;
using System.Text;
using Disque.Raytracer.BRDFs;
using Disque.Math;
using Disque.Raytracer.Samplers;

namespace Disque.Raytracer.Materials
{
    public class GlossyReflective : Phong
    {
        GlossySpecular glossy_specular = new GlossySpecular();

        public void SetKR(float k)
        {
            glossy_specular.ReflectionCoefficient = k;
        }

        public void SetCR(Vector3 c)
        {
            glossy_specular.SpecularColor = c;
        }

        public override Vector3 Shade(ShadeRec sr)
        {
            Vector3 wo = -sr.Ray.Direction;
            Vector3 L = ambient.RHO(sr, wo) * sr.World.AmbientLight.L(sr);
            int num_lights = sr.World.Lights.Count;
            for (int j = 0; j < num_lights; j++)
            {
                Vector3 wi = sr.World.Lights[j].GetDirection(sr);
                float ndotwi = Vector3.Dot(sr.Normal, wi);
                if (ndotwi > 0.0)
                {
                    bool in_shadow = false;
                    if (sr.World.Lights[j].Shadows)
                    {
                        Ray shadowRay = new Ray(sr.HitPoint, wi);
                        in_shadow = sr.World.Lights[j].InShadow(shadowRay, sr);
                    }

                    if (!in_shadow)
                        L += (diffuse.F(sr, wo, wi) + specular.F(sr, wo, wi)) * sr.World.Lights[j].L(sr) * ndotwi;
                }
            }
            return (L);
        }

        public override Vector3 Area_Light_Shade(ShadeRec sr)
        {
            Vector3 L = (base.Area_Light_Shade(sr));
            Vector3 wo = (-sr.Ray.Direction);
            Vector3 wi = Vector3.Zero;
            float pdf = 0;
            Vector3 fr = (glossy_specular.Sample_F(sr, wo, ref wi, ref pdf));
            Ray reflected_ray = new Ray(sr.HitPoint, wi);
            L += fr * sr.World.Tracer.TraceRay(reflected_ray, sr.Depth + 1) * (Vector3.Dot(sr.Normal, wi)) / pdf;
            return (L);
        }

        public void SetExponent(float e)
        {
            glossy_specular.Exponent = e;
        }

        public void SetSamples(int ns, float exp)
        {
            glossy_specular.SetSamples(ns, exp);
        }

        public void SetSampler(Sampler s, float exp)
        {
            glossy_specular.SetSampler(s, exp);
        }

        public override void SetSampler(Sampler sampler)
        {
            glossy_specular.SetSampler(sampler, glossy_specular.Exponent);
            base.SetSampler(sampler);
        }
    }
}
