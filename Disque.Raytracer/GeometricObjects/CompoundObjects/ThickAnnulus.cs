using System;
using System.Collections.Generic;
using System.Text;
using Disque.Raytracer.GeometricObjects.Primitives;
using Disque.Math;

namespace Disque.Raytracer.GeometricObjects.CompoundObjects
{
    public class ThickAnnulus : Compound
    {
        BBox bbox;

        public ThickAnnulus(float bottom, float top, float i_radius, float o_radius, string name)
            : base(name)
        {
            Objects.Add(new Annulus(new Vector3(0.0f, bottom, 0.0f), new Vector3(0, -1, 0), i_radius, o_radius - i_radius, name + "Annulus"));
            Objects.Add(new Annulus(new Vector3(0.0f, top, 0.0f), new Vector3(0, 1, 0), i_radius, o_radius - i_radius, name + "Annulus2"));
            Objects.Add(new OpenCylinder(bottom, top, i_radius, name + "OpenCylinder"));	// iwall
            Objects.Add(new OpenCylinder(bottom, top, o_radius, name + "OpenCylnder2"));	// owall
            bbox.Min.X = -o_radius;
            bbox.Min.Y = bottom;
            bbox.Min.Z = -o_radius;
            bbox.Max.X = o_radius;
            bbox.Max.Y = top;
            bbox.Max.Z = o_radius;
        }

        public override BBox GetBoundingBox()
        {
            return bbox;
        }
    }
}
