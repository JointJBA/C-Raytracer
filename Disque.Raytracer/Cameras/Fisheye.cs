using System;
using System.Collections.Generic;
using System.Text;
using Disque.Raytracer.Worlds;
using Disque.Math;

namespace Disque.Raytracer.Cameras
{
    public class Fisheye : Camera
    {
        public Fisheye()
            : base()
        {
        }

        public float PSIMax;
        Vector3 ray_direction(Vector2 pp, int hres, int vres, float s, ref float rsquared)
        {
            Vector2 pn = new Vector2(2.0f / (s * hres) * pp.X, 2.0f / (s * vres) * pp.Y);
            rsquared = pn.X * pn.X + pn.Y * pn.Y;
            if (rsquared <= 1.0)
            {
                float r = MathHelper.Sqrt(rsquared);
                float psi = r * PSIMax * MathHelper.PIOn180;
                float sin_psi = MathHelper.Sin(psi);
                float cos_psi = MathHelper.Cos(psi);
                float sin_alpha = pn.Y / r;
                float cos_alpha = pn.X / r;
                Vector3 dir = sin_psi * cos_alpha * U + sin_psi * sin_alpha * V - cos_psi * W;
                return (dir);
            }
            else
                return (new Vector3(0));
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
            Vector2 pp = new Vector2();						// sample point on the pixel
            float r_squared = 0;				// sum of squares of Vector3ised device coordinates
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
                        ray.Direction = ray_direction(pp, hres, vres, s, ref r_squared);

                        if (r_squared <= 1.0)
                            L += world.Tracer.TraceRay(ray, depth);
                    }
                    L /= vp.NumSamples;
                    L *= ExposureTime;
                    world.Screen.AddPixel(pp.X, pp.Y, L);
                }
        }
    }
}
