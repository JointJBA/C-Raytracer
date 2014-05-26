using System;
using System.Collections.Generic;
using System.Text;
using Disque.Math;
using Disque.Raytracer.Samplers;

namespace Disque.Raytracer.Materials
{
    public class Material
    {
        public bool Shadows = true;

        public virtual Vector3 Get_Le(ShadeRec sr)
        {
            return Colors.White;
        }

        public virtual Vector3 Shade(ShadeRec sr)
        {
            return new Vector3(0, 0, 0);
        }

        public virtual Vector3 Area_Light_Shade(ShadeRec sr)
        {
            return Shade(sr);
        }

        public virtual Vector3 Path_Shade(ShadeRec sr)
        {
            return Shade(sr);
        }

        public virtual Vector3 Global_Shade(ShadeRec sr)
        {
            return Shade(sr);
        }

        public virtual void SetSampler(Sampler sampler)
        {
        }

        public static Material Glass
        {
            get
            {
                Transparent glass = new Transparent();
                glass.SetSpecularRC(0.5f);
                glass.SetExp(2000);
                glass.SetIndexOfRefraction(1.5f);
                glass.SetReflectiveRC(0.1f);
                glass.SetTransmissionCoefficient(0.9f);
                return glass;
            }
        }

        public static Material ColorMaterial(Vector3 color)
        {
            return new Matte(0.8f, 0.8f, color);
        }
    }
}
