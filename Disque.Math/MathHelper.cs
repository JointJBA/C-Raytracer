using Sys = System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Disque.Math;

namespace Disque
{
    public static class MathHelper
    {
        public const float PI = 3.141592653589793f;
        public const float InvTwoPI = 0.15915494309189533577f;
        public const float Epilson = 0.0001f;
        public const float SleepEpilson = 0.3f;
        public const float PI4div3 = 4.18879f;
        public static float Abs(float n)
        {
            return Sqrt(Pow(n));
        }
        public static float Exp(double value)
        {
            return (float)Sys.Math.Exp(value);
        }
        public static float Pow(float a, float b = 2.0f)
        {
            return (float)Sys.Math.Pow(a, b);
        }
        public static float Sqrt(float n)
        {
            return (float)Sys.Math.Sqrt(n);
        }
        public static float Cbrt(float n)
        {
            float div = 1 / 3;
            return Pow(n, div);
        }
        public static float Cos(float radian)
        {
            return (float)Sys.Math.Cos(radian);
        }
        public static float Acos(float radian)
        {
            return (float)Sys.Math.Acos(radian);
        }
        public static float Sin(float radian)
        {
            return (float)Sys.Math.Sin(radian);
        }
        public static float Asin(float radian)
        {
            return (float)Sys.Math.Asin(radian);
        }
        public static float Tan(float radian)
        {
            return (float)Sys.Math.Tan(radian);
        }
        public static float Atan(float radian)
        {
            return (float)Sys.Math.Atan(radian);
        }
        public static float Atan2(float x, float y)
        {
            return (float)Sys.Math.Atan2(x, y);
        }
        public static float ToRadians(float degrees)
        {
            return degrees * (PI / 180);
        }
        public static float ToDegrees(float radians)
        {
            return radians * (180 / PI);
        }
        public static float Min(float a, float b)
        {
            if (a < b)
                return a;
            return b;
        }
        public static float Max(float a, float b)
        {
            if (a > b)
                return a;
            return b;
        }
        public static int Min(int a, int b)
        {
            if (a < b)
                return a;
            return b;
        }
        public static int Max(int a, int b)
        {
            if (a > b)
                return a;
            return b;
        }
        public static float Lerp(float t, float v1, float v2)
        {
            return (1.0f - t) * v1 + t * v2;
        }
        public static float Clamp(float val, float low, float high)
        {
            if (val < low) return low;
            else if (val > high) return high;
            else return val;
        }
        public static Vector3 UniformSampleCone(float u1, float u2, float costhetamax, Vector3 x, Vector3 y, Vector3 z)
        {
            float costheta = MathHelper.Lerp(u1, costhetamax, 1.0f);
            float sintheta = MathHelper.Sqrt(1.0f - costheta * costheta);
            float phi = u2 * 2.0f * MathHelper.PI;
            return MathHelper.Cos(phi) * sintheta * x + MathHelper.Sin(phi) * sintheta * y + costheta * z;
        }
        public static void ConcentricSampleDisk(float u1, float u2, ref float dx, ref float dy)
        {
            float r, theta;
            float sx = 2 * u1 - 1;
            float sy = 2 * u2 - 1;
            if (sx == 0.0 && sy == 0.0)
            {
                dx = 0.0f;
                dy = 0.0f;
                return;
            }
            if (sx >= -sy)
            {
                if (sx > sy)
                {
                    r = sx;
                    if (sy > 0.0) theta = sy / r;
                    else theta = 8.0f + sy / r;
                }
                else
                {
                    r = sy;
                    theta = 2.0f - sx / r;
                }
            }
            else
            {
                if (sx <= sy)
                {
                    r = -sx;
                    theta = 4.0f - sy / r;
                }
                else
                {
                    r = -sy;
                    theta = 6.0f + sx / r;
                }
            }
            theta *= MathHelper.PI / 4.0f;
            dx = r * MathHelper.Cos(theta);
            dy = r * MathHelper.Sin(theta);
        }
        public static bool Quadratic(float A, float B, float C, ref float t0, ref float t1)
        {
            float discrim = B * B - 4.0f * A * C;
            if (discrim <= 0.0f) return false;
            float rootDiscrim = Sqrt(discrim);
            float q;
            if (B < 0) q = -.5f * (B - rootDiscrim);
            else q = -.5f * (B + rootDiscrim);
            t0 = q / A;
            t1 = C / q;
            if (t0 > t1) Swap<float>(ref t0, ref t1);
            return true;
        }
        public static void Swap<T>(ref T t1, ref T t2)
        {
            T temp = t1;
            t1 = t2;
            t2 = temp;
        }
        public static void Copy<T>(ref T[] destination, ref T[] source)
        {
            destination = (T[])source.Clone();
        }
        public static void Copy<T>(out T[,] destination, T[,] source)
        {
            destination = (T[,])source.Clone();
        }
    }
}
