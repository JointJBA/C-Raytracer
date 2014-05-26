using System;
using System.Collections.Generic;
using System.Text;
using Disque.Raytracer.Worlds;
using Disque.Math;

namespace Disque.Raytracer.Cameras
{
    public class Orthographic : Camera
    {
        public float Zoom = 1, Distance = 1;

        public Orthographic()
            : base()
        {
        }

        Vector3 get_direction(Vector2 p)
        {
            Vector3 dir = p.X * U + p.Y * V - Distance * W;
            return (dir);
        }

        public override void RenderScene(World world)
        {
            Vector3 L;
            ViewPlane vp = world.ViewPlane;
            Ray ray = new Ray();
            int depth = 0;
            Vector2 pp;
            int n = (int)MathHelper.Sqrt((float)vp.NumSamples);
            vp.S /= Zoom;
            ray.Position = Position;

            for (int r = 0; r < vp.VRes; r++)
                for (int c = 0; c < vp.HRes; c++)
                {
                    L = Vector3.Zero; ;
                    for (int p = 0; p < n; p++)
                        for (int q = 0; q < n; q++)
                        {
                            pp.X = vp.S * (((float)c) - 0.5f * ((float)vp.HRes) + (((float)q) + 0.5f) / ((float)n));
                            pp.Y = vp.S * (((float)r) - 0.5f * ((float)vp.VRes) + (((float)p) + 0.5f) / ((float)n));
                            ray.Direction = Target - Position;
                            ray.Direction.Normalize();
                            Vector3 temp = get_direction(pp);
                            ray.Position = Position + temp;
                            L += world.Tracer.TraceRay(ray, depth);
                        }

                    L /= vp.NumSamples;
                    L *= ExposureTime;
                    world.Screen.AddPixel(r, c, L);
                }
        }

        public override void RenderStereo(World world, float x, int offset)
        {
            Vector3 L;
            ViewPlane vp = world.ViewPlane;
            Ray ray = new Ray();
            int depth = 0;
            Vector2 pp, sp;
            int n = (int)MathHelper.Sqrt((float)vp.NumSamples);
            vp.S /= Zoom;
            ray.Position = Position;

            for (int r = 0; r < vp.VRes; r++)
                for (int c = 0; c < vp.HRes; c++)
                {
                    L = Vector3.Zero; ;
                    for (int p = 0; p < n; p++)
                        for (int q = 0; q < n; q++)
                        {
                            sp = vp.Sampler.SampleUnitSquare();
                            pp = new Vector2();
                            pp.X = vp.S * (((float)c) - 0.5f * ((float)vp.HRes) + sp.X) + x;
                            pp.Y = vp.S * (((float)r) - 0.5f * ((float)vp.VRes) + sp.Y);
                            ray.Direction = get_direction(pp);
                            L += world.Tracer.TraceRay(ray, depth);
                        }

                    L /= vp.NumSamples;
                    L *= ExposureTime;
                    world.Screen.AddPixel(r, c + offset, L);
                }
        }
    }
}
