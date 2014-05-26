using System;
using System.Collections.Generic;
using System.Text;
using Disque.Math;

namespace Disque.Raytracer.Lights
{
    public class Ambient : Light
    {
        public float RadianceScale = 1;
        public Vector3 Color = Colors.White;

        public override Vector3 GetDirection(ShadeRec sr)
        {
            return new Vector3(0);
        }

        public override Light Clone()
        {
            Ambient am = new Ambient();
            am.Color = Color;
            am.RadianceScale = RadianceScale;
            return am;
        }

        public override Vector3 L(ShadeRec sr)
        {
            return RadianceScale * Color;
        }

        public override bool InShadow(Ray shadowRay, ShadeRec sr)
        {
            return false;
        }
    }
}
