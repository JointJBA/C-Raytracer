using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Disque.Math;

namespace Disque.Raytracer.GeometricObjects.Primitives
{
    public class Quadratic : GeometricObject
    {
        Vector3 location;
        float A, B, C, D, E, F, G, H, I, J;

        public Quadratic(Vector3 pos,
            float a, float b, float c, float d, float e,
            float f, float g, float h, float i, float j, string name)
            : base(name)
        {
            A = a; B = b; C = c; D = d; E = e; F = f; G = g; H = h; I = i; J = j;
            location = pos;
        }

        public override bool Hit(Ray ray, ref float tmin, ref ShadeRec sr)
        {
            Vector3 O = ray.Position;
            Vector3 vecD = ray.Direction;
            Vector3 intersection;
            Vector3 normal;
            float Aq, Bq, Cq, t0, t1, t;
            Aq = A * vecD.X * vecD.X +
                            B * vecD.Y * vecD.Y +
                            C * vecD.Z * vecD.Z +
                            D * vecD.X * vecD.Y +
                            E * vecD.X * vecD.Z +
                            F * vecD.Y * vecD.Z;

            Bq = 2 * (A * O.X * vecD.X + B * O.Y * vecD.Y + C * O.Z * vecD.Z +
                            D * (O.X * vecD.Y + O.Y * vecD.X) +
                            E * (O.X * vecD.Z + O.Z * vecD.X) +
                            F * (O.Y * vecD.Z + O.Z * vecD.Y)) +
                            G * vecD.X +
                            H * vecD.Y +
                            I * vecD.Z;

            Cq = A * O.X * O.X +
                            B * O.Y * O.Y +
                            C * O.Z * O.Z +
                            D * O.X * O.Y +
                            E * O.X * O.Z +
                            F * O.Y * O.Z +
                            G * O.X +
                            H * O.Y +
                            I * O.Z +
                            J;
            if (Aq == 0) { t = -Cq / Bq; }
            else
            {
                if ((Bq * Bq - 4 * Aq * Cq) < 0) return false;
                else
                {
                    t0 = (-Bq - MathHelper.Sqrt(Bq * Bq - 4.0f * Aq * Cq)) / (2.0f * Aq);
                    t1 = (-Bq + MathHelper.Sqrt(Bq * Bq - 4.0f * Aq * Cq)) / (2.0f * Aq);

                    if (!(t0 < 0.001 && t1 < 0.001))
                    {
                        if (t0 <= 0.001)
                        {
                            t = t1;
                        }
                        else
                        {
                            t = t0;
                        }
                        double x0 = (O.X - location.X);
                        double y0 = (O.Y - location.Y);
                        double z0 = (O.Z - location.Z);

                        double xd = vecD.X;
                        double yd = vecD.Y;
                        double zd = vecD.Z;

                        intersection = new Vector3(O.X + vecD.X * t, O.Y + vecD.Y * t, O.Z + vecD.Z * t);
                        normal = new Vector3((2 * A * ((O.X - location.X) + vecD.X * t) +
                                                        2 * F * ((O.Y - location.Y) + vecD.Y * t) +
                                                        2 * E * ((O.Z - location.Z) + vecD.Z * t) + G - location.X),
                                                   (2 * B * ((O.Y - location.Y) + vecD.Y * t) +
                                                    2 * F * ((O.X - location.X) + vecD.X * t) +
                                                    2 * D * ((O.Z - location.Z) + vecD.Z * t) + H - location.Y),
                                                   (2 * C * ((O.Z - location.Z) + vecD.Z * t) +
                                                        2 * E * ((O.X - location.X) + vecD.X * t) +
                                                        2 * D * ((O.Y - location.Y) + vecD.Y * t) + I - location.Z));

                        normal.Normalize();
                        sr.Normal = normal;
                        sr.LocalHitPoint = intersection;
                        if(t > tmin)
                            tmin = t;
                        return (true);
                    }
                }
            }
            return (false);
        }
    }
}
