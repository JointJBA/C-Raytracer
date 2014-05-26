using System;
using System.Collections.Generic;
using System.Text;
using Disque.Raytracer.Worlds;
using Disque.Math;

namespace Disque.Raytracer.Lights
{
    public class PointLight : Light
    {        
        public float RadianceScale = 1;
        public Vector3 Color = Colors.White, Position = Vector3.Zero;

        public override Vector3 GetDirection(ShadeRec sr)
        {
            return (Vector3.Normalize(Position - sr.HitPoint));
        }

        public override Light Clone()
        {
            PointLight pl = new PointLight();
            pl.RadianceScale = RadianceScale;
            pl.Color = Color;
            pl.Position = Position;
            return pl;
        }

        public override Vector3 L(ShadeRec sr)
        {
            return RadianceScale * Color;
        }

        public override bool InShadow(Ray ray, ShadeRec sr)
        {
            float t = float.MaxValue;		// may be need an initialization
            int num_objects = sr.World.Objects.Count;
            float d = (Position - ray.Position).Length();
            for (int j = 0; j < num_objects; j++)
                if (sr.World.Objects[j].ShadowHit(ray, ref t) && t < d)
                    return (true);
            return (false);
        }
    }
}
