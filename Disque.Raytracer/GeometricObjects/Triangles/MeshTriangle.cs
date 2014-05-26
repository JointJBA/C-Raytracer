using System;
using System.Collections.Generic;
using System.Text;
using Disque.Math;

namespace Disque.Raytracer.GeometricObjects.Triangles
{
    public class MeshTriangle : GeometricObject
    {
        protected Mesh Mesh;
        protected int index0, index1, index2;
        protected Vector3 Normal;
        protected float Area;
        BBox bbox;

        public int Index0
        {
            get
            {
                return index0;
            }
        }

        public int Index1
        {
            get
            {
                return index1;
            }
        }

        public int Index2
        {
            get
            {
                return index2;
            }
        }

        public MeshTriangle(int i0, int i1, int i2, Mesh m, string name)
            : base(name)
        {
            index0 = i0;
            index1 = i1;
            index2 = i2;
            Mesh = m;
            SetBoundingBox();
        }

        public override bool Hit(Ray ray, ref float tmin, ref ShadeRec sr)
        {
            return base.Hit(ray, ref tmin, ref sr);
        }

        public override bool ShadowHit(Ray ray, ref float tmin)
        {
            return base.ShadowHit(ray, ref tmin);
        }

        public void ComputeNormal(bool reverseNormal)
        {
            Normal = Vector3.Cross((Mesh.Vertices[index1] - Mesh.Vertices[index0]), (Mesh.Vertices[index2] - Mesh.Vertices[index0]));
            Normal.Normalize();
            if (reverseNormal)
                Normal = -Normal;
        }

        public override Vector3 GetNormal()
        {
            return Normal;
        }

        protected float interpolate_u(float beta, float gamma)
        {
            return ((1 - beta - gamma) * Mesh.U[index0]
                + beta * Mesh.U[index1]
                    + gamma * Mesh.U[index2]);
        }

        protected float interpolate_v(float beta, float gamma)
        {
            return ((1 - beta - gamma) * Mesh.V[index0]
                + beta * Mesh.V[index1]
                    + gamma * Mesh.V[index2]);
        }

        public override BBox GetBoundingBox()
        {
            return bbox;
        }

        public override void SetBoundingBox()
        {
            Vector3 v0 = Mesh.Vertices[index0], v1 = Mesh.Vertices[index1], v2 = Mesh.Vertices[index2];
            float delta = 0.000001f;
            bbox = (new BBox(MathHelper.Min(MathHelper.Min(v0.X, v1.X), v2.X) - delta, MathHelper.Max(MathHelper.Max(v0.X, v1.X), v2.X) + delta,
                         MathHelper.Min(MathHelper.Min(v0.Y, v1.Y), v2.Y) - delta, MathHelper.Max(MathHelper.Max(v0.Y, v1.Y), v2.Y) + delta,
                         MathHelper.Min(MathHelper.Min(v0.Z, v1.Z), v2.Z) - delta, MathHelper.Max(MathHelper.Max(v0.Z, v1.Z), v2.Z) + delta));
        }
    }
}
