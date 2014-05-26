using System;
using System.Collections.Generic;
using System.Text;
using Disque.Math;
using Disque.Raytracer.Materials;

namespace Disque.Raytracer.GeometricObjects
{
    public class Instance : GeometricObject
    {
        public GeometricObject Object;
        Matrix4 inv_matrix = Matrix4.Identity;
        static Matrix4 forward_matrix = Matrix4.Identity;
        BBox bbox;
        public bool TransformTexture = true;

        public Instance(GeometricObject obj, string name)
            : base(name)
        {
            Object = obj;
            bbox = obj.GetBoundingBox();
        }

        public override GeometricObject Clone()
        {
            return base.Clone();
        }

        public virtual void ComputeBoundingBox()
        {
            BBox object_bbox = Object.GetBoundingBox();
            Vector3[] v = new Vector3[8];
            v[0].X = object_bbox.Min.X; v[0].Y = object_bbox.Min.Y; v[0].Z = object_bbox.Min.Z;
            v[1].X = object_bbox.Max.X; v[1].Y = object_bbox.Min.Y; v[1].Z = object_bbox.Min.Z;
            v[2].X = object_bbox.Max.X; v[2].Y = object_bbox.Max.Y; v[2].Z = object_bbox.Min.Z;
            v[3].X = object_bbox.Min.X; v[3].Y = object_bbox.Max.Y; v[3].Z = object_bbox.Min.Z;
            v[4].X = object_bbox.Min.X; v[4].Y = object_bbox.Min.Y; v[4].Z = object_bbox.Max.Z;
            v[5].X = object_bbox.Max.X; v[5].Y = object_bbox.Min.Y; v[5].Z = object_bbox.Max.Z;
            v[6].X = object_bbox.Max.X; v[6].Y = object_bbox.Max.Y; v[6].Z = object_bbox.Max.Z;
            v[7].X = object_bbox.Min.X; v[7].Y = object_bbox.Max.Y; v[7].Z = object_bbox.Max.Z;
            v[0] = Vector3.Transform(v[0], forward_matrix);
            v[1] = Vector3.Transform(v[1], forward_matrix);
            v[2] = Vector3.Transform(v[2],forward_matrix);
            v[3] = Vector3.Transform(v[3], forward_matrix);
            v[4] = Vector3.Transform( v[4], forward_matrix);
            v[5] = Vector3.Transform( v[5], forward_matrix);
            v[6] = Vector3.Transform(v[6], forward_matrix);
            v[7] = Vector3.Transform( v[7], forward_matrix);
            forward_matrix = Matrix4.Identity;
            float x0 = float.MaxValue;
            float y0 = float.MaxValue;
            float z0 = float.MaxValue;
            for (int j = 0; j <= 7; j++)
            {
                if (v[j].X < x0)
                    x0 = v[j].X;
            }

            for (int j = 0; j <= 7; j++)
            {
                if (v[j].Y < y0)
                    y0 = v[j].Y;
            }

            for (int j = 0; j <= 7; j++)
            {
                if (v[j].Z < z0)
                    z0 = v[j].Z;
            }
            float x1 = float.MinValue;
            float y1 = float.MinValue;
            float z1 = float.MinValue;
            for (int j = 0; j <= 7; j++)
            {
                if (v[j].X > x1)
                    x1 = v[j].X;
            }
            for (int j = 0; j <= 7; j++)
            {
                if (v[j].Y > y1)
                    y1 = v[j].Y;
            }

            for (int j = 0; j <= 7; j++)
            {
                if (v[j].Z > z1)
                    z1 = v[j].Z;
            }
            bbox.Min.X = x0;
            bbox.Min.Y = y0;
            bbox.Min.Z = z0;
            bbox.Max.X = x1;
            bbox.Max.Y = y1;
            bbox.Max.Z = z1;
        }

        public override BBox GetBoundingBox()
        {
            return bbox;
        }

        public override void SetBoundingBox()
        {
            ComputeBoundingBox();
        }

        public override Material GetMaterial()
        {
            return Object.GetMaterial();
        }

        public override void SetMaterial(Material m)
        {
            Object.SetMaterial(m);
        }

        public override bool Hit(Ray ray, ref float t, ref ShadeRec sr)
        {
            Ray inv_ray = new Ray(ray.Position, ray.Direction);
            inv_ray.Position = Vector3.TransformPosition(inv_ray.Position, inv_matrix);
            inv_ray.Direction = Vector3.Transform(inv_ray.Direction, inv_matrix);
            if (Object.Hit(inv_ray, ref t, ref sr))
            {
                sr.Normal = Vector3.TransformNormal(sr.Normal, inv_matrix);
                sr.Normal.Normalize();
                if (Object.GetMaterial() != null)
                    Material = Object.GetMaterial();
                if (!TransformTexture)
                    sr.LocalHitPoint = ray.Position + t * ray.Direction;
                return (true);
            }
            return (false);
        }

        public override bool ShadowHit(Ray ray, ref float tmin)
        {
            if (!Shadows)
                return false;
            bool prev = Object.GetShadows();
            Object.SetShadows(true);
            Ray inv_ray = new Ray();
            inv_ray.Position = Vector3.TransformPosition(ray.Position, inv_matrix);
            inv_ray.Direction = Vector3.Transform(ray.Direction, inv_matrix);
            if (Object.ShadowHit(inv_ray, ref tmin))
            {
                Object.SetShadows(prev);
                return (true);
            }
            Object.SetShadows(prev);
            return (false);
        }

        public void Translate(Vector3 trans)
        {
            Matrix4 inv_translation_matrix = Matrix4.CreateTranslation(-trans);
            inv_matrix = inv_matrix * inv_translation_matrix;
            Matrix4 translation_matrix = Matrix4.CreateTranslation(trans);
            forward_matrix = translation_matrix * forward_matrix;
        }

