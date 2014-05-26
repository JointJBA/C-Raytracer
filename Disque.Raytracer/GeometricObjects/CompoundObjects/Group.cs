using System;
using System.Collections.Generic;
using System.Text;
using Disque.Math;

namespace Disque.Raytracer.GeometricObjects.CompoundObjects
{
    public class Group : Compound
    {
        BBox bbox;

        public Group(string name)
            : base(name)
        {
            bbox = new BBox();
        }

        public override void AddObject(GeometricObject o)
        {
            bbox.Union(o.GetBoundingBox());
            Objects.Add(o);
        }

        public override bool Hit(Ray ray, ref float tmin, ref ShadeRec sr)
        {
            if (bbox.Hit(ray))
                return base.Hit(ray, ref tmin, ref sr);
            else 
                return false;
        }

        public override bool ShadowHit(Ray ray, ref float tmin)
        {
            if (bbox.Hit(ray))
                return base.ShadowHit(ray, ref tmin);
            else
                return false;
        }

        public override BBox GetBoundingBox()
        {
            return bbox;
        }
    }
}
