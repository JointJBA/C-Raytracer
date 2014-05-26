using System;
using System.Collections.Generic;
using System.Text;
using Disque.Raytracer.BRDFs;
using Disque.Math;
using Disque.Raytracer.Lights;
using Disque.Raytracer.Samplers;

namespace Disque.Raytracer.Materials
{
    public class Matte : Material
    {
        Lambertian ambient = new Lambertian();
        Lambertian diffuse = new Lambertian();

        public Matte(float ka, float kd, Vector3 color)
        {
            SetKA(ka);
            SetKD(kd);
            SetCD(color);
        }

        public void SetKA(float ka)
        {
            ambient.ReflectionCoefficient = ka;
        }

        public void SetKD(float kd)
        {
            diffuse.ReflectionCoefficient = kd;
        }

        public void SetCD(Vector3 c)
        {
            ambient.Color = diffuse.Color = c;
        }

        public override void SetSampler(Sampler sampler)
        {
            ambient.SetSampler(sampler);
            diffuse.SetSampler(sampler);
            base.SetSampler(sampler);
        }

        public override Vector3 Get_Le(ShadeRec sr)
        {
            return Vector3.One;
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
                if (ndotwi > 0)
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
                            L += diffuse.F(sr, wo, wi) * sr.World.Lights[j].L(sr) * ndotwi;
                        }
                    }
                    else
                    {
                        L += diffuse.F(sr, wo, wi) * sr.World.Lights[j].L(sr) * ndotwi;
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
                            Ray shadow_ray = new Ray(sr.HitPoint, wi);
                            in_shadow = sr.World.Lights[j].InShadow(shadow_ray, sr);
                        }

                        if (!in_shadow)
                            L += diffuse.F(sr, wo, wi) * sr.World.Lights[j].L(sr) * sr.World.Lights[j].G(sr) * ndotwi / sr.World.Lights[j].PDF(sr);
                    }
                    else
                    {
                        L += diffuse.F(sr, wo, wi) * sr.World.Lights[j].L(sr) * sr.World.Lights[j].G(sr) * ndotwi / sr.World.Lights[j].PDF(sr);
                    }
                }
            }
            return (L);
        }

        public override Vector3 Path_Shade(ShadeRec sr)
        {
            Vector3 wi = Vector3.Zero;
            Vector3 wo = -sr.Ray.Direction;
            float pdf = 0;
            Vector3 f = diffuse.Sample_F(sr, wo, ref wi, ref pdf);
            float ndotwi = Vector3.Dot(sr.Normal, wi);
            Ray reflected_ray = new Ray(sr.HitPoint, wi);
            Vector3 s = sr.World.Tracer.TraceRay(reflected_ray, sr.Depth + 1);
            return (f * s * ndotwi / pdf);
        }

        public override Vector3 Global_Shade(ShadeRec sr)
        {
            Vector3 L = Vector3.Zero;
            if (sr.Depth == 0)
                L = Area_Light_Shade(sr);
            Vector3 wi = Vector3.Zero;
            Vector3 wo = -sr.Ray.Direction;
            float pdf = 0;
            Vector3 f = diffuse.Sample_F(sr, wo, ref wi, ref pdf);
            float ndotwi = Vector3.Dot(sr.Normal, wi);
            Ray reflected_ray = new Ray(sr.HitPoint, wi);
            L += f * sr.World.Tracer.TraceRay(reflected_ray, sr.Depth + 1) * ndotwi / pdf;
            return (L);
        }
    }
}
