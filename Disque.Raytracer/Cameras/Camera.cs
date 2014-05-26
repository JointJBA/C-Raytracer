using System;
using System.Collections.Generic;
using System.Text;
using Disque.Raytracer.Worlds;
using Disque.Math;

namespace Disque.Raytracer.Cameras
{
    public class Camera
    {
        public Vector3 Position, Target, Up, U, V;
        protected Vector3 W;
        float RollAngle;
        public float ExposureTime;

        public Camera()
        {
            Position = new Vector3(0, 0, 500);
            Target = new Vector3(0, 0, 0);
            RollAngle = 0.0f;
            ExposureTime = 1.0f;
            Up = new Vector3(0, 1, 0);
            U = new Vector3(1, 0, 0);
            V = new Vector3(0, 1, 0);
            W = new Vector3(0, 0, 1);
        }

        public void ComputeUVW()
        {
            W = Position - Target;
            W.Normalize();
            U = Vector3.Cross(Up, W);
            U.Normalize();
            V = Vector3.Cross(W, U);
            if (Position.X == Target.X && Position.Z == Target.Z && Position.Y > Target.Y)
            {
                U = new Vector3(0, 0, 1);
                V = new Vector3(1, 0, 0);
                W = new Vector3(0, 1, 0);	
            }
            if (Position.X == Target.X && Position.Z == Target.Z && Position.Y < Target.Y)
            {
                U = new Vector3(1, 0, 0);
                V = new Vector3(0, 0, 1);
                W = new Vector3(0, -1, 0);	
            }
        }

        public void SetRollAngle(float a)
        {
            RollAngle = a;
            float ra = MathHelper.ToRadians(a);
            Matrix4 F, T, R, Ra;
            T = Matrix4.Identity;
            R = Matrix4.Identity;
            Ra = Matrix4.Identity;
            Vector3 q = Up;
            q.Normalize();
            Vector3 w = Position - Target;
            w.Normalize();
            Vector3 p = Vector3.Cross(w, q);
            p.Normalize();
            T[0, 3] = Target.X;
            T[1, 3] = Target.Y;
            T[2, 3] = Target.Z;
            R[0, 0] = q.X;
            R[1, 0] = q.Y;
            R[2, 0] = q.Z;
            R[0, 1] = p.X;
            R[1, 1] = p.Y;
            R[2, 1] = p.Z;
            R[0, 2] = w.X;
            R[1, 2] = w.Y;
            R[2, 2] = w.Z;
            Ra[0, 0] = MathHelper.Cos(ra);
            Ra[0, 1] = MathHelper.Sin(ra);
            Ra[1, 0] = -MathHelper.Sin(ra);
            Ra[1, 1] = MathHelper.Cos(ra);
            F = (T * R) * Ra;
            T[0, 3] = -Target.X;
            T[1, 3] = -Target.Y;
            T[2, 3] = -Target.Z;
            R[0, 0] = q.X;
            R[0, 1] = q.Y;
            R[0, 2] = q.Z;
            R[1, 0] = p.X;
            R[1, 1] = p.Y;
            R[1, 2] = p.Z;
            R[2, 0] = w.X;
            R[2, 1] = w.Y;
            R[2, 2] = w.Z;
            F = (F * R) * T;
            Up = (Vector3.Transform((Position + Up), F) - Position);
            Up.Normalize();
        }

        public virtual void RenderScene(World world) { }

        public virtual Vector3 RenderRay(World world, int c, int r, int n)
        {
            return new Vector3(0);
        }

        public virtual void RenderStereo(World world, float x, int offset) { }
    }
}
