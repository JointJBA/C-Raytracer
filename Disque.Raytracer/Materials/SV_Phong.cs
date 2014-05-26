using System;
using System.Collections.Generic;
using System.Text;
using Disque.Raytracer.BRDFs;
using Disque.Math;
using Disque.Raytracer.Textures;
using Disque.Raytracer.Samplers;

namespace Disque.Raytracer.Materials
{
    public class SV_Phong : Material
    {
        protected SV_Lambertian ambient = new SV_Lambertian();
        protected SV_Lambertian diffuse = new SV_Lambertian();
        protected SV_GlossySpecular specular = new SV_GlossySpecular();

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
                        {
                            L += (diffuse.F(sr, wo, wi) + specular.F(sr, wo, wi)) * sr.World.Lights[j].L(sr) * ndotwi;
                        }
                    }
                    else
                    {
                        L += (diffuse.F(sr, wo, wi) + specular.F(sr, wo, wi)) * sr.World.Lights[j].L(sr) * ndotwi;
                    }
                }
            }
            return (L);
        }

        public override Vector3 Area_Light_Shade(ShadeRec sr)
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
                            L += (diffuse.F(sr, wo, wi)
                                      + specular.F(sr, wo, wi)) * sr.World.Lights[j].L(sr) * ndotwi * sr.World.Lights[j].G(sr) / sr.World.Lights[j].PDF(sr);
                    }
                    else
                    {
                        L += (diffuse.F(sr, wo, wi)
                                      + specular.F(sr, wo, wi)) * sr.World.Lights[j].L(sr) * ndotwi * sr.World.Lights[j].G(sr) / sr.World.Lights[j].PDF(sr);
                    }
                }
            }
            return (L);
        }

        public void SetAmbientRC(float k)
        {
            ambient.ReflectionCoefficient = k;
        }

        public void SetDiffuseRC(float k)
        {
            diffuse.ReflectionCoefficient = k;
        }

        public void SetSpecularRC(float k)
        {
            specular.ReflectionCoefficient = k;
        }

        public void SetCD(Texture c)
        {
            ambient.Texture = c;
            diffuse.Texture = c;
        }

        public void SetSpecularColor(Texture c)
        {
            specular.SpecularTexture = c;
        }

        public void SetExp(float e)
        {
            specular.Exponent = e;
        }

        public override void SetSampler(Sampler sampler)
        {
            ambient.SetSampler(sampler);
            diffuse.SetSampler(sampler);
            specular.SetSampler(sampler);
            base.SetSampler(sampler);
        }
    }
}
