using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Disque.Math
{
    public struct Matrix2
    {
        float[,] data;
        public Matrix2(float[,] d)
        {
            data = d;
        }
        public static Matrix2 CreateMatrixFromRotation(float rad)
        {
            return new Matrix2(new float[2, 2] 
            {
                { MathHelper.Cos(rad), -MathHelper.Sin(rad) },
                { MathHelper.Sin(rad), MathHelper.Cos(rad) }
            }
            );
        }
        public static Matrix2 operator +(Matrix2 a, Matrix2 b)
        {
            float[,] result = new float[2, 2];
            for (int r = 0; r < 2; r++)
                for (int c = 0; c < 2; c++)
                    result[r, c] = a.data[r, c] + b.data[r, c];
            return new Matrix2(result);
        }
        public static Matrix2 operator -(Matrix2 a, Matrix2 b)
        {
            float[,] result = new float[2, 2];
            for (int r = 0; r < 2; r++)
                for (int c = 0; c < 2; c++)
                    result[r, c] = a.data[r, c] - b.data[r, c];
            return new Matrix2(result);
        }
        public static Matrix2 operator *(float s, Matrix2 m)
        {
            float[,] result = new float[2, 2];
            for (int r = 0; r < 2; r++)
                for (int c = 0; c < 2; c++)
                    result[r, c] = m.data[r, c] * s;
            return new Matrix2(result);
        }
        public static Matrix2 operator *(Matrix2 m, float s)
        {
            return s * m;
        }
        public static Matrix2 operator *(Matrix2 a, Matrix2 b)
        {
            float[,] result = 
            {
                {a.data[0, 0] * b.data[0, 0] + a.data[0, 1] * b.data[1, 0], a.data[0, 0] * b.data[0, 1] + a.data[0, 1] * b.data[1, 1]},
                {a.data[1, 0] * b.data[0, 0] + a.data[1, 1] * b.data[1, 0], a.data[1, 0] * b.data[0, 1] + a.data[1, 1] * b.data[1, 1]}
            };
            return new Matrix2(result);
        }
        public float this[int r, int c]
        {
            get
            {
                return data[r, c];
            }
            set
            {
                data[r, c] = value;
            }
        }
        public override string ToString()
        {
            return "[" + data[0, 0] + ", " + data[0, 1] + "]\n" + "[" + data[1, 0] + ", " + data[1, 1] + "]";
        }
    }
}
