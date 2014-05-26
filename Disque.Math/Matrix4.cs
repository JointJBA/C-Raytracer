using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Disque.Math
{
    public struct Matrix4
    {
        float[] data;
        public Matrix4(float[] data)
        {
            this.data = data;
        }
        public static Matrix4 operator *(Matrix4 m, Matrix4 o)
        {
            float[] data = m.data;
            Matrix4 result = Matrix4.Identity;
            result.data = new float[12];
            result.data[0] = (o.data[0] * data[0]) + (o.data[4] * data[1]) +
            (o.data[8] * data[2]);
            result.data[4] = (o.data[0] * data[4]) + (o.data[4] * data[5]) +
            (o.data[8] * data[6]);
            result.data[8] = (o.data[0] * data[8]) + (o.data[4] * data[9]) +
            (o.data[8] * data[10]);
            result.data[1] = (o.data[1] * data[0]) + (o.data[5] * data[1]) +
            (o.data[9] * data[2]);
            result.data[5] = (o.data[1] * data[4]) + (o.data[5] * data[5]) +
            (o.data[9] * data[6]);
            result.data[9] = (o.data[1] * data[8]) + (o.data[5] * data[9]) +
            (o.data[9] * data[10]);
            result.data[2] = (o.data[2] * data[0]) + (o.data[6] * data[1]) +
            (o.data[10] * data[2]);
            result.data[6] = (o.data[2] * data[4]) + (o.data[6] * data[5]) +
            (o.data[10] * data[6]);
            result.data[10] = (o.data[2] * data[8]) + (o.data[6] * data[9]) +
            (o.data[10] * data[10]);
            result.data[3] = (o.data[3] * data[0]) + (o.data[7] * data[1]) +
            (o.data[11] * data[2]) + data[3];
            result.data[7] = (o.data[3] * data[4]) + (o.data[7] * data[5]) +
            (o.data[11] * data[6]) + data[7];
            result.data[11] = (o.data[3] * data[8]) + (o.data[7] * data[9]) +
            (o.data[11] * data[10]) + data[11];
            return result;
        }
        public float GetDeterminant()
        {
            return data[8] * data[5] * data[2] + data[4] * data[9] * data[2] +
                data[8] * data[1] * data[6] -
                data[0] * data[9] * data[6] -
                data[4] * data[1] * data[10] +
                data[0] * data[5] * data[10];
        }
        public void SetInverse(Matrix4 m)
        {
            float det = GetDeterminant();
            if (det == 0) return;
            det = (1.0f) / det;

            data[0] = (-m.data[9] * m.data[6] + m.data[5] * m.data[10]) * det;
            data[4] = (m.data[8] * m.data[6] - m.data[4] * m.data[10]) * det;
            data[8] = (-m.data[8] * m.data[5] + m.data[4] * m.data[9]) * det;

            data[1] = (m.data[9] * m.data[2] - m.data[1] * m.data[10]) * det;
            data[5] = (-m.data[8] * m.data[2] + m.data[0] * m.data[10]) * det;
            data[9] = (m.data[8] * m.data[1] - m.data[0] * m.data[9]) * det;

            data[2] = (-m.data[5] * m.data[2] + m.data[1] * m.data[6]) * det;
            data[6] = (+m.data[4] * m.data[2] - m.data[0] * m.data[6]) * det;
            data[10] = (-m.data[4] * m.data[1] + m.data[0] * m.data[5]) * det;

            data[3] = (m.data[9] * m.data[6] * m.data[3]
                       - m.data[5] * m.data[10] * m.data[3]
                       - m.data[9] * m.data[2] * m.data[7]
                       + m.data[1] * m.data[10] * m.data[7]
                       + m.data[5] * m.data[2] * m.data[11]
                       - m.data[1] * m.data[6] * m.data[11]) * det;
            data[7] = (-m.data[8] * m.data[6] * m.data[3]
                       + m.data[4] * m.data[10] * m.data[3]
                       + m.data[8] * m.data[2] * m.data[7]
                       - m.data[0] * m.data[10] * m.data[7]
                       - m.data[4] * m.data[2] * m.data[11]
                       + m.data[0] * m.data[6] * m.data[11]) * det;
            data[11] = (m.data[8] * m.data[5] * m.data[3]
                       - m.data[4] * m.data[9] * m.data[3]
                       - m.data[8] * m.data[1] * m.data[7]
                       + m.data[0] * m.data[9] * m.data[7]
                       + m.data[4] * m.data[1] * m.data[11]
                       - m.data[0] * m.data[5] * m.data[11]) * det;
        }
        public Matrix4 Inverse()
        {
            Matrix4 result = Matrix4.Identity;
            result.SetInverse(this);
            return result;
        }
        public void Invert()
        {
            SetInverse(this);
        }
        public static Matrix4 Inverse(Matrix4 m)
        {
            Matrix4 result = Matrix4.Identity;
            result.SetInverse(m);
            return result;
        }
        public void SetOrientationAndPos(Quaternion q, Vector3 pos)
        {
            data[0] = 1 - (2 * q.Z * q.Z + 2 * q.W * q.W);
            data[1] = 2 * q.Y * q.Z + 2 * q.W * q.X;
            data[2] = 2 * q.Y * q.W - 2 * q.Z * q.X;
            data[3] = pos.X;
            data[4] = 2 * q.Y * q.Z - 2 * q.W * q.X;
            data[5] = 1 - (2 * q.Y * q.Y + 2 * q.W * q.W);
            data[6] = 2 * q.Z * q.W + 2 * q.Y * q.X;
            data[7] = pos.Y;
            data[8] = 2 * q.Y * q.W + 2 * q.Z * q.X;
            data[9] = 2 * q.Z * q.W - 2 * q.Y * q.X;
            data[10] = 1 - (2 * q.Y * q.Y + 2 * q.Z * q.Z);
            data[11] = pos.Z;
        }
        public static Matrix4 SetOrientationAndPosition(Quaternion q, Vector3 pos)
        {
            float[] data = new float[12];
            data[0] = 1 - (2 * q.Z * q.Z + 2 * q.W * q.W);
            data[1] = 2 * q.Y * q.Z + 2 * q.W * q.X;
            data[2] = 2 * q.Y * q.W - 2 * q.Z * q.X;
            data[3] = pos.X;
            data[4] = 2 * q.Y * q.Z - 2 * q.W * q.X;
            data[5] = 1 - (2 * q.Y * q.Y + 2 * q.W * q.W);
            data[6] = 2 * q.Z * q.W + 2 * q.Y * q.X;
            data[7] = pos.Y;
            data[8] = 2 * q.Y * q.W + 2 * q.Z * q.X;
            data[9] = 2 * q.Z * q.W - 2 * q.Y * q.X;
            data[10] = 1 - (2 * q.Y * q.Y + 2 * q.Z * q.Z);
            data[11] = pos.Z;
            return new Matrix4(data);
        }
        public Vector3 Transform(Vector3 vector)
        {
            return Vector3.Transform(vector, this);
        }
        public Vector3 TransformInverse(Vector3 vector)
        {
            Vector3 tmp = vector;
            tmp.X -= data[3];
            tmp.Y -= data[7];
            tmp.Z -= data[11];
            return new Vector3(
            tmp.X * data[0] +
            tmp.Y * data[4] +
            tmp.Z * data[8],
            tmp.X * data[1] +
            tmp.Y * data[5] +
            tmp.Z * data[9],
            tmp.X * data[2] +
            tmp.Y * data[6] +
            tmp.Z * data[10]
            );
        }
        public Vector3 TransformDirection(Vector3 vector)
        {
            return new Vector3(
            vector.X * data[0] +
            vector.Y * data[1] +
            vector.Z * data[2],
            vector.X * data[4] +
            vector.Y * data[5] +
            vector.Z * data[6],
            vector.X * data[8] +
            vector.Y * data[9] +
            vector.Z * data[10]
            );
        }
        public Vector3 TransformInverseDirection(Vector3 vector)
        {
            return new Vector3(
            vector.X * data[0] +
            vector.Y * data[4] +
            vector.Z * data[8],
            vector.X * data[1] +
            vector.Y * data[5] +
            vector.Z * data[9],
            vector.X * data[2] +
            vector.Y * data[6] +
            vector.Z * data[10]
            );
        }
        public Vector3 GetAxisVector(int i)
        {
            return new Vector3(data[i], data[i + 4], data[i + 8]);
        }
        public static Matrix4 CreateFromTranslation(Vector3 trans)
        {
            return new Matrix4(new float[12]
            {
                1,0,0,trans.X,
                0,1,0,trans.Y,
                0,0,1,trans.Z
            });
        }
        public static Matrix4 CreateFromScale(Vector3 scale)
        {
            Matrix4 ret = new Matrix4(new float[12]
            {
                scale.X,0,0,0,
                0,scale.Y,0,0,
                0,0,scale.Z,0
            });
            return ret;
        }
        public static Matrix4 CreateRotation(Vector3 axis, float radian)
        {
            float x = axis.X;
            float y = axis.Y;
            float z = axis.Z;
            float num = MathHelper.Sin(radian);
            float num2 = MathHelper.Cos(radian);
            float num3 = x * x;
            float num4 = y * y;
            float num5 = z * z;
            float num6 = x * y;
            float num7 = x * z;
            float num8 = y * z;
            Matrix4 result = Matrix4.Identity;
            result[0] = num3 + num2 * (1f - num3);
            result[1] = num6 - num2 * num6 + num * z;
            result[2] = num7 - num2 * num7 - num * y;
            result[3] = 0f;
            result[4] = num6 - num2 * num6 - num * z;
            result[5] = num4 + num2 * (1f - num4);
            result[6] = num8 - num2 * num8 + num * x;
            result[7] = 0f;
            result[8] = num7 - num2 * num7 + num * y;
            result[9] = num8 - num2 * num8 - num * x;
            result[10] = num5 + num2 * (1f - num5);
            result[11] = 0f;
            return result;
        }
        public float this[int i]
        {
            get
            {
                if (data == null) data = new float[12];
                return data[i];
            }
            set
            {
                if (data == null) data = new float[12];
                data[i] = value;
            }
        }
        public static implicit operator float[](Matrix4 matrix)
        {
            return matrix.data;
        }
        public override string ToString()
        {
            return "{" +
                data[0] + "," + data[1] + "," + data[2] + "," + data[3] + "\n"+
                data[4] + "," + data[5] + "," + data[6] + "," + data[7] + "\n"+
                data[8] + "," + data[9] + "," + data[10] + "," + data[11] + "\n"+
                "0,0,0,1}"
                ;
        }
        public static readonly Matrix4 Identity = new Matrix4(new float[12] 
        {
        1,0,0,0,
        0,1,0,0,
        0,0,1,0});
    }
}
