using System;
using System.Collections.Generic;
using System.Text;
using Disque.Math;

namespace Disque.Raytracer.GeometricObjects.Primitives
{
    public class Torus : GeometricObject
    {       
        float aa, bb;
        BBox bbox;

        public Torus(float _a, float _b, string name)
            : base(name)
        {
            aa = _a;
            bb = _b;
            bbox = new BBox(-_a - _b, _a + _b, -_b, _b, -_a - _b, _a + _b);
        }

        public Vector3 ComputeNormal(Vector3 p)
        {
            return getNormal(p);
        }

        Vector3 getNormal(Vector3 p)
        {
            Vector3 locP = Vector3.Zero;
            float k, nx, ny, nz;
            locP = p;
            k = 4.0f * (locP.X * locP.X + locP.Y * locP.Y + locP.Z * locP.Z
                    + bb * bb - aa * aa);

            nx = locP.X * k - 8.0f * bb * bb * locP.X;
            ny = locP.Y * k;
            nz = locP.Z * k - 8.0f * bb * bb * locP.Z;

            Vector3 res = new Vector3(nx, ny, nz);
            res.Normalize();
            return res;
        }

        public override bool Hit(Ray ray, ref float tmin, ref ShadeRec sr)
        {
            //if (!bbox.Hit(ray))
            //    return false;
            //Vector3 O = ray.Position;
            //    Vector3 D = ray.Direction;
            //Vector3 intersection;
            //float a, b, c, d, e, t;
            //float[] roots;
            //int nbRoots;
            //float tmpA = D.X * D.X + D.Y * D.Y + D.Z * D.Z;
            //float tmpB = 2.0f * (D.X * O.X + D.Y * O.Y + D.Z * O.Z);
            //float tmpC = O.X * O.X + O.Y * O.Y + O.Z * O.Z + bigRadius * bigRadius - smallRadius * smallRadius;
            //float tmp = 4.0f * bigRadius * bigRadius;
            //float tmpD = tmp * (D.X * D.X + D.Z * D.Z);
            //float tmpE = tmp * 2.0f * (D.X * O.X + D.Z * O.Z);
            //float tmpF = tmp * (O.X * O.X + O.Z * O.Z);
            //a = tmpA * tmpA;
            //b = 2.0f * tmpA * tmpB;
            //c = 2.0f * tmpA * tmpC + tmpB * tmpB - tmpD;
            //d = 2.0f * tmpB * tmpC - tmpE;
            //e = tmpC * tmpC - tmpF;
            //roots = new float[4];
            //var solver = new float[5] { a, b, c, d, e };
            //nbRoots = MathHelper.SolveQuartic(solver, roots);
            //t = float.MaxValue;
            //for (int j = 0; j < nbRoots; j++)
            //{
            //    if (roots[j] > 0.001 && roots[j] < t)
            //        t = roots[j];
            //}
            //if (t != float.MaxValue)
            //{
            //    intersection = new Vector3(O.X + D.X * t, O.Y + D.Y * t, O.Z + D.Z * t);
            //    sr.LocalHitPoint = (intersection);
            //    sr.Normal = (getNormal(intersection));
            //    tmin =(t);
            //    return (true);
            //}
            //return (false);
            if (!bbox.Hit(ray))
                return (false);
            float x1 = ray.Position.X; float y1 = ray.Position.Y; float z1 = ray.Position.Z;
            float d1 = ray.Direction.X; float d2 = ray.Direction.Y; float d3 = ray.Direction.Z;
            float[] coeffs = new float[5];	// coefficient array for the quartic equation
            float[] roots = new float[4];	// solution array for the quartic equation	
            float sum_d_sqrd = d1 * d1 + d2 * d2 + d3 * d3;
            float e = x1 * x1 + y1 * y1 + z1 * z1 - aa * aa - bb * bb;
            float f = x1 * d1 + y1 * d2 + z1 * d3;
            float four_a_sqrd = 4.0f * aa * aa;
            coeffs[0] = e * e - four_a_sqrd * (bb * bb - y1 * y1); 	// constant term
            coeffs[1] = 4.0f * f * e + 2.0f * four_a_sqrd * y1 * d2;
            coeffs[2] = 2.0f * sum_d_sqrd * e + 4.0f * f * f + four_a_sqrd * d2 * d2;
            coeffs[3] = 4.0f * sum_d_sqrd * f;
            coeffs[4] = sum_d_sqrd * sum_d_sqrd;  					// coefficient of t^4
            int num_real_roots = MathHelper.SolveQuartic(coeffs, roots);
            bool intersected = false;
            float t = float.MaxValue;
            if (num_real_roots == 0)  // ray misses the torus
                return (false);
            for (int j = 0; j < num_real_roots; j++)
                if (roots[j] > MathHelper.Epsilon)
                {
                    intersected = true;
                    if (roots[j] < t)
                        t = roots[j];
                }
            if (!intersected)
                return (false);
            tmin = t;
            sr.LocalHitPoint = ray.Position + t * ray.Direction;
            sr.Normal = ComputeNormal(sr.LocalHitPoint);
            return (true);
        }

        public override bool ShadowHit(Ray ray, ref float tmin)
        {
            return false;
        }
    }
}
