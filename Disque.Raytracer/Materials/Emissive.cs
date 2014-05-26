using System;
using System.Collections.Generic;
using System.Text;
using Disque.Math;

namespace Disque.Raytracer.Materials
{
    public class Emissive : Material
    {
        public float Radiance;
        public Vector3 Color;

        public override Vector3 Get_Le(ShadeRec sr)
        {
           return  Radiance * Color;
        }

        public override Vector3 Shade(ShadeRec sr)
        {
            if (Vector3.Dot(-sr.Normal, sr.Ray.Direction) > 0.0f)
                return (Radiance * Color);
            else
                return (Vector3.Zero);
        }

        public override Vector3 Area_Light_Shade(ShadeRec sr)
        {
            if (Vector3.Dot(-sr.Normal, sr.Ray.Direction) > 0.0f)		//here may be ConcaveSphere not support
                return (Radiance * Color);
            else
                return (Vector3.Zero);
        }

        public override Vector3 Path_Shade(ShadeRec sr)
        {
            if (Vector3.Dot(-sr.Normal, sr.Ray.Direction) > 0.0f)
                return (Radiance * Color);
            else
                return Vector3.Zero;

        }

        public override Vector3 Global_Shade(ShadeRec sr)
        {
            if (sr.Depth == 1)
                return (Vector3.Zero);
            if (Vector3.Dot(-sr.Normal, sr.Ray.Direction) > 0.0f)
                return (Radiance * Color);
            else
                return (Vector3.Zero);
        }
    }
}
