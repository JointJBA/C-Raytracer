using System;
using System.Collections.Generic;
using System.Text;
using Disque.Raytracer.GeometricObjects.Primitives;
using Disque.Math;

namespace Disque.Raytracer.GeometricObjects.CompoundObjects
{
    public class SolidCone : Compound
    {
        BBox bbox;

        public SolidCone(float r, float h, string name)
            : base(name)
        {
            Objects.Add(new OpenCone(r, h, name + "OpenCone"));
            Objects.Add(new Disk(new Vector3(0, 0, 0), Vector3.Down, r, name + "Disk"));
            bbox = new BBox();
            bbox.Min.X = -r;
            bbox.Min.Y = 0;
            bbox.Min.Z = -r;
            bbox.Max.X = r;
            bbox.Max.Y = h;
            bbox.Max.Z = r;
        }

        public override BBox GetBoundingBox()
        {
            return bbox;
        }

        public override bool Hit(Ray ray, ref float tmin, ref ShadeRec sr)
        {
            if (bbox.Hit(ray))
                return base.Hit(ray, ref tmin, ref sr);
            return false;
        }

        public override bool ShadowHit(Ray ray, ref float tmin)
        {
            if (bbox.Hit(ray))
                return base.ShadowHit(ray, ref tmin);
            return false;
        }
    }
}
