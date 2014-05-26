using System;
using System.Collections.Generic;
using System.Text;
using Disque.Math;
using Disque.Raytracer.BRDFs;
using Disque.Raytracer.BTDFs;
using Disque.Raytracer.Samplers;

namespace Disque.Raytracer.Materials
{
    /// <summary>
    /// A transparent material.
    /// </summary>
    public class Transparent : Phong
    {
        PerfectSpecular reflective = new PerfectSpecular();
        PerfectTransmitter specular_btdf = new PerfectTransmitter();

        /// <summary>
        /// Sets the index of refraction of the material.
        /// </summary>
        /// <param name="i">Index of refraction.</param>
        public void SetIndexOfRefraction(float i)
        {
            specular_btdf.IndexOfRefraction = i;
        }

        public void SetReflectiveRC(float kr)
        {
            reflective.ReflectionCoefficient = kr;
        }

        public void SetReflectiveColor(Vector3 c)
        {
            reflective.Color = c;
        }

        public void SetTransmissionCoefficient(float kt)
        {
            specular_btdf.TransmissionCoefficient = kt;
        }

        public override Vector3 Shade(ShadeRec sr)
        {
            Vector3 L = base.Shade(sr);
            Vector3 wo = -sr.Ray.Direction;
            Vector3 wi = Vector3.Zero;
            Vector3 fr = reflective.Sample_F(sr, wo, ref wi);
            Ray reflected_ray = new Ray(sr.HitPoint, wi);
            if (specular_btdf.TIR(sr))
            {
                L += sr.World.Tracer.TraceRay(reflected_ray, sr.Depth + 1);
            }
            else
            {
                Vector3 wt = Vector3.Zero;
                Vector3 ft = specular_btdf.Sample_F(sr, wo, ref wt);
                Ray transmitted_ray = new Ray(sr.HitPoint, wt);
                L += fr * sr.World.Tracer.TraceRay(reflected_ray, sr.Depth + 1) * Vector3.AbsDot(sr.Normal, wi);
                L += ft * sr.World.Tracer.TraceRay(transmitted_ray, sr.Depth + 1) * Vector3.AbsDot(sr.Normal, wt);		//this is very important for transparent rendering
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
                    bool in_shadow = false;
                    if (sr.World.Lights[j].Shadows)
                    {
                        Ray shadowRay = new Ray(sr.HitPoint, wi);
                        in_shadow = sr.World.Lights[j].InShadow(shadowRay, sr);
                    }
                    if (!in_shadow)
                        L += (diffuse.F(sr, wo, wi) + specular_btdf.F(sr, wo, wi)) * sr.World.Lights[j].L(sr) * ndotwi * sr.World.Lights[j].G(sr) / sr.World.Lights[j].PDF(sr);
                }
            }
            return (L);
        }

        public override void SetSampler(Sampler sampler)
        {
            reflective.SetSampler(sampler);
            base.SetSampler(sampler);
        }
    }
}
