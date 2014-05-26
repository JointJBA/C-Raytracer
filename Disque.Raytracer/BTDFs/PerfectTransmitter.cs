using System;
using System.Collections.Generic;
using System.Text;
using Disque.Math;

namespace Disque.Raytracer.BTDFs
{
    public class PerfectTransmitter : BTDF
    {
        public float IndexOfRefraction = 1;
        public float TransmissionCoefficient = 0;

        public bool TIR(ShadeRec sr)
        {
            Vector3 wo = (-sr.Ray.Direction);
            float cos_thetai = Vector3.Dot(sr.Normal, wo);
            float eta = IndexOfRefraction;
            if (cos_thetai < 0.0f)
                eta = 1.0f / eta;
            return (1.0f - (1.0f - cos_thetai * cos_thetai) / (eta * eta) < 0.0f);
        }

        public override Vector3 F(ShadeRec sr, Vector3 wo, Vector3 wi)
        {
            return Colors.Black;
        }

        public override Vector3 Sample_F(ShadeRec sr, Vector3 wo, ref Vector3 wt)
        {
            Vector3 n = (sr.Normal);
            float cos_thetai = Vector3.Dot(n, wo);
            float eta = IndexOfRefraction;
            if (cos_thetai < 0.0)
            {
                cos_thetai = -cos_thetai;
                n = -n;  					// reverse direction of normal
                eta = 1.0f / eta; 			// invert ior 
            }
            float temp = 1.0f - (1.0f - cos_thetai * cos_thetai) / (eta * eta);
            float cos_theta2 = MathHelper.Sqrt(temp);
            wt = -wo / eta - (cos_theta2 - cos_thetai / eta) * n;

            return (TransmissionCoefficient / (eta * eta) * Vector3.One / MathHelper.Abs(Vector3.Dot(sr.Normal, wt)));
        }

        public override Vector3 RHO(ShadeRec sr, Vector3 wo)
        {
            return Colors.Black;
        }
    }
}
