using Sys = System;
using System.Collections.Generic;
using System.Text;
using Disque.Math;
using System;

namespace Disque.Raytracer
{
    class MathHel
    {
        static Random r = new Random(DateTime.Now.Millisecond);
        public const float PI = 3.141592653589793f;
        public const float TwoPI = 6.2831853071795864769f;
        public const float PIOn180 = 0.0174532925199432957f;
        public const float InvPI = 0.3183098861837906715f;
        public const float InvTwoPI = 0.15915494309189533577f;
        public const float Epsilon = 0.0001f;
        public const float SleepEpilson = 0.3f;
        public const float PI4div3 = 4.18879f;
        public const float InvRandMax = 1.0f / 0x7fff;
        public static float Abs(float n)
        {
            return Sqrt(Pow(n));
        }
        public static float Exp(float value)
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
        public static int Clamp(int val, int low, int high)
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
        public static int RandomInt()
        {
            return r.Next(0x7fff);
        }
        public static int RandomInt(int max)
        {
            return r.Next(max);
        }
        public static int RandomInt(int l, int h)
        {
            return (r.Next(l, h));
        }
        public static float RandomFloat()
        {
            return (float)RandomInt() * InvRandMax;
        }
        public static float RandomFloat(float l, float h)
        {
            float ans = RandomFloat() * (h - (l)) + (l);
            return ans;
        }
        public static int SolveQuadric(float[] c, float[] s)
        {
            float p, q, D;
            p = c[1] / (2 * c[2]);
            q = c[0] / c[2];
            D = p * p - q;
            if ((D) == 0)
            {
                s[0] = -p;
                return 1;
            }
            else if (D > 0)
            {
                float sqrt_D = Sqrt(D);

                s[0] = sqrt_D - p;
                s[1] = -sqrt_D - p;
                return 2;
            }
            else
                return 0;
        }
        public static int SolveCubic(float[] c, float[] s)
        {
            int i, num;
            float sub;
            float A, B, C;
            float sq_A, p, q;
            float cb_p, D;
            A = c[2] / c[3];
            B = c[1] / c[3];
            C = c[0] / c[3];
            sq_A = A * A;
            p = 1.0f / 3.0f * (-1.0f / 3.0f * sq_A + B);
            q = 1.0f / 2.0f * (2.0f / 27.0f * A * sq_A - 1.0f / 3.0f * A * B + C);
            cb_p = p * p * p;
            D = q * q + cb_p;

            if (IsZero(D))
            {
                if (IsZero(q))
                { /* one triple solution */
                    s[0] = 0;
                    num = 1;
                }
                else
                { /* one single and one float solution */
                    float u = Cbrt(-q);
                    s[0] = 2.0f * u;
                    s[1] = -u;
                    num = 2;
                }
            }
            else if (D < 0)
            {
                float phi = 1.0f / 3.0f * Acos(-q / Sqrt(-cb_p));
                float t = 2 * Sqrt(-p);

                s[0] = t * Cos(phi);
                s[1] = -t * Cos(phi + PI / 3.0f);
                s[2] = -t * Cos(phi - PI / 3.0f);
                num = 3;
            }
            else
            { /* one real solution */
                float sqrt_D = Sqrt(D);
                float u = Cbrt(sqrt_D - q);
                float v = -Cbrt(sqrt_D + q);
                s[0] = u + v;
                num = 1;
            }
            sub = 1.0f / 3.0f * A;
            for (i = 0; i < num; ++i)
                s[i] -= sub;
            return num;
        }
        public static int SolveQuartic(float[] c, float[] s)
        {
            float[] coeffs = new float[4];
            float z, u, v, sub;
            float A, B, C, D;
            float sq_A, p, q, r;
            int i, num;
            A = c[3] / c[4];
            B = c[2] / c[4];
            C = c[1] / c[4];
            D = c[0] / c[4];
            sq_A = A * A;
            p = -3.0f / 8.0f * sq_A + B;
            q = 1.0f / 8.0f * sq_A * A - 1.0f / 2.0f * A * B + C;
            r = -3.0f / 256.0f * sq_A * sq_A + 1.0f / 16.0f * sq_A * B - 1.0f / 4.0f * A * C + D;
            if (IsZero(r))
            {
                coeffs[0] = q;
                coeffs[1] = p;
                coeffs[2] = 0;
                coeffs[3] = 1;

                num = SolveCubic(coeffs, s);
                s[num++] = 0;
            }
            else
            {
                coeffs[0] = 1.0f / 2.0f * r * p - 1.0f / 8.0f * q * q;
                coeffs[1] = -r;
                coeffs[2] = -1.0f / 2.0f * p;
                coeffs[3] = 1;
                SolveCubic(coeffs, s);
                z = s[0];
                u = z * z - r;
                v = 2.0f * z - p;
                if (IsZero(u))
                    u = 0;
                else if (u > 0)
                    u = MathHelper.Sqrt(u);
                else
                    return 0;

                if (IsZero(v))
                    v = 0;
                else if (v > 0)
                    v = Sqrt(v);
                else
                    return 0;
                coeffs[0] = z - u;
                coeffs[1] = q < 0 ? -v : v;
                coeffs[2] = 1;
                num = SolveQuadric(coeffs, s);
                coeffs[0] = z + u;
                coeffs[1] = q < 0 ? v : -v;
                coeffs[2] = 1;
                num += SolveQuadric(coeffs, s);
            }
            sub = 1.0f / 4.0f * A;
            for (i = 0; i < num; ++i)
                s[i] -= sub;
            return num;
        }
        public static bool IsZero(float x)
        {
            float EQN_EPS = 1e-90f;
            return (x) > -EQN_EPS && (x) < EQN_EPS;
        }
        public static int Floor(float p)
        {
            return (int)Sys.Math.Floor(p);
        }
    }
}
