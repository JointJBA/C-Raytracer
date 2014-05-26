using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Disque.Math
{
    public struct Quaternion
    {
        public float X;
        public float Y;
        public float Z;
        public float W;
        public Quaternion(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }
        public void Normalize()
        {
            float d = X * X + Y * Y + Z * Z + W * W;
            if (d == 0)
            {
                X = 1;
                return;
            }
            d = (1.0f) / MathHelper.Sqrt(d);
            X *= d;
            Y *= d;
            Z *= d;
            W *= d;
        }
        public static Quaternion Normalize(Quaternion quat)
        {
            Quaternion q = quat;
            q.Normalize();
            return q;
        }
        public static Quaternion Slerp(float t, Quaternion q1, Quaternion q2)
        {
            float cosTheta = Dot(q1, q2);
            if (cosTheta > 0.9995f)
            {
                return Normalize((1.0f - t) * q1 + t * q2);
            }
            else
            {
                float theta = MathHelper.Acos(MathHelper.Clamp(cosTheta, -1.0f, 1.0f));
                float thetap = theta * t;
                Quaternion qperp = Normalize(q2 - q1 * cosTheta);
                return q1 * MathHelper.Cos(thetap) + qperp * MathHelper.Sin(thetap);
            }
        }
        public static float Dot(Quaternion q1, Quaternion q2)
        {
            return q1.X * q2.X + q1.Y * q2.Y + q1.Z * q2.Z + q1.W * q2.W;
        }
        public static Quaternion operator *(Quaternion q, Quaternion multiplier)
        {
            Quaternion rv = new Quaternion();
            rv.X = q.X * multiplier.X - q.Y * multiplier.Y - q.Z * multiplier.Z - q.W * multiplier.W;
            rv.Y = q.X * multiplier.Y + q.Y * multiplier.X + q.Z * multiplier.W - q.W * multiplier.Z;
            rv.Z = q.X * multiplier.Z + q.Z * multiplier.X + q.W * multiplier.Y - q.Y * multiplier.W;
            rv.W = q.X * multiplier.W + q.W * multiplier.X + q.Y * multiplier.Z - q.Z * multiplier.Y;
            return rv;
        }
        public static Quaternion operator *(Quaternion q, float m)
        {
            return new Quaternion(q.X * m, q.Y * m, q.Z * m, q.W * m);
        }
        public static Quaternion operator *(float m, Quaternion q)
        {
            return q * m;
        }
        public static Quaternion operator /(Quaternion q, float m)
        {
            return new Quaternion(q.X / m, q.Y / m, q.Z / m, q.W / m);
        }
        public static Quaternion operator +(Quaternion q1, Quaternion q2)
        {
            return new Quaternion(q1.X + q2.X, q1.Y + q2.Y, q1.Z + q2.Z, q1.W + q2.W);
        }
        public static Quaternion operator -(Quaternion q1, Quaternion q2)
        {
            return new Quaternion(q1.X - q2.X, q1.Y - q2.Y, q1.Z - q2.Z, q1.W - q2.W);
        }
        public static Quaternion CreateFromAxisAngle(Vector3 axis, float angle)
        {
            float num = angle * 0.5f;
            float num2 = MathHelper.Sin(num);
            float w = MathHelper.Cos(num);
            Quaternion result;
            result.X = axis.X * num2;
            result.Y = axis.Y * num2;
            result.Z = axis.Z * num2;
            result.W = w;
            return result;
        }
        public void RotateByVector(Vector3 vector)
        {
            Quaternion q = new Quaternion(0, vector.X, vector.Y, vector.Z);
            Assignthis(this * q);
        }
        public void AddScaledVector(Vector3 vector, float scale)
        {
            Quaternion q = new Quaternion(0, vector.X * scale, vector.Y * scale, vector.Z * scale);
            q *= this;
            X += q.X * (0.5f);
            Y += q.Y * (0.5f);
            Z += q.Z * (0.5f);
            W += q.W * (0.5f);
        }
        public void Assignthis(Quaternion quat)
        {
            X = quat.X;
            Y = quat.Y;
            Z = quat.Z;
            W = quat.W;
        }
        public override string ToString()
        {
            return X + "," + Y + "," + Z + "," + W;
        }
        public static Quaternion Identity = new Quaternion(1, 0, 0, 0);
    }
}
