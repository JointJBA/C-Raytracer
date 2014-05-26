using System;
using System.Collections.Generic;
using System.Text;
using Disque.Raytracer.BTDFs;
using Disque.Raytracer.BRDFs;
using Disque.Math;
using Disque.Raytracer.Samplers;

namespace Disque.Raytracer.Materials
{
    public class Dielectric : Material
    {
        Vector3 cf_in, cf_out;
        FresnelReflector fresnel_brdf = new FresnelReflector();
        FresnelTransmitter fresnel_btdf = new FresnelTransmitter();

        public void Set_eta_in(float i)
        {
            fresnel_brdf.eta_in = fresnel_btdf.eta_in = i;
        }

        public void Set_eta_out(float i)
        {
            fresnel_brdf.eta_out = fresnel_btdf.eta_out = i;
        }

        public void Set_cf_in(Vector3 i)
        {
            cf_in = i;
        }

        public void Set_cf_out(Vector3 i)
        {
            cf_out = i;
        }

        public override void SetSampler(Sampler sampler)
        {
            fresnel_brdf.SetSampler(sampler);
        }

        public override Vector3 Shade(ShadeRec sr)
        {
            Vector3 L = (base.Shade(sr));
            Vector3 wi = Vector3.Zero;
            Vector3 wo = (-sr.Ray.Direction);
            Vector3 fr = fresnel_brdf.Sample_F(sr, wo, ref wi);
            Ray reflected_ray = new Ray(sr.HitPoint, wi);
            float t = 0;
            Vector3 Lr, Lt;
            float ndotwi = Vector3.Dot(sr.Normal, wi);
            if (fresnel_btdf.TIR(sr))
            {								// total internal reflection
                if (ndotwi < 0.0)
                {
                    Lr = sr.World.Tracer.TraceRay(reflected_ray, ref t, sr.Depth + 1);
                    L += cf_in.Pow(t) * Lr;
                }
                else
                {
                    Lr = sr.World.Tracer.TraceRay(reflected_ray, ref t, sr.Depth + 1);   // kr = 1  
                    L += cf_out.Pow(t) * Lr;   					// outside filter color
                }
            }
            else
            {
                Vector3 wt = Vector3.Zero;
                Vector3 ft = fresnel_btdf.Sample_F(sr, wo, ref wt);  	// computes wt
                Ray transmitted_ray = new Ray(sr.HitPoint, wt);
                float ndotwt = Vector3.Dot(sr.Normal, wt);
                if (ndotwi < 0.0)
                {
                    Lr = fr * sr.World.Tracer.TraceRay(reflected_ray, ref t, sr.Depth + 1) * MathHelper.Abs(ndotwi);
                    L += cf_in.Pow(t) * Lr;
                    Lt = ft * sr.World.Tracer.TraceRay(transmitted_ray, ref t, sr.Depth + 1) * MathHelper.Abs(ndotwt);
                    L += cf_out.Pow(t) * Lt;
                }
                else
                {
                    Lr = fr * sr.World.Tracer.TraceRay(reflected_ray, ref t, sr.Depth + 1) * MathHelper.Abs(ndotwi);
                    L += cf_out.Pow(t) * Lr;						// outside filter color
                    Lt = ft * sr.World.Tracer.TraceRay(transmitted_ray, ref t, sr.Depth + 1) * MathHelper.Abs(ndotwt);
                    L += cf_in.Pow(t) * Lt; 						// inside filter color
                }
            }
            return (L);
        }
    }
}
