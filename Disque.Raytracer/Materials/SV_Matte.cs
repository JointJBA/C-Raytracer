using System;
using System.Collections.Generic;
using System.Text;
using Disque.Raytracer.BRDFs;
using Disque.Math;
using Disque.Raytracer.Lights;
using Disque.Raytracer.Textures;
using Disque.Raytracer.Samplers;

namespace Disque.Raytracer.Materials
{
    public class SV_Matte : Material
    {
        SV_Lambertian ambient = new SV_Lambertian();
        SV_Lambertian diffuse = new SV_Lambertian();

        public SV_Matte(float ka, float kd, Texture texture)
        {
            SetKA(ka);
            SetKD(kd);
            SetCD(texture);
        }

        public void SetKA(float ka)
        {
            ambient.ReflectionCoefficient = ka;
        }

        public void SetKD(float kd)
        {
            diffuse.ReflectionCoefficient = kd;
        }

        public void SetCD(Texture c)
        {
            ambient.Texture = diffuse.Texture = c;
        }

        public override Vector3 Shade(ShadeRec sr)
        {
            Vector3 wo = -sr.Ray.Direction;
            Vector3 L = ambient.RHO(sr, wo) * sr.World.AmbientLight.L(sr);
            int num_lights = sr.World.Lights.Count;
            for (int j = 0; j < num_lights; j++)
            {
                Light light_ptr = sr.World.Lights[j];
                Vector3 wi = light_ptr.GetDirection(sr);
                wi.Normalize();
                float ndotwi = Vector3.Dot(sr.Normal, wi);
                float ndotwo = Vector3.Dot(sr.Normal, wo);
                if (ndotwi > 0.0 && ndotwo > 0.0)
                {
                    bool in_shadow = false;
                    if (sr.World.Lights[j].Shadows)
                    {
                        Ray shadow_ray = new Ray(sr.HitPoint, wi);		//hitPoint ?
                        in_shadow = light_ptr.InShadow(shadow_ray, sr);	//.object ?
                    }
                    if (!in_shadow)
                        L += diffuse.F(sr, wo, wi) * light_ptr.L(sr) * light_ptr.G(sr) * ndotwi;
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
                Light light_ptr = sr.World.Lights[j];
                Vector3 wi = light_ptr.GetDirection(sr);	//compute_direction ?
                wi.Normalize();
                float ndotwi = Vector3.Dot(sr.Normal, wi);
                float ndotwo = Vector3.Dot(sr.Normal, wo);
                if (ndotwi > 0.0 && ndotwo > 0.0)
                {
                    bool in_shadow = false;

                    if (sr.World.Lights[j].Shadows)
                    {
                        Ray shadow_ray = new Ray(sr.HitPoint, wi);
                        in_shadow = light_ptr.InShadow(shadow_ray, sr);
                    }
                    if (!in_shadow)
                        L += diffuse.F(sr, wo, wi) * light_ptr.L(sr) * light_ptr.G(sr) * ndotwi / sr.World.Lights[j].PDF(sr);
                }
            }
            return (L);
        }

        public override Vector3 Path_Shade(ShadeRec sr)
        {
            Vector3 wo = -sr.Ray.Direction;
            Vector3 wi = Vector3.Zero;
            Vector3 f = diffuse.Sample_F(sr, wo, ref wi);
            float ndotwi = Vector3.Dot(sr.Normal, wi);
            Ray reflected_ray = new Ray(sr.HitPoint, wi);
            return (f * sr.World.Tracer.TraceRay(reflected_ray, sr.Depth + 1) * ndotwi);
        }

        public override Vector3 Global_Shade(ShadeRec sr)
        {
            Vector3 L = Vector3.Zero;
            if (sr.Depth == 0)
                L = Area_Light_Shade(sr);
            Vector3 wi = Vector3.Zero;
            Vector3 wo = -sr.Ray.Direction;
            Vector3 f = diffuse.Sample_F(sr, wo, ref wi);
            float ndotwi = Vector3.Dot(sr.Normal, wi);
            Ray reflected_ray = new Ray(sr.HitPoint, wi);
            L += f * sr.World.Tracer.TraceRay(reflected_ray, sr.Depth + 1) * ndotwi;
            return (L);
        }

        public override void SetSampler(Sampler sampler)
        {
            ambient.SetSampler(sampler);
            diffuse.SetSampler(sampler);
        }
    }
}
