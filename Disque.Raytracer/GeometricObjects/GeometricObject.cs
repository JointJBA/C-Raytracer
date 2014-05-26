using System;
using System.Collections.Generic;
using System.Text;
using Disque.Raytracer.Materials;
using Disque.Math;
using Disque.Raytracer.Rml;
using Disque.Raytracer.Worlds;
using Disque.Raytracer.Samplers;

namespace Disque.Raytracer.GeometricObjects
{
    public class GeometricObject
    {
        static Dictionary<string, GeometricObject> Objects = new Dictionary<string, GeometricObject>();
        protected internal Material Material;
        protected bool Shadows = true;
        string name;
        public string Name
        {
            get
            {
                return name;
            }
        }

        public static GeometricObject GetObject(string s)
        {
            return Objects[s];
        }

        public GeometricObject(string _name)
        {
            if (!Objects.ContainsKey(_name))
            {
                Objects.Add(_name, this); 
                name = _name;
            }
            else
            {
                throw new Exception(_name + " already exists.");
            }
        }

        public virtual bool Hit(Ray ray, ref float tmin, ref ShadeRec sr)
        {
            return false;
        }

        public virtual bool ShadowHit(Ray ray, ref float tmin)
        {
            return false;
        }

        public virtual BBox GetBoundingBox()
        {
            return new BBox();
        }

        public virtual void SetBoundingBox()
        {
        }

        public virtual Vector3 Sample()
        {
            return Vector3.Zero;
        }

        public virtual void SetSampler(Sampler s)
        {
        }

        public virtual Vector3 GetNormal()
        {
            return Vector3.Zero;
        }

        public virtual Vector3 GetNormal(Vector3 p)
        {
            return Vector3.Zero;
        }

        public virtual float PDF(ShadeRec sr)
        {
            return 0;
        }

        public virtual GeometricObject Clone()
        {
            throw new NotImplementedException();
        }

        public virtual void SetMaterial(Material m)
        {
            Material = m;
        }

        public virtual Material GetMaterial()
        {
            return Material;
        }

        public virtual void SetShadows(bool sh)
        {
            Shadows = sh;
        }        
        
        public virtual bool GetShadows()
        {
            return Shadows;
        }

        public static void Clear()
        {
            Objects.Clear();
        }

        public static bool Contains(string name)
        {
            return Objects.ContainsKey(name);
        }

        public static void Remove(string name)
        {
            Objects.Remove(name);
        }

        public static string GetUniqueRandomName()
        {
            string name = "";
            do
            {
                name = "";
                for (int i = 0; i < 20; i++)
                {
                    name += ((char)MathHelper.RandomInt(66, 120));
                }
            }
            while (Contains(name));
            return name;
        }

    }
}