        //public void RotateX(float theta)
        //{
        //    Matrix4 rotation_matrix = Matrix4.CreateRotation(new Vector3(1, 0, 0), theta);
        //    Matrix4 inv_rotation_matrix = Matrix4.CreateRotation(new Vector3(1, 0, 0), -theta);
        //    inv_matrix = inv_matrix * inv_rotation_matrix;
        //    forward_matrix = rotation_matrix * forward_matrix;
        //}
        //public void RotateY(float theta)
        //{
        //    Matrix4 rotation_matrix = Matrix4.CreateRotation(new Vector3(0, 1, 0), theta);
        //    Matrix4 inv_rotation_matrix = Matrix4.CreateRotation(new Vector3(0, 1, 0), -theta);
        //    inv_matrix = inv_matrix * inv_rotation_matrix;
        //    forward_matrix = rotation_matrix * forward_matrix;
        //    //Console.WriteLine(inv_matrix);
        //    //Console.WriteLine(Matrix4.Inverse(forward_matrix));
        //    //float sin_theta = MathHelper.Sin(theta * MathHelper.PI / 180.0f);
        //    //float cos_theta = MathHelper.Cos(theta * MathHelper.PI / 180.0f);
        //    //Matrix4 inv_y_rotation_matrix = Matrix4.Identity;					// temporary inverse rotation matrix about y axis
        //    //inv_y_rotation_matrix[0, 0] = cos_theta;
        //    //inv_y_rotation_matrix[0, 2] = -sin_theta;
        //    //inv_y_rotation_matrix[2, 0] = sin_theta;
        //    //inv_y_rotation_matrix[2, 2] = cos_theta;
        //    //inv_matrix = inv_matrix * inv_y_rotation_matrix;
        //    //Matrix4 y_rotation_matrix = Matrix4.Identity;						// temporary rotation matrix about x axis
        //    //y_rotation_matrix[0, 0] = cos_theta;
        //    //y_rotation_matrix[0, 2] = sin_theta;
        //    //y_rotation_matrix[2, 0] = -sin_theta;
        //    //y_rotation_matrix[2, 2] = cos_theta;
        //    //forward_matrix = y_rotation_matrix * forward_matrix;
        //}
        //public void RotateZ(float theta)
        //{
        //    Matrix4 rotation_matrix = Matrix4.CreateRotation(new Vector3(0, 0, 1), theta);
        //    Matrix4 inv_rotation_matrix = Matrix4.CreateRotation(new Vector3(0, 0, 1), -theta);
        //    inv_matrix = inv_matrix * inv_rotation_matrix;
        //    forward_matrix = rotation_matrix * forward_matrix;
        //}
        //public void Rotate(Vector3 axis, float theta)
        //{
        //    //Matrix4 rotation_matrix = Matrix4.CreateRotation(axis, theta);
        //    //Matrix4 inv_rotation_matrix = Matrix4.CreateRotation(axis, -theta);
        //    //inv_matrix = inv_matrix * inv_rotation_matrix;
        //    //forward_matrix = rotation_matrix * forward_matrix;
        //}
        //public void Scale(Vector3 scale)
        //{
        //    Matrix4 scale_matrix = Matrix4.CreateScale(scale);
        //    Matrix4 inv_scale_matrix = Matrix4.CreateScale(new Vector3(1.0f / scale.X, 1.0f / scale.Y, 1.0f / scale.Z));
        //    inv_matrix = inv_matrix * inv_scale_matrix;
        //    forward_matrix = scale_matrix * forward_matrix;
        //}

        public void Transform(Matrix4 m)
        {
            Matrix4 inv = m.Inverse();
            inv_matrix *= inv;
            forward_matrix = m * forward_matrix;
        }
        /*
         * void												
Instance::shear(const Matrix& s) {
	
	Matrix inverse_shearing_matrix;    // inverse shear matrix
	
	// discriminant

	double d = 1.0 	- s.m[1][0] * s.m[0][1] - s.m[2][0] * s.m[0][2]  - s.m[2][1] * s.m[1][2]
					+ s.m[1][0] * s.m[2][1] * s.m[0][2] + s.m[2][0] * s.m[0][1] * s.m[2][1];
					
	// diagonals
	
	inverse_shearing_matrix.m[0][0] = 1.0 - s.m[2][1] * s.m[1][2];
	inverse_shearing_matrix.m[1][1] = 1.0 - s.m[2][0] * s.m[0][2];
	inverse_shearing_matrix.m[2][2] = 1.0 - s.m[1][0] * s.m[0][1];
	inverse_shearing_matrix.m[3][3] = d;
	
	// first row
	
	inverse_shearing_matrix.m[0][1] = -s.m[1][0] + s.m[2][0] * s.m[1][2];
	inverse_shearing_matrix.m[0][2] = -s.m[2][0] + s.m[1][0] * s.m[2][1];
	
	// second row
	
	inverse_shearing_matrix.m[1][0] = -s.m[0][1] + s.m[2][1] * s.m[0][2];
	inverse_shearing_matrix.m[1][2] = -s.m[2][1] + s.m[2][0] * s.m[0][1];
	
	// third row
	
	inverse_shearing_matrix.m[2][0] = -s.m[0][2] + s.m[0][1] * s.m[1][2];
	inverse_shearing_matrix.m[2][1] = -s.m[1][2] + s.m[1][0] * s.m[0][2] ;
	
	// divide by discriminant
	
	inverse_shearing_matrix = inverse_shearing_matrix / d;
	
	inv_matrix = inv_matrix * inverse_shearing_matrix;	
	
	forward_matrix = s * forward_matrix; 
}
*/
    }
}
