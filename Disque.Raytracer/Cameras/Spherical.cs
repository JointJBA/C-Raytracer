using System;
using System.Collections.Generic;
using System.Text;
using Disque.Raytracer.Worlds;
using Disque.Math;

namespace Disque.Raytracer.Cameras
{
    public class Spherical : Camera
    {
        public float LambdaMax, PSIMax;

        Vector3 ray_direction(Vector2 pp, int hres, int vres, float s)
        {
            Vector2 pn = new Vector2(2.0f / (s * hres) * pp.X, 2.0f / (s * vres) * pp.Y);
            float lambda = pn.X * LambdaMax * MathHelper.PIOn180;
            float psi = pn.Y * PSIMax * MathHelper.PIOn180;
            float phi = MathHelper.PI - lambda;
            float theta = 0.5f * MathHelper.PI - psi;
            float sin_phi = MathHelper.Sin(phi);
            float cos_phi = MathHelper.Cos(phi);
            float sin_theta = MathHelper.Sin(theta);
            float cos_theta = MathHelper.Cos(theta);
            Vector3 dir = sin_theta * sin_phi * U + cos_theta * V + sin_theta * cos_phi * W;
            return (dir);
        }

        public Spherical()
            : base()
        {
        }

        public override void RenderScene(World world)
        {
            Vector3 L;
            ViewPlane vp = (world.ViewPlane);
            int hres = vp.HRes;
            int vres = vp.VRes;
            float s = vp.S;
            Ray ray = new Ray();
            int depth = 0;
            Vector2 sp; 					// sample point in [0, 1] X [0, 1]
            Vector2 pp;						// sample point on the pixel
            ray.Position = Position;
            for (int r = 0; r < vres; r++)		// up
                for (int c = 0; c < hres; c++)
                {	// across 					
                    L = new Vector3(0, 0, 0);

                    for (int j = 0; j < vp.NumSamples; j++)
                    {
                        sp = vp.Sampler.SampleUnitSquare();
                        pp = new Vector2();
                        pp.X = s * (c - 0.5f * hres + sp.X);
                        pp.Y = s * (r - 0.5f * vres + sp.Y);
                        ray.Direction = ray_direction(pp, hres, vres, s);
                        L += world.Tracer.TraceRay(ray, depth);
                    }
                    L /= vp.NumSamples;
                    L *= ExposureTime;
                    world.Screen.AddPixel(r, c, L);
                }
        }
    }
}
