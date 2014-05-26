using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Disque.Math
{
    public struct Vector2
    {
        public static Vector2 SmallGravity = new Vector2(0, -9.81f);
        public static Vector2 BigGravity = new Vector2(0, -20.0f);
        public static Vector2 Up = new Vector2(0, 1);
        public static Vector2 Down = new Vector2(0, -1);
        public static Vector2 Right = new Vector2(1, 0);
        public static Vector2 Left = new Vector2(-1, 0);
        float _x;
        float _y;
        public float X
        {
            get { return _x; }
            set { _x = value; }
        }
        public float Y
        {
            get { return _y; }
            set { _y = value; }
        }
        public float Magnitude
        {
            get
            {
                return MathHelper.Sqrt(X * X + Y * Y);
            }
        }
        public float SquaredMagnitude
        {
            get
            {
                return (X * X + Y * Y);
            }
        }
        public Vector2(float x, float y)
        {
            _x = x;
            _y = y;
        }
        public Vector2(double x, double y)
        {
            _x = (float)x;
            _y = (float)y;
        }
        public float Dot(Vector2 v)
        {
            return X * v.X + Y * v.Y;
        }
        public void Invert()
        {
            X = -X;
            Y = -Y;
        }
        public void Normalize()
        {
            float l = Magnitude;
            if (l > 0)
            {
                this *= (1 / l);
            }
        }
        public static float Dot(Vector2 vector1, Vector2 vector2)
        {
            return vector1.Dot(vector2);
        }
        public static Vector2 Invert(Vector2 vector)
        {
            vector.Invert();
            return vector;
        }
        public static Vector2 Normalize(Vector2 vector)
        {
            vector.Normalize();
            return vector;
        }
        public static float GetDistance(Vector2 a, Vector2 b)
        {
            return MathHelper.Sqrt(MathHelper.Pow(b.X - a.X) + MathHelper.Pow(b.Y - a.Y));
        }
        public static Vector2 operator *(Vector2 v, float scale)
        {
            return new Vector2(v.X * scale, v.Y * scale);
        }
        public static Vector2 operator *(float scale, Vector2 v)
        {
            return v * scale;
        }
        public static Vector2 operator ^(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X * v2.X, v1.Y * v2.Y);
        }
        public static Vector2 operator +(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X + v2.X, v1.Y + v2.Y);
        }
        public static Vector2 operator -(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X - v2.X, v1.Y - v2.Y);
        }
        public static Vector2 Transform(Vector2 v, Matrix2 m)
        {
            return new Vector2(m[0, 0] * v.X + m[0, 1] * v.Y, m[1, 0] * v.X + m[1, 1] * v.Y);
        }
        public static Vector2 Clamp(Vector2 value1, Vector2 min, Vector2 max)
        {
            Vector2 Vector2 = new Vector2();
            float f3 = value1.X;
            f3 = f3 > max.X ? max.X : f3;
            f3 = f3 < min.X ? min.X : f3;
            float f2 = value1.Y;
            f2 = f2 > max.Y ? max.Y : f2;
            f2 = f2 < min.Y ? min.Y : f2;
            Vector2.X = f3;
            Vector2.Y = f2;
            return Vector2;
        }
        public float this[int i]
        {
            get
            {
                if (i == 0) return X;
                return Y;
            }
            set
            {
                if (i == 0) X = value;
                if (i == 1) Y = value;
            }
        }
        public override string ToString()
        {
            return "{" + _x + "," + _y + "}";
        }
    }
}
