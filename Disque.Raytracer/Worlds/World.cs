using System;
using System.Collections.Generic;
using System.Text;
using Disque.Raytracer.Tracers;
using Disque.Raytracer.GeometricObjects;
using Disque.Raytracer.Cameras;
using Disque.Raytracer.Samplers;
using Disque.Raytracer.Lights;
using Disque.Math;
using Disque.Raytracer.GeometricObjects.CompoundObjects;

namespace Disque.Raytracer.Worlds
{
	public class World
	{
        public ViewPlane ViewPlane = new ViewPlane();
        public Light AmbientLight;
        public Camera Camera;
        public Vector3 Background = Vector3.One;
        public Tracer Tracer;
        public PixelScreen Screen;
        public readonly List<GeometricObject> Objects = new List<GeometricObject>();
        public List<GeometricObject> FreeObjects = new List<GeometricObject>();
        public List<Light> Lights = new List<Light>();

        public ShadeRec HitObjects(Ray ray)
        {
            ShadeRec sr = new ShadeRec(this);
            float t = 0;
            Vector3 normal = Vector3.Zero;
            Vector3 local_hit_point = Vector3.Zero;
            float tmin = float.MaxValue;
            int num_objects = Objects.Count;

            for (int j = 0; j < num_objects; j++)
            {
                if (Objects[j].Hit(ray, ref t, ref sr) && (t < tmin))
                {
                    sr.Hit_an_object = true;
                    tmin = t;
                    sr.Material = Objects[j].GetMaterial();
                    sr.HitPoint = ray.Position + t * ray.Direction;
                    normal = sr.Normal;
                    local_hit_point = sr.LocalHitPoint;
                }
            }
            if (sr.Hit_an_object)
            {
                sr.T = tmin;
                sr.Normal = normal;
                sr.LocalHitPoint = local_hit_point;
            }
            return (sr);
        }

        public void RenderScene()
        {
            if (Screen == null)
                Screen = new PixelScreen(ViewPlane);
            Screen.Pixels.Clear();
            Camera.ComputeUVW();
            Camera.RenderScene(this);
        }

        public void Remove()
        {
            foreach (GeometricObject go in Objects)
            {
                removeObject(go);
            }
            foreach (GeometricObject go in FreeObjects)
            {
                removeObject(go);
            }
        }

        static void removeObject(GeometricObject go)
        {
            if (go is Compound)
            {
                Compound com = (Compound)go;
                foreach (GeometricObject g in com.Objects)
                {
                    removeObject(g);
                }
            }
            GeometricObject.Remove(go.Name);
        }
    }
}
