using System;
using System.Collections.Generic;
using System.Text;
using Disque.Raytracer.Samplers;
using Disque.Raytracer.Worlds;
using Disque.Math;

namespace Disque.Raytracer.Cameras
{
    public class ThinLens : Camera
    {
        public float Radius = 1, Distance = 1, FocalDistance = 1, Zoom = 1;
        Sampler Sampler;

        public void SetSampler(Sampler sp)
        {
            Sampler = sp;
            Sampler.MapSamplesToDisk();
        }

        Vector3 ray_direction(Vector2 pixel_point, Vector2 lens_point)
        {
            Vector2 p = new Vector2(pixel_point.X * FocalDistance / Distance, pixel_point.Y * FocalDistance / Distance);   // hit point on focal plane
            Vector3 dir = (p.X - lens_point.X) * U + (p.Y - lens_point.Y) * V - FocalDistance * W;
            dir.Normalize();
            return (dir);
        }

        public override void RenderScene(World world)
        {
            Vector3 L;
            Ray ray = new Ray();
            ViewPlane vp = world.ViewPlane;
            int depth = 0;
            Vector2 sp;			// sample point in [0, 1] X [0, 1]
            Vector2 pp;			// sample point on a pixel
            Vector2 dp; 		// sample point on unit disk
            Vector2 lp;			// sample point on lens
            vp.S /= Zoom;
            for (int r = 0; r < vp.VRes; r++)			// up
                for (int c = 0; c < vp.HRes; c++)
                {		// across 
                    L = new Vector3(0, 0, 0);
                    for (int n = 0; n < vp.NumSamples; n++)
                    {
                        sp = vp.Sampler.SampleUnitSquare();
                        pp = new Vector2();
                        pp.X = vp.S * (((float)c) - ((float)vp.HRes) / 2.0f + sp.X);
                        pp.Y = vp.S * (((float)r) - ((float)vp.VRes) / 2.0f + sp.Y);

                        dp = Sampler.SampleUnitDisk();
                        lp = dp * Radius;
                        ray.Position = Position + lp.Y * U + lp.Y * V;
                        ray.Direction = ray_direction(pp, lp);
                        L += world.Tracer.TraceRay(ray, depth);
                    }

                    L /= vp.NumSamples;
                    L *= ExposureTime;
                    world.Screen.AddPixel(r, c, L);
                }
        }

        public override void RenderStereo(World world, float x, int offset)
        {
            Vector3 L = Vector3.Zero;
            Ray ray = new Ray();
            ViewPlane vp = (world.ViewPlane);
            int depth = 0;
            Vector2 sp, pp, dp, lp;			// sample point in [0, 1] X [0, 1]	
            vp.S /= Zoom;
            for (int r = 0; r < vp.VRes; r++)			// up
                for (int c = 0; c < vp.HRes; c++)
                {		// across 
                    L = Vector3.Zero;
                    for (int n = 0; n < vp.NumSamples; n++)
                    {
                        sp = vp.Sampler.SampleUnitSquare();
                        pp = new Vector2();
                        pp.X = vp.S * (c - vp.HRes / 2.0f + sp.X);
                        pp.Y = vp.S * (r - vp.VRes / 2.0f + sp.Y);
                        dp = Sampler.SampleUnitDisk();
                        lp = dp * Radius;
                        ray.Position = Position + lp.X * U + lp.Y * V;
                        ray.Direction = ray_direction(pp, lp);
                        L += world.Tracer.TraceRay(ray, depth);
                    }

                    L /= vp.NumSamples;
                    L *= ExposureTime;
                    world.Screen.AddPixel(r, c + offset, L);
                }
        }
    }
}
