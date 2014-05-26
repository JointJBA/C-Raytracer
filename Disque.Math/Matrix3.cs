using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Disque.Math
{
    public struct Matrix3
    {
        float[] data;
        public Matrix3(float[] data)
        {
            this.data = data;
        }
        public static Matrix3 operator *(Matrix3 m, Matrix3 o)
        {
            float[] data = m.data;
            return new Matrix3(new float[] {
                data[0]*o.data[0] + data[1]*o.data[3] + data[2]*o.data[6],
                data[0]*o.data[1] + data[1]*o.data[4] + data[2]*o.data[7],
                data[0]*o.data[2] + data[1]*o.data[5] + data[2]*o.data[8],
                data[3]*o.data[0] + data[4]*o.data[3] + data[5]*o.data[6],
                data[3]*o.data[1] + data[4]*o.data[4] + data[5]*o.data[7],
                data[3]*o.data[2] + data[4]*o.data[5] + data[5]*o.data[8],
                data[6]*o.data[0] + data[7]*o.data[3] + data[8]*o.data[6],
                data[6]*o.data[1] + data[7]*o.data[4] + data[8]*o.data[7],
                data[6]*o.data[2] + data[7]*o.data[5] + data[8]*o.data[8]}
);
        }
        public static Matrix3 operator*(Matrix3 m, float scalar)
        {
            float[] data = new float[9];
            data[0] *= scalar; data[1] *= scalar; data[2] *= scalar;
            data[3] *= scalar; data[4] *= scalar; data[5] *= scalar;
            data[6] *= scalar; data[7] *= scalar; data[8] *= scalar;
            return new Matrix3(data);
        }
        public static Matrix3 operator +(Matrix3 m1, Matrix3 o)
        {
            float[] data = m1.data;
            data[0] += o.data[0]; data[1] += o.data[1]; data[2] += o.data[2];
            data[3] += o.data[3]; data[4] += o.data[4]; data[5] += o.data[5];
            data[6] += o.data[6]; data[7] += o.data[7]; data[8] += o.data[8];
            return new Matrix3(data);
        }
        public float this[int i]
        {
            get
            {
                if (data == null) data = new float[9];
                return data[i];
            }
            set
            {
                if (data == null) data = new float[9];
                data[i] = value;
            }
        }
        public static Matrix3 Identity = new Matrix3(new float[] { 1, 0, 0, 0, 1, 0, 0, 0, 1 });
        public void SetInverse(Matrix3 m)
        {
            float t4 = m.data[0] * m.data[4];
            float t6 = m.data[0] * m.data[5];
            float t8 = m.data[1] * m.data[3];
            float t10 = m.data[2] * m.data[3];
            float t12 = m.data[1] * m.data[6];
            float t14 = m.data[2] * m.data[6];
            float t16 = (t4 * m.data[8] - t6 * m.data[7] - t8 * m.data[8] + t10 * m.data[7] + t12 * m.data[5] - t14 * m.data[4]);
            if (t16 == (float)0.0f) return;
            float t17 = 1.0f / t16;
            if (data == null) data = new float[9];
            data[0] = (m.data[4] * m.data[8] - m.data[5] * m.data[7]) * t17;
            data[1] = -(m.data[1] * m.data[8] - m.data[2] * m.data[7]) * t17;
            data[2] = (m.data[1] * m.data[5] - m.data[2] * m.data[4]) * t17;
            data[3] = -(m.data[3] * m.data[8] - m.data[5] * m.data[6]) * t17;
            data[4] = (m.data[0] * m.data[8] - t14) * t17;
            data[5] = -(t6 - t10) * t17;
            data[6] = (m.data[3] * m.data[7] - m.data[4] * m.data[6]) * t17;
            data[7] = -(m.data[0] * m.data[7] - t12) * t17;
            data[8] = (t4 - t8) * t17;
        }
        public Matrix3 Inverse()
        {
            Matrix3 result = Matrix3.Identity;
            result.SetInverse(this);
            return result;
        }
        public void Invert()
        {
            SetInverse(this);
        }
        public void SetTranspose(Matrix3 m)
        {
            data[0] = m.data[0];
            data[1] = m.data[3];
            data[2] = m.data[6];
            data[3] = m.data[1];
            data[4] = m.data[4];
            data[5] = m.data[7];
            data[6] = m.data[2];
            data[7] = m.data[5];
            data[8] = m.data[8];
        }
        public Matrix3 Transpose()
        {
            Matrix3 result = Matrix3.Identity;
            result.SetTranspose(this);
            return result;
        }
        public void SetOrientation(Quaternion q)
        {
            data[0] = 1 - (2 * q.Z * q.Z + 2 * q.W * q.W);
            data[1] = 2 * q.Y * q.Z + 2 * q.W * q.X;
            data[2] = 2 * q.Y * q.W - 2 * q.Z * q.X;
            data[3] = 2 * q.Y * q.Z - 2 * q.W * q.X;
            data[4] = 1 - (2 * q.Y * q.Y + 2 * q.W * q.W);
            data[5] = 2 * q.Z * q.W + 2 * q.Y * q.X;
            data[6] = 2 * q.Y * q.W + 2 * q.Z * q.X;
            data[7] = 2 * q.Z * q.W - 2 * q.Y * q.X;
            data[8] = 1 - (2 * q.Y * q.Y + 2 * q.Z * q.Z);
        }
        public static Matrix3 SetOrientation(Quaternion q, bool diff)
        {
            float[] data = new float[9];
            data[0] = 1.0f - (2.0f * q.Z * q.Z + 2.0f * q.W * q.W);
            data[1] = 2.0f * q.Y * q.Z + 2.0f * q.W * q.X;
            data[2] = 2.0f * q.Y * q.W - 2.0f * q.Z * q.X;
            data[3] = 2.0f * q.Y * q.Z - 2.0f * q.W * q.X;
            data[4] = 1.0f - (2.0f * q.Y * q.Y + 2.0f * q.W * q.W);
            data[5] = 2.0f * q.Z * q.W + 2.0f * q.Y * q.X;
            data[6] = 2.0f * q.Y * q.W + 2.0f * q.Z * q.X;
            data[7] = 2.0f * q.Z * q.W - 2.0f * q.Y * q.X;
            data[8] = 1.0f - (2.0f * q.Y * q.Y + 2.0f * q.Z * q.Z);
            return new Matrix3(data);
        }
        public static Matrix3 LinearInterpolate(Matrix3 a, Matrix3 b, float prop)
        {
            Matrix3 result = Matrix3.Identity;
            for (int i = 0; i < 9; i++)
            {
                result.data[i] = a.data[i] * (1 - prop) + b.data[i] * prop;
            }
            return result;
        }
        public void SetBlockInertiaTensor(Vector3 halfSizes, float mass)
        {
            Vector3 squares = halfSizes * halfSizes;
            SetInertiaTensorCoeffs(0.3f*mass*(squares.Y + squares.Z),
                0.3f*mass*(squares.X + squares.Z),
                0.3f*mass*(squares.X + squares.Y));
        }
        public Vector3 Transform(Vector3 vector)
        {
            return Vector3.Transform(vector, this);
        }
        public Vector3 TransformTranspose(Vector3 vector)
        {
            return new Vector3(
                vector.X * data[0] + vector.Y * data[3] + vector.Z * data[6],
                vector.X * data[1] + vector.Y * data[4] + vector.Z * data[7],
                vector.X * data[2] + vector.Y * data[5] + vector.Z * data[8]
            );
        }
        public void SetComponents(Vector3 compOne, Vector3 compTwo, Vector3 compThree)
        {
            data = new float[9];
            data[0] = compOne.X;
            data[1] = compTwo.X;
            data[2] = compThree.X;
            data[3] = compOne.Y;
            data[4] = compTwo.Y;
            data[5] = compThree.Y;
            data[6] = compOne.Z;
            data[7] = compTwo.Z;
            data[8] = compThree.Z;

        }
        public void SetInertiaTensorCoeffs(float ix, float iy, float iz, float ixy = 0, float ixz = 0, float iyz = 0)
        {
            if (data == null) data = new float[9];
            data[0] = ix;
            data[1] = data[3] = -ixy;
            data[2] = data[6] = -ixz;
            data[4] = iy;
            data[5] = data[7] = -iyz;
            data[8] = iz;
        }
        public static implicit operator float[](Matrix3 matrix)
        {
            return matrix.data;
        }
        public override string ToString()
        {
            return "{" + data[0] + "," + data[1] + "," + data[2] + "," + data[3] + "," + data[4] + "," + data[5] + "," + data[6] + "," + data[7] + "," + data[8] + "}";
        }
        public void SetSkewSymmetric(Vector3 vector)
        {
            data[0] = data[4] = data[8] = 0;
            data[1] = -vector.Z;
            data[2] = vector.Y;
            data[3] = vector.Z;
            data[5] = -vector.X;
            data[6] = -vector.Y;
            data[7] = vector.X;
        }
    }
}
