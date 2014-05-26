using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Disque.Math;

namespace Disque.Raytracer.Textures.NoiseBased
{
    public class CubicNoise : LatticeNoise
    {
        public CubicNoise(int octaves, float lacunarity, float gain)
            : base(octaves, lacunarity, gain)
        {
        }

        public override float value_noise(Vector3 p)
        {
            int ix, iy, iz;
            float fx, fy, fz;
            float[] xknots = new float[4], yknots = new float[4], zknots = new float[4];
            ix = MathHelper.Floor(p.X);
            fx = p.X - ix;
            iy = MathHelper.Floor(p.Y);
            fy = p.Y - iy;
            iz = MathHelper.Floor(p.Z);
            fz = p.Z - iz;
            for (int k = -1; k <= 2; k++)
            {
                for (int j = -1; j <= 2; j++)
                {
                    for (int i = -1; i <= 2; i++)
                    {
                        xknots[i + 1] = value_table[LinearNoise.INDEX(ix + i, iy + j, iz + k)];
                    }
                    yknots[j + 1] = four_knot_spline(fx, xknots);
                }
                zknots[k + 1] = four_knot_spline(fy, yknots);
            }
            return (MathHelper.Clamp(four_knot_spline(fz, zknots), -1.0f, 1.0f));
        }

        public override Vector3 vector_noise(Vector3 p)
        {
            int ix, iy, iz;
            float fx, fy, fz;
            Vector3[] xknots = new Vector3[4], yknots = new Vector3[4], zknots = new Vector3[4];
            ix = MathHelper.Floor(p.X);
            fx = p.X - ix;
            iy = MathHelper.Floor(p.Y);
            fy = p.Y - iy;
            iz = MathHelper.Floor(p.Z);
            fz = p.Z - iz;
            for (int k = -1; k <= 2; k++)
            {
                for (int j = -1; j <= 2; j++)
                {
                    for (int i = -1; i <= 2; i++)
                    {
                        xknots[i + 1] = vector_table[LinearNoise.INDEX(ix + i, iy + j, iz + k)];
                    }
                    yknots[j + 1] = four_knot_spline(fx, xknots);
                }
                zknots[k + 1] = four_knot_spline(fy, yknots);
            }
            return (four_knot_spline(fz, zknots));
        }

        float four_knot_spline(float x, float[] knots)
        {
            float c3 = -0.5f * knots[0] + 1.5f * knots[1] - 1.5f * knots[2] + 0.5f * knots[3];
            float c2 = knots[0] - 2.5f * knots[1] + 2.0f * knots[2] - 0.5f * knots[3];
            float c1 = 0.5f * (-knots[0] + knots[2]);
            float c0 = knots[1];
            return (((c3 * x + c2) * x + c1) * x + c0);
        }

        Vector3 four_knot_spline(float x, Vector3[] knots)
        {
            Vector3 c3 = -0.5f * knots[0] + 1.5f * knots[1] - 1.5f * knots[2] + 0.5f * knots[3];
            Vector3 c2 = knots[0] - 2.5f * knots[1] + 2.0f * knots[2] - 0.5f * knots[3];
            Vector3 c1 = 0.5f * (-knots[0] + knots[2]);
            Vector3 c0 = knots[1];
            return (((c3 * x + c2) * x + c1) * x + c0);
        }
    }
}
