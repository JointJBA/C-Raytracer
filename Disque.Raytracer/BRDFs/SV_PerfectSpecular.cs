using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Disque.Raytracer.Textures;
using Disque.Math;

namespace Disque.Raytracer.BRDFs
{
    public class SV_PerfectSpecular : BRDF
    {
        float kr;
        Texture cr;

        public void SetReflectionCoefficient(float k)
        {
            kr = k;
        }

        public void SetTexture(Texture c)
        {
            cr = c;
        }

        public override Vector3 F(ShadeRec sr, Vector3 wo, Vector3 wi)
        {
            return Vector3.Zero;
        }

        public override Vector3 Sample_F(ShadeRec sr, Vector3 wo, ref Vector3 wi)
        {
            float ndotwo = Vector3.Dot(sr.Normal, wo);
            wi = -wo + 2.0f * sr.Normal * ndotwo;
            return (kr * cr.GetColor(sr) / Vector3.AbsDot(sr.Normal, wi));
        }

        public override Vector3 Sample_F(ShadeRec sr, Vector3 wo, ref Vector3 wi, ref float pdf)
        {
            float ndotwo = Vector3.Dot(sr.Normal, wo);
            wi = -wo + 2.0f * sr.Normal * ndotwo;
            pdf = Vector3.AbsDot(sr.Normal, wi);
            return kr * cr.GetColor(sr);
        }

        public override Vector3 RHO(ShadeRec sr, Vector3 wo)
        {
            return Vector3.Zero;
        }
    }
}
