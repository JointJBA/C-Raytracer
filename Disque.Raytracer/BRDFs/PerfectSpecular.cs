using System;
using System.Collections.Generic;
using System.Text;
using Disque.Math;

namespace Disque.Raytracer.BRDFs
{
    public class PerfectSpecular : BRDF
    {
        public Vector3 Color = Colors.White;
        public float ReflectionCoefficient = 0;

        public override Vector3 F(ShadeRec sr, Vector3 wo, Vector3 wi)
        {
            return Colors.Black;
        }

        public override Vector3 Sample_F(ShadeRec sr, Vector3 wo, ref Vector3 wi)
        {
            float ndotwo = Vector3.Dot(sr.Normal, wo);
            wi = -wo + 2.0f * sr.Normal * ndotwo;
            return (ReflectionCoefficient * Color / MathHelper.Abs(Vector3.Dot(sr.Normal, wi)));
        }

        public override Vector3 Sample_F(ShadeRec sr, Vector3 wo, ref Vector3 wi, ref float pdf)
        {
            float ndotwo = Vector3.Dot(sr.Normal, wo);
            wi = -wo + 2.0f * sr.Normal * ndotwo;
            pdf = MathHelper.Abs(Vector3.Dot(sr.Normal, wi));
            return (ReflectionCoefficient * Color);  
        }

        public override Vector3 RHO(ShadeRec sr, Vector3 wo)
        {
            return Colors.Black;
        }
    }
}
