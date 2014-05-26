using System;
using System.Collections.Generic;
using System.Text;
using Disque.Math;

namespace Disque.Raytracer.BTDFs
{
    public class FresnelTransmitter : BTDF
    {
        public float eta_in = 0, eta_out = 1;

        public bool TIR(ShadeRec sr)
        {
            Vector3 wo = (-sr.Ray.Direction);
            float cos_thetai = Vector3.Dot(sr.Normal, wo);
            float eta = eta_in / eta_out;
            if (cos_thetai < 0.0)
                eta = 1.0f / eta;
            return (1.0f - (1.0f - cos_thetai * cos_thetai) / (eta * eta) < 0.0f);
        }

        public override Vector3 Sample_F(ShadeRec sr, Vector3 wo, ref Vector3 wt)
        {
            Vector3 n = (sr.Normal);
            float cos_thetai = Vector3.Dot(n, wo);

            float eta = eta_in / eta_out;	

            if (cos_thetai < 0.0)
            {  
                cos_thetai = -cos_thetai;
                n = -n;
                eta = 1.0f / eta;
            }

            float temp = 1.0f - (1.0f - cos_thetai * cos_thetai) / (eta * eta);
            float cos_theta2 = MathHelper.Sqrt(temp);
            wt = -wo / eta - (cos_theta2 - cos_thetai / eta) * n;
            return (fresnel(sr) / (eta * eta) * Colors.White / Vector3.AbsDot(sr.Normal, wt));
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
            return (1 - kr);
        }

    }
}
