using System;
using System.Collections.Generic;
using System.Text;
using Disque.Raytracer.Worlds;
using Disque.Math;

namespace Disque.Raytracer.Cameras
{
    public class Pinhole : Camera
    {
        public float Zoom = 1, Distance = 1;

        Vector3 GetDirection(Vector2 p)
        {
            Vector3 dir = p.X * U + p.Y * V - Distance * W;
            dir.Normalize();
            return (dir);
        }

        public override void RenderScene(World world)
        {
            Vector3 L;
            ViewPlane vp = new ViewPlane(world.ViewPlane);
            Ray ray = new Ray();
            int depth = 0;
            Vector2 pp  = new Vector2();
            vp.S /= Zoom;
            ray.Position = Position;
            int n = (int)MathHelper.Sqrt((float)vp.NumSamples);
            for (int r = 0; r < vp.VRes; r++)
                for (int c = 0; c < vp.HRes; c++)
                {
                    L = new Vector3(0, 0, 0);
                    for (int p = 0; p < n; p++)
                        for (int q = 0; q < n; q++)
                        {
                            pp.X = vp.S * (c - 0.5f * vp.HRes + (q + 0.5f) / n);
                            pp.Y = vp.S * (r - 0.5f * vp.VRes + (p + 0.5f) / n);
                            ray.Direction = GetDirection(pp);
                            L += world.Tracer.TraceRay(ray, depth);
                        }

                    L /= ((float)vp.NumSamples);
                    L *= ExposureTime;
                    world.Screen.AddPixel(r, c, L);
                }
        }

        public override Vector3 RenderRay(World world, int c, int r, int n)
        {
            Vector3 L = Vector3.Zero;
            Vector2 pp = new Vector2();
            var ray = new Ray();
            int depth = 0;
            ray.Position = Position;
            ViewPlane vp = world.ViewPlane;
            for (int p = 0; p < n; p++)
                for (int q = 0; q < n; q++)
                {
                    pp.X = vp.S * (c - 0.5f * vp.HRes + (q + 0.5f) / n);
                    pp.Y = vp.S * (r - 0.5f * vp.VRes + (p + 0.5f) / n);
                    ray.Direction = GetDirection(pp);
                    L += world.Tracer.TraceRay(ray, depth);
                }

            L /= ((float)vp.NumSamples);
            L *= ExposureTime;
            return L;
        }

        public override void RenderStereo(World world, float x, int offset)
        {
            Vector3 L = Vector3.Zero;
            ViewPlane vp = world.ViewPlane;
            Ray ray = new Ray();
            int depth = 0;
            Vector2 pp = new Vector2();
            Vector2 sp = new Vector2();
            vp.S /= Zoom;
            ray.Position = Position;
            for (int r = 0; r < vp.VRes; r++)
                for (int c = 0; c < vp.HRes; c++)
                {
                    L = new Vector3(0, 0, 0);
                    for (int j = 0; j < vp.NumSamples; j++)
                    {
                        sp = vp.Sampler.SampleUnitSquare();
                        pp.X = vp.S * (c - 0.5f * vp.HRes + sp.X) + x;
                        pp.Y = vp.S * (r - 0.5f * vp.VRes + sp.Y);
                        ray.Direction = GetDirection(pp);
                        L += world.Tracer.TraceRay(ray, depth);
                    }
                    L /= vp.NumSamples;
                    L *= ExposureTime;
                    world.Screen.AddPixel(r, c + offset, L);
                }
        }

        public Pinhole()
            : base()
        {
        }
    }
}
