using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Disque.Raytracer.BRDFs;
using Disque.Math;

namespace Disque.Raytracer.Materials
{
    public class SV_GlossyReflector : SV_Phong
    {
        SV_GlossySpecular glossy_specular_brdf = new SV_GlossySpecular();

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

                if (ndotwi > 0.0)
                {
                    bool in_shadow = false;

                    if (sr.World.Lights[j].Shadows)
                    {
                        Ray shadowRay = new Ray(sr.HitPoint, wi);
                        in_shadow = sr.World.Lights[j].InShadow(shadowRay, sr);
                    }
                    if (!in_shadow)
                        L += (diffuse.F(sr, wo, wi)
                                  + specular.F(sr, wo, wi)) * sr.World.Lights[j].L(sr) * ndotwi;
                }
            }
            return (L);
        }

        public override Vector3 Area_Light_Shade(ShadeRec sr)
        {
            Vector3 L = (base.Area_Light_Shade(sr));  // direct illumination
            Vector3 wo = -sr.Ray.Direction;
            Vector3 wi = Vector3.Zero;
            float pdf = 0.0f;
            Vector3 fr = glossy_specular_brdf.Sample_F(sr, wo, ref wi, ref pdf);
            Ray reflected_ray = new Ray(sr.HitPoint, wi);
            L += fr * sr.World.Tracer.TraceRay(reflected_ray, sr.Depth + 1) * Vector3.Dot(sr.Normal, wi) / pdf;
            return (L);
        }
    }
}
