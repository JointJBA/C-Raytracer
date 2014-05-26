using System;
using System.Collections.Generic;
using System.Text;
using Disque.Math;

namespace Disque.Raytracer.Lights
{
    public class Directional : Light
    {
        public float RadianceScale = 1;
        public Vector3 Color = Colors.White, Direction = Vector3.Up;

        public override Vector3 GetDirection(ShadeRec sr)
        {
            return Direction;
        }

        public override Light Clone()
        {
            Directional s = new Directional();
            s.RadianceScale = RadianceScale;
            s.Color = Color;
            s.Direction = Direction;
            return s;
        }

        public override Vector3 L(ShadeRec sr)
        {
            return RadianceScale * Color;
        }

        public override bool InShadow(Ray shadowRay, ShadeRec sr)
        {
            float t = float.MaxValue;
            int num_objects = sr.World.Objects.Count;
            float d = MathHelper.Epsilon;
            for (int j = 0; j < num_objects; j++)
            {
                if (sr.World.Objects[j].ShadowHit(shadowRay, ref t) && t > d)
                {
                    return (true);
                }
            }
            return (false);
        }

        public Directional()
        {
            Color = Vector3.One;
            RadianceScale = 1.0f;
            Direction = Vector3.Up;
        }
    }
}
