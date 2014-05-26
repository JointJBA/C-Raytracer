using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Disque.Math;

namespace Disque.Raytracer.Textures.NoiseBased
{
    public class LinearNoise : LatticeNoise
    {
        public LinearNoise(int oct, float lac, float g)
            : base(oct, lac, g)
        {
        }

        public override float value_noise(Vector3 p)
        {
            int ix, iy, iz;
            float fx, fy, fz;
            float[, ,] d = new float[2, 2, 2];
            float x0, x1, x2, x3, y0, y1, z0;
            ix = MathHelper.Floor(p.X);
            fx = p.X - ix;
            iy = MathHelper.Floor(p.Y);
            fy = p.Y - iy;
            iz = MathHelper.Floor(p.Z);
            fz = p.Z - iz;
            for (int k = 0; k <= 1; k++)
                for (int j = 0; j <= 1; j++)
                    for (int i = 0; i <= 1; i++)
                        d[k, j, i] = value_table[INDEX(ix + i, iy + j, iz + k)];

            x0 = MathHelper.Lerp(fx, d[0, 0, 0], d[0, 0, 1]);
            x1 = MathHelper.Lerp(fx, d[0, 1, 0], d[0, 1, 1]);
            x2 = MathHelper.Lerp(fx, d[1, 0, 0], d[1, 0, 1]);
            x3 = MathHelper.Lerp(fx, d[1, 1, 0], d[1, 1, 1]);
            y0 = MathHelper.Lerp(fy, x0, x1);
            y1 = MathHelper.Lerp(fy, x2, x3);
            z0 = MathHelper.Lerp(fz, y0, y1);
            return (z0);
        }

        public override Vector3 vector_noise(Vector3 p)
        {
            int ix, iy, iz;
            float fx, fy, fz;
            Vector3[, ,] d = new Vector3[2, 2, 2];
            Vector3 x0, x1, x2, x3, y0, y1, z0;
            ix = MathHelper.Floor(p.X);
            fx = p.X - ix;
            iy = MathHelper.Floor(p.Y);
            fy = p.Y - iy;
            iz = MathHelper.Floor(p.Z);
            fz = p.Z - iz;
            for (int k = 0; k <= 1; k++)
                for (int j = 0; j <= 1; j++)
                    for (int i = 0; i <= 1; i++)
                        d[k, j, i] = vector_table[INDEX(ix + i, iy + j, iz + k)];
            x0 = MathHelper.Lerp(fx, d[0, 0, 0], d[0, 0, 1]);
            x1 = MathHelper.Lerp(fx, d[0, 1, 0], d[0, 1, 1]);
            x2 = MathHelper.Lerp(fx, d[1, 0, 0], d[1, 0, 1]);
            x3 = MathHelper.Lerp(fx, d[1, 1, 0], d[1, 1, 1]);
            y0 = MathHelper.Lerp(fy, x0, x1);
            y1 = MathHelper.Lerp(fy, x2, x3);
            z0 = MathHelper.Lerp(fz, y0, y1);
            return (z0);
        }

        public static byte PERM(int x)
        {
            return permutation_table[x & kTableMask];
        }

        public static byte INDEX(int ix, int iy, int iz)
        {
            return PERM((ix) + PERM((iy) + PERM(iz)));
        }
    }
}
