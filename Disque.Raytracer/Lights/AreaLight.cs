using System;
using System.Collections.Generic;
using System.Text;
using Disque.Math;
using Disque.Raytracer.Materials;
using Disque.Raytracer.GeometricObjects;

namespace Disque.Raytracer.Lights
{
    public class AreaLight : Light
    {
        Material Material;
        GeometricObject Object;
        Vector3 sample_point;
        Vector3 light_normal;
        Vector3 wi;

        public override Light Clone()
        {
            throw new NotImplementedException();
        }

        public override Vector3 GetDirection(ShadeRec sr)
        {
            sample_point = Object.Sample();
            light_normal = Object.GetNormal(sample_point);
            wi = sample_point - sr.HitPoint;
            wi.Normalize();
            return wi;
        }

        public override Vector3 L(ShadeRec sr)
        {
            float ndotd = Vector3.Dot(-light_normal, wi);
            if (ndotd > 0.0)
                return (Material.Get_Le(sr));
            else
                return (new Vector3());
        }

        public override bool InShadow(Ray ray, ShadeRec sr)
        {
            float t = 0;
            int num_objects = sr.World.Objects.Count;
            float ts = Vector3.Dot((sample_point - ray.Position), ray.Direction);
            for (int j = 0; j < num_objects; j++)
                if (sr.World.Objects[j].ShadowHit(ray, ref t) && t < ts)
                    return (true);

            return (false);
        }

        public override float G(ShadeRec sr)
        {
            float ndotd = Vector3.Dot(-light_normal, wi);
            float d2 = (sample_point - sr.HitPoint).LengthSquared();
            return (ndotd / d2);
        }

        public override float PDF(ShadeRec sr)
        {
            return Object.PDF(sr);
        }

        public void SetObject(GeometricObject obj)
        {
            Object = obj;
            Material = Object.GetMaterial();
        }
    }
}
