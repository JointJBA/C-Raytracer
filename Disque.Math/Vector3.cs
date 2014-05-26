using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Disque.Math
{
    public struct Vector3
    {
        public static readonly Vector3 SmallGravity = new Vector3(0, -9.81f, 0);
        public static readonly Vector3 BigGravity = new Vector3(0, -20.0f, 0);
        public static readonly Vector3 Up = new Vector3(0, 1, 0);
        public static readonly Vector3 Down = new Vector3(0, -1, 0);
        public static readonly Vector3 Right = new Vector3(1, 0, 0);
        public static readonly Vector3 Left = new Vector3(-1, 0, 0);
        public static readonly Vector3 Zero = new Vector3(0, 0, 0);
        public static readonly Vector3 One = new Vector3(1, 1, 1);
        public float X;
        public float Y;
        public float Z;
        public float Magnitude
        {
            get
            {
                return MathHelper.Sqrt(X * X + Y * Y + Z * Z);
            }
        }
        public float SquaredMagnitude
        {
            get
            {
                return (X * X + Y * Y + Z * Z);
            }
        }
        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        public Vector3(float value)
        {
            X = Y = Z = value;
        }
        public Vector3(double x, double y, double z)
        {
            X = (float)x;
            Y = (float)y;
            Z = (float)z;
        }
        public Vector3(double value)
        {
            X = Y = Z = (float)value;
        }
        public float Dot(Vector3 v)
        {
            return X * v.X + Y * v.Y + Z * v.Z;
        }
        public float AbsDot(Vector3 v)
        {
            return MathHelper.Abs(Dot(v));
        }
        public static float AbsDot(Vector3 v1, Vector3 v2)
        {
            return MathHelper.Abs(Dot(v1, v2));
        }
        public Vector3 Cross(Vector3 vector)
        {
            return new Vector3(Y * vector.Z - Z * vector.Y, Z * vector.X - X * vector.Z, X * vector.Y - Y * vector.X);
        }
        public void Invert()
        {
            X = -X;
            Y = -Y;
            Z = -Z;
        }
        public static float Dot(Vector3 vector1, Vector3 vector2)
        {
            return vector1.X * vector2.X + vector1.Y * vector2.Y + vector1.Z * vector2.Z;
        }
        public void Normalize()
        {
            float num = this.X * this.X + this.Y * this.Y + this.Z * this.Z;
            float num2 = 1f / MathHelper.Sqrt(num);
            this.X *= num2;
            this.Y *= num2;
            this.Z *= num2;
        }
        public static Vector3 Normalize(Vector3 value)
        {
            float num = value.X * value.X + value.Y * value.Y + value.Z * value.Z;
            float num2 = 1f / MathHelper.Sqrt(num);
            Vector3 result;
            result.X = value.X * num2;
            result.Y = value.Y * num2;
            result.Z = value.Z * num2;
            return result;
        }
        public static void Normalize(ref Vector3 value, out Vector3 result)
        {
            float num = value.X * value.X + value.Y * value.Y + value.Z * value.Z;
            float num2 = 1f / MathHelper.Sqrt(num);
            result.X = value.X * num2;
            result.Y = value.Y * num2;
            result.Z = value.Z * num2;
        }
        public static Vector3 Cross(Vector3 vector1, Vector3 vector2)
        {
            Vector3 result;
            result.X = vector1.Y * vector2.Z - vector1.Z * vector2.Y;
            result.Y = vector1.Z * vector2.X - vector1.X * vector2.Z;
            result.Z = vector1.X * vector2.Y - vector1.Y * vector2.X;
            return result;
        }
        public static void Cross(ref Vector3 vector1, ref Vector3 vector2, out Vector3 result)
        {
            float x = vector1.Y * vector2.Z - vector1.Z * vector2.Y;
            float y = vector1.Z * vector2.X - vector1.X * vector2.Z;
            float z = vector1.X * vector2.Y - vector1.Y * vector2.X;
            result.X = x;
            result.Y = y;
            result.Z = z;
        }
        public static Vector3 Reflect(Vector3 vector, Vector3 normal)
        {
            float num = vector.X * normal.X + vector.Y * normal.Y + vector.Z * normal.Z;
            Vector3 result;
            result.X = vector.X - 2f * num * normal.X;
            result.Y = vector.Y - 2f * num * normal.Y;
            result.Z = vector.Z - 2f * num * normal.Z;
            return result;
        }
        public static void Reflect(ref Vector3 vector, ref Vector3 normal, out Vector3 result)
        {
            float num = vector.X * normal.X + vector.Y * normal.Y + vector.Z * normal.Z;
            result.X = vector.X - 2f * num * normal.X;
            result.Y = vector.Y - 2f * num * normal.Y;
            result.Z = vector.Z - 2f * num * normal.Z;
        }
        public static Vector3 Min(Vector3 value1, Vector3 value2)
        {
            Vector3 result;
            result.X = ((value1.X < value2.X) ? value1.X : value2.X);
            result.Y = ((value1.Y < value2.Y) ? value1.Y : value2.Y);
            result.Z = ((value1.Z < value2.Z) ? value1.Z : value2.Z);
            return result;
        }
        public static void Min(ref Vector3 value1, ref Vector3 value2, out Vector3 result)
        {
            result.X = ((value1.X < value2.X) ? value1.X : value2.X);
            result.Y = ((value1.Y < value2.Y) ? value1.Y : value2.Y);
            result.Z = ((value1.Z < value2.Z) ? value1.Z : value2.Z);
        }
        public static Vector3 Max(Vector3 value1, Vector3 value2)
        {
            Vector3 result;
            result.X = ((value1.X > value2.X) ? value1.X : value2.X);
            result.Y = ((value1.Y > value2.Y) ? value1.Y : value2.Y);
            result.Z = ((value1.Z > value2.Z) ? value1.Z : value2.Z);
            return result;
        }
        public static void Max(ref Vector3 value1, ref Vector3 value2, out Vector3 result)
        {
            result.X = ((value1.X > value2.X) ? value1.X : value2.X);
            result.Y = ((value1.Y > value2.Y) ? value1.Y : value2.Y);
            result.Z = ((value1.Z > value2.Z) ? value1.Z : value2.Z);
        }
        public static Vector3 Clamp(Vector3 value1, Vector3 min, Vector3 max)
        {
            float num = value1.X;
            num = ((num > max.X) ? max.X : num);
            num = ((num < min.X) ? min.X : num);
            float num2 = value1.Y;
            num2 = ((num2 > max.Y) ? max.Y : num2);
            num2 = ((num2 < min.Y) ? min.Y : num2);
            float num3 = value1.Z;
            num3 = ((num3 > max.Z) ? max.Z : num3);
            num3 = ((num3 < min.Z) ? min.Z : num3);
            Vector3 result;
            result.X = num;
            result.Y = num2;
            result.Z = num3;
            return result;
        }
        public static void Clamp(ref Vector3 value1, ref Vector3 min, ref Vector3 max, out Vector3 result)
        {
            float num = value1.X;
            num = ((num > max.X) ? max.X : num);
            num = ((num < min.X) ? min.X : num);
            float num2 = value1.Y;
            num2 = ((num2 > max.Y) ? max.Y : num2);
            num2 = ((num2 < min.Y) ? min.Y : num2);
            float num3 = value1.Z;
            num3 = ((num3 > max.Z) ? max.Z : num3);
            num3 = ((num3 < min.Z) ? min.Z : num3);
            result.X = num;
            result.Y = num2;
            result.Z = num3;
        }
        public static Vector3 Lerp(Vector3 value1, Vector3 value2, float amount)
        {
            Vector3 result;
            result.X = value1.X + (value2.X - value1.X) * amount;
            result.Y = value1.Y + (value2.Y - value1.Y) * amount;
            result.Z = value1.Z + (value2.Z - value1.Z) * amount;
            return result;
        }
        public static void Lerp(ref Vector3 value1, ref Vector3 value2, float amount, out Vector3 result)
        {
            result.X = value1.X + (value2.X - value1.X) * amount;
            result.Y = value1.Y + (value2.Y - value1.Y) * amount;
            result.Z = value1.Z + (value2.Z - value1.Z) * amount;
        }
        public static Vector3 Barycentric(Vector3 value1, Vector3 value2, Vector3 value3, float amount1, float amount2)
        {
            Vector3 result;
            result.X = value1.X + amount1 * (value2.X - value1.X) + amount2 * (value3.X - value1.X);
            result.Y = value1.Y + amount1 * (value2.Y - value1.Y) + amount2 * (value3.Y - value1.Y);
            result.Z = value1.Z + amount1 * (value2.Z - value1.Z) + amount2 * (value3.Z - value1.Z);
            return result;
        }
        public static void Barycentric(ref Vector3 value1, ref Vector3 value2, ref Vector3 value3, float amount1, float amount2, out Vector3 result)
        {
            result.X = value1.X + amount1 * (value2.X - value1.X) + amount2 * (value3.X - value1.X);
            result.Y = value1.Y + amount1 * (value2.Y - value1.Y) + amount2 * (value3.Y - value1.Y);
            result.Z = value1.Z + amount1 * (value2.Z - value1.Z) + amount2 * (value3.Z - value1.Z);
        }
        public static Vector3 SmoothStep(Vector3 value1, Vector3 value2, float amount)
        {
            amount = ((amount > 1f) ? 1f : ((amount < 0f) ? 0f : amount));
            amount = amount * amount * (3f - 2f * amount);
            Vector3 result;
            result.X = value1.X + (value2.X - value1.X) * amount;
            result.Y = value1.Y + (value2.Y - value1.Y) * amount;
            result.Z = value1.Z + (value2.Z - value1.Z) * amount;
            return result;
        }
        public static void SmoothStep(ref Vector3 value1, ref Vector3 value2, float amount, out Vector3 result)
        {
            amount = ((amount > 1f) ? 1f : ((amount < 0f) ? 0f : amount));
            amount = amount * amount * (3f - 2f * amount);
            result.X = value1.X + (value2.X - value1.X) * amount;
            result.Y = value1.Y + (value2.Y - value1.Y) * amount;
            result.Z = value1.Z + (value2.Z - value1.Z) * amount;
        }
        public static Vector3 CatmullRom(Vector3 value1, Vector3 value2, Vector3 value3, Vector3 value4, float amount)
        {
            float num = amount * amount;
            float num2 = amount * num;
            Vector3 result;
            result.X = 0.5f * (2f * value2.X + (-value1.X + value3.X) * amount + (2f * value1.X - 5f * value2.X + 4f * value3.X - value4.X) * num + (-value1.X + 3f * value2.X - 3f * value3.X + value4.X) * num2);
            result.Y = 0.5f * (2f * value2.Y + (-value1.Y + value3.Y) * amount + (2f * value1.Y - 5f * value2.Y + 4f * value3.Y - value4.Y) * num + (-value1.Y + 3f * value2.Y - 3f * value3.Y + value4.Y) * num2);
            result.Z = 0.5f * (2f * value2.Z + (-value1.Z + value3.Z) * amount + (2f * value1.Z - 5f * value2.Z + 4f * value3.Z - value4.Z) * num + (-value1.Z + 3f * value2.Z - 3f * value3.Z + value4.Z) * num2);
            return result;
        }
        public static void CatmullRom(ref Vector3 value1, ref Vector3 value2, ref Vector3 value3, ref Vector3 value4, float amount, out Vector3 result)
        {
            float num = amount * amount;
            float num2 = amount * num;
            result.X = 0.5f * (2f * value2.X + (-value1.X + value3.X) * amount + (2f * value1.X - 5f * value2.X + 4f * value3.X - value4.X) * num + (-value1.X + 3f * value2.X - 3f * value3.X + value4.X) * num2);
            result.Y = 0.5f * (2f * value2.Y + (-value1.Y + value3.Y) * amount + (2f * value1.Y - 5f * value2.Y + 4f * value3.Y - value4.Y) * num + (-value1.Y + 3f * value2.Y - 3f * value3.Y + value4.Y) * num2);
            result.Z = 0.5f * (2f * value2.Z + (-value1.Z + value3.Z) * amount + (2f * value1.Z - 5f * value2.Z + 4f * value3.Z - value4.Z) * num + (-value1.Z + 3f * value2.Z - 3f * value3.Z + value4.Z) * num2);
        }
        public static Vector3 Hermite(Vector3 value1, Vector3 tangent1, Vector3 value2, Vector3 tangent2, float amount)
        {
            float num = amount * amount;
            float num2 = amount * num;
            float num3 = 2f * num2 - 3f * num + 1f;
            float num4 = -2f * num2 + 3f * num;
            float num5 = num2 - 2f * num + amount;
            float num6 = num2 - num;
            Vector3 result;
            result.X = value1.X * num3 + value2.X * num4 + tangent1.X * num5 + tangent2.X * num6;
            result.Y = value1.Y * num3 + value2.Y * num4 + tangent1.Y * num5 + tangent2.Y * num6;
            result.Z = value1.Z * num3 + value2.Z * num4 + tangent1.Z * num5 + tangent2.Z * num6;
            return result;
        }
        public static void Hermite(ref Vector3 value1, ref Vector3 tangent1, ref Vector3 value2, ref Vector3 tangent2, float amount, out Vector3 result)
        {
            float num = amount * amount;
            float num2 = amount * num;
            float num3 = 2f * num2 - 3f * num + 1f;
            float num4 = -2f * num2 + 3f * num;
            float num5 = num2 - 2f * num + amount;
            float num6 = num2 - num;
            result.X = value1.X * num3 + value2.X * num4 + tangent1.X * num5 + tangent2.X * num6;
            result.Y = value1.Y * num3 + value2.Y * num4 + tangent1.Y * num5 + tangent2.Y * num6;
            result.Z = value1.Z * num3 + value2.Z * num4 + tangent1.Z * num5 + tangent2.Z * num6;
        }
        public static Vector3 Negate(Vector3 value)
        {
            Vector3 result;
            result.X = -value.X;
            result.Y = -value.Y;
            result.Z = -value.Z;
            return result;
        }
        public static void Negate(ref Vector3 value, out Vector3 result)
        {
            result.X = -value.X;
            result.Y = -value.Y;
            result.Z = -value.Z;
        }
        public static Vector3 Add(Vector3 value1, Vector3 value2)
        {
            Vector3 result;
            result.X = value1.X + value2.X;
            result.Y = value1.Y + value2.Y;
            result.Z = value1.Z + value2.Z;
            return result;
        }
        public static void Add(ref Vector3 value1, ref Vector3 value2, out Vector3 result)
        {
            result.X = value1.X + value2.X;
            result.Y = value1.Y + value2.Y;
            result.Z = value1.Z + value2.Z;
        }
        public static Vector3 Subtract(Vector3 value1, Vector3 value2)
        {
            Vector3 result;
            result.X = value1.X - value2.X;
            result.Y = value1.Y - value2.Y;
            result.Z = value1.Z - value2.Z;
            return result;
        }
        public static void Subtract(ref Vector3 value1, ref Vector3 value2, out Vector3 result)
        {
            result.X = value1.X - value2.X;
            result.Y = value1.Y - value2.Y;
            result.Z = value1.Z - value2.Z;
        }
        public static Vector3 Multiply(Vector3 value1, Vector3 value2)
        {
            Vector3 result;
            result.X = value1.X * value2.X;
            result.Y = value1.Y * value2.Y;
            result.Z = value1.Z * value2.Z;
            return result;
        }
        public static void Multiply(ref Vector3 value1, ref Vector3 value2, out Vector3 result)
        {
            result.X = value1.X * value2.X;
            result.Y = value1.Y * value2.Y;
            result.Z = value1.Z * value2.Z;
        }
        public static Vector3 Multiply(Vector3 value1, float scaleFactor)
        {
            Vector3 result;
            result.X = value1.X * scaleFactor;
            result.Y = value1.Y * scaleFactor;
            result.Z = value1.Z * scaleFactor;
            return result;
        }
        public static void Multiply(ref Vector3 value1, float scaleFactor, out Vector3 result)
        {
            result.X = value1.X * scaleFactor;
            result.Y = value1.Y * scaleFactor;
            result.Z = value1.Z * scaleFactor;
        }
        public static Vector3 Divide(Vector3 value1, Vector3 value2)
        {
            Vector3 result;
            result.X = value1.X / value2.X;
            result.Y = value1.Y / value2.Y;
            result.Z = value1.Z / value2.Z;
            return result;
        }
        public static void Divide(ref Vector3 value1, ref Vector3 value2, out Vector3 result)
        {
            result.X = value1.X / value2.X;
            result.Y = value1.Y / value2.Y;
            result.Z = value1.Z / value2.Z;
        }
        public static Vector3 Divide(Vector3 value1, float value2)
        {
            float num = 1f / value2;
            Vector3 result;
            result.X = value1.X * num;
            result.Y = value1.Y * num;
            result.Z = value1.Z * num;
            return result;
        }
        public static void Divide(ref Vector3 value1, float value2, out Vector3 result)
        {
            float num = 1f / value2;
            result.X = value1.X * num;
            result.Y = value1.Y * num;
            result.Z = value1.Z * num;
        }
        public static Vector3 operator -(Vector3 value)
        {
            Vector3 result;
            result.X = -value.X;
            result.Y = -value.Y;
            result.Z = -value.Z;
            return result;
        }
        public static bool operator ==(Vector3 value1, Vector3 value2)
        {
            return value1.X == value2.X && value1.Y == value2.Y && value1.Z == value2.Z;
        }
        public static bool operator !=(Vector3 value1, Vector3 value2)
        {
            return value1.X != value2.X || value1.Y != value2.Y || value1.Z != value2.Z;
        }
        public static Vector3 operator +(Vector3 value1, Vector3 value2)
        {
            Vector3 result;
            result.X = value1.X + value2.X;
            result.Y = value1.Y + value2.Y;
            result.Z = value1.Z + value2.Z;
            return result;
        }
        public static Vector3 operator -(Vector3 value1, Vector3 value2)
        {
            Vector3 result;
            result.X = value1.X - value2.X;
            result.Y = value1.Y - value2.Y;
            result.Z = value1.Z - value2.Z;
            return result;
        }
        public static Vector3 operator *(Vector3 value1, Vector3 value2)
        {
            Vector3 result;
            result.X = value1.X * value2.X;
            result.Y = value1.Y * value2.Y;
            result.Z = value1.Z * value2.Z;
            return result;
        }
        public static Vector3 operator *(Vector3 value, float scaleFactor)
        {
            Vector3 result;
            result.X = value.X * scaleFactor;
            result.Y = value.Y * scaleFactor;
            result.Z = value.Z * scaleFactor;
            return result;
        }
        public static Vector3 operator *(float scaleFactor, Vector3 value)
        {
            Vector3 result;
            result.X = value.X * scaleFactor;
            result.Y = value.Y * scaleFactor;
            result.Z = value.Z * scaleFactor;
            return result;
        }
        public static Vector3 operator /(Vector3 value1, Vector3 value2)
        {
            Vector3 result;
            result.X = value1.X / value2.X;
            result.Y = value1.Y / value2.Y;
            result.Z = value1.Z / value2.Z;
            return result;
        }
        public static Vector3 operator /(Vector3 value, float divider)
        {
            float num = 1f / divider;
            Vector3 result;
            result.X = value.X * num;
            result.Y = value.Y * num;
            result.Z = value.Z * num;
            return result;
        }
        public static Vector3 LocalToWorld(Vector3 local, Matrix4 transform)
        {
            return Vector3.Transform((local), transform);
        }
        public static Vector3 WorldToLocal(Vector3 world, Matrix4 transform)
        {
            return transform.TransformInverse(world);
        }
        public static Vector3 LocalToWorldDirn(Vector3 local, Matrix4 transform)
        {
            return transform.TransformDirection(local);
        }
        public static Vector3 WorldToLocalDirn(Vector3 world, Matrix4 transform)
        {
            return transform.TransformInverseDirection(world);
        }
        public static Vector3 Transform(Vector3 vector, Matrix4 matrix)
        {
            float[] data = matrix;
            return new Vector3(
                vector.X * data[0] + vector.Y * data[1] + vector.Z * data[2] + data[3],
                vector.X * data[4] + vector.Y * data[5] + vector.Z * data[6] + data[7],
                vector.X * data[8] + vector.Y * data[9] + vector.Z * data[10] + data[11]);
        }
        public static Vector3 TransformDirection(Vector3 vector, Matrix4 matrix)
        {
            return matrix.TransformDirection(vector);
        }
        public static Vector3 Transform(Vector3 vector, Matrix3 matrix)
        {
            float[] data = matrix;
            return new Vector3(
                vector.X * data[0] + vector.Y * data[1] + vector.Z * data[2],
                vector.X * data[3] + vector.Y * data[4] + vector.Z * data[5],
                vector.X * data[6] + vector.Y * data[7] + vector.Z * data[8]
            );
        }
        public void Clear()
        {
            X = Y = Z = 0;
        }
        public float this[int i]
        {
            get
            {
                if (i == 0) return X;
                if (i == 1) return Y;
                return Z;
            }
            set
            {
                if (i == 0) X = value;
                if (i == 1) Y = value;
                if (i == 2) Z = value;
            }
        }
        public override string ToString()
        {
            return "{" + X + "," + Y + "," + Z + "}";
        }
        public static float Distance(Vector3 v1, Vector3 v2)
        {
            return (v2 - v1).Magnitude;
        }
        public static float DistanceSquared(Vector3 v1, Vector3 v2)
        {
            return (v2 - v1).SquaredMagnitude;
        }
        public Vector3 Pow(float t)
        {
            return new Vector3(MathHelper.Pow(X, t), MathHelper.Pow(Y, t), MathHelper.Pow(Z, t));
        }
    }
}
