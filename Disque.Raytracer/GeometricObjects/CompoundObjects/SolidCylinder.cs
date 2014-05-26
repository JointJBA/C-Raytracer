using System;
using System.Collections.Generic;
using System.Text;
using Disque.Raytracer.GeometricObjects.Primitives;
using Disque.Math;

namespace Disque.Raytracer.GeometricObjects.CompoundObjects
{
    public class SolidCylinder : Compound
    {
        BBox bbox = new BBox();

        public SolidCylinder(float bottom, float top, float radius, string name)
            : base(name)
        {
            Objects.Add(new Disk(new Vector3(0, bottom, 0), Vector3.Down, radius, name + "Disk"));
            Objects.Add(new OpenCylinder(bottom, top, radius, name + "OpenCylinder"));
            Objects.Add(new Disk(new Vector3(0, top, 0), Vector3.Up, radius, name + "Disk2"));
            bbox.Min.X = -radius;
            bbox.Min.Y = bottom;
            bbox.Min.Z = -radius;
            bbox.Max.X = radius;
            bbox.Max.Y = top;
            bbox.Max.Z = radius;
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

        public override BBox GetBoundingBox()
        {
            return bbox;
        }
    }
}
