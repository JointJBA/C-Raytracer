using System;
using System.Collections.Generic;
using System.Text;
using Disque.Raytracer.Materials;
using Disque.Math;

namespace Disque.Raytracer.GeometricObjects.CompoundObjects
{
    public class Compound : GeometricObject
    {        
        public readonly List<GeometricObject> Objects = new List<GeometricObject>();

        BBox bbox = new BBox(Vector3.Zero, Vector3.Zero);

        public Compound(string name)
            : base(name)
        {
        }

        public override bool Hit(Ray ray, ref float tmin, ref ShadeRec sr)
        {
            if (!bbox.Hit(ray))
                return false;
            float t = 0;
            Vector3 normal = new Vector3(), local_hit_point = Vector3.Zero;
            bool hit = false;
            tmin = float.MaxValue;
            int num_objects = Objects.Count;
            for (int j = 0; j < num_objects; j++)
                if (Objects[j].Hit(ray, ref t,ref sr) && (t < tmin))
                {
                    hit = true;
                    tmin = t;
                    Material = Objects[j].GetMaterial();
                    normal = sr.Normal;
                    local_hit_point = sr.LocalHitPoint;
                }

            if (hit)
            {
                sr.T = tmin;
                sr.Normal = normal;
                sr.LocalHitPoint = local_hit_point;
            }

            return (hit);
        }

        public override bool ShadowHit(Ray ray, ref float tmin)
        {
            if (!Shadows)
                return false;
            float t = float.MaxValue;		// may be important too 
            bool hit = false;
            int num_objects = Objects.Count;
            for (int j = 0; j < num_objects; j++)
                if (Objects[j].ShadowHit(ray, ref t) && (t < tmin))
                {
                    hit = true;
                    tmin = t;
                }
            return (hit);
        }

        public void CopyObjects(List<GeometricObject> objects)
        {
            for (int i = 0; i < Objects.Count; i++)
            {
                GeometricObject obj = Objects[i].Clone();
                objects.Add(obj);
            }
        }

        public override void SetMaterial(Material m)
        {
            for (int i = 0; i < Objects.Count; i++)
                Objects[i].SetMaterial(m);
        }

        public virtual void AddObject(GeometricObject o)
        {
            bbox = BBox.Join(bbox, o.GetBoundingBox());
            Objects.Add(o);
        }

        public override BBox GetBoundingBox()
        {
            return bbox;
        }
    }
}
