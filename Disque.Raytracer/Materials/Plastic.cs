using System;
using System.Collections.Generic;
using System.Text;
using Disque.Raytracer.BRDFs;
using Disque.Math;
using Disque.Raytracer.Samplers;

namespace Disque.Raytracer.Materials
{
    public class Plastic : Material
    {
        public BRDF ambient, diffuse, specular;

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
                    if (Shadows)
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
                    else
                    {
                        L += (diffuse.F(sr, wo, wi) + specular.F(sr, wo, wi)) * sr.World.Lights[j].L(sr) * ndotwi;
                    }
                }
            }
            return (L);
        }

        public override void SetSampler(Sampler sampler)
        {
            ambient.SetSampler(sampler);
            diffuse.SetSampler(sampler);
            specular.SetSampler(sampler);
        }
    }
}
