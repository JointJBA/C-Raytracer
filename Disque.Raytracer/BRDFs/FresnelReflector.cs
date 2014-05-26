using System;
using System.Collections.Generic;
using System.Text;
using Disque.Math;
using Disque.Raytracer.Textures;

namespace Disque.Raytracer.BRDFs
{
    public class FresnelReflector : BRDF
    {
        public float eta_in = 0, eta_out = 0;
        public Vector3 Color;

        public override Vector3 Sample_F(ShadeRec sr, Vector3 wo, ref Vector3 wr)
        {
            float ndotwo = Vector3.Dot(sr.Normal, wo);
            wr = -wo + 2.0f * sr.Normal * ndotwo;
            return (fresnel(sr) * Colors.White / Vector3.AbsDot(sr.Normal, wr));
        }

        public float fresnel(ShadeRec sr)
        {
            Vector3 normal = (sr.Normal);
            float ndotd = Vector3.Dot(-normal, sr.Ray.Direction);
            float eta;
            if (ndotd < 0.0)
            {
                normal = -normal;
                eta = eta_out / eta_in;
            }
            else
                eta = eta_in / eta_out;

            float cos_theta_i = Vector3.Dot(-normal, sr.Ray.Direction);
            float temp = 1.0f - (1.0f - cos_theta_i * cos_theta_i) / (eta * eta);
            float cos_theta_t = MathHelper.Sqrt(1.0f - (1.0f - cos_theta_i * cos_theta_i) / (eta * eta));
            float r_parallel = (eta * cos_theta_i - cos_theta_t) / (eta * cos_theta_i + cos_theta_t);
            float r_perpendicular = (cos_theta_i - eta * cos_theta_t) / (cos_theta_i + eta * cos_theta_t);
            float kr = 0.5f * (r_parallel * r_parallel + r_perpendicular * r_perpendicular);
            return (kr);
        }
    }
}
