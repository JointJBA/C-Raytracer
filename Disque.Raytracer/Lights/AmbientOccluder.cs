using System;
using System.Collections.Generic;
using System.Text;
using Disque.Math;
using Disque.Raytracer.Samplers;

namespace Disque.Raytracer.Lights
{
    public class AmbientOccluder : Light
    {
        public float RadianceScale = 1;
        public float MinAmount = 0;
        public Vector3 Color = Colors.White;
        Vector3 U, V, W;
        Sampler sampler;

        public override Light Clone()
        {
            throw new NotImplementedException();
        }

        public override Vector3 GetDirection(ShadeRec sr)
        {
            Vector3 sp = sampler.SampleHemisphere();
            return (sp.X * U + sp.Y * V + sp.Z * W);
        }

        public override Vector3 L(ShadeRec sr)
        {
            W = sr.Normal;
            V = Vector3.Cross(W, new Vector3(0.0072f, 1, 0.0034f));
            V.Normalize();
            U = Vector3.Cross(V, W);
            Ray shadow_ray = new Ray();
            shadow_ray.Position = sr.HitPoint;
            shadow_ray.Direction = GetDirection(sr);

            if (InShadow(shadow_ray, sr))
                return (MinAmount * RadianceScale * Color);
            else
                return (RadianceScale * Color);
        }

        public override bool InShadow(Ray ray, ShadeRec sr)
        {
            float t = float.MaxValue;
            int num_objects = sr.World.Objects.Count;
            for (int j = 0; j < num_objects; j++)
            {
                if (sr.World.Objects[j].ShadowHit(ray, ref t))
                    return true;
            }
            return (false);
        }

        public void SetSampler(Sampler s)
        {
            sampler = s;
            sampler.MapSamplesToHemisphere(1);
        }
    }
}
