using System;
using System.Collections.Generic;

using System.Text;
using Disque.Raytracer.Worlds;
using Disque.Raytracer.Materials;
using Disque.Math;

namespace Disque.Math
{
    public class ShadeRec
    {
        public bool Hit_an_object;
        public Vector3 LocalHitPoint;
        public Vector3 HitPoint;
        public Vector3 Color;
        public Vector3 Normal;
        public World World;
        public Ray Ray;
        public int Depth;
        public Material Material;
        public float T, U, V;
        public ShadeRec(World world)
        {
            World = world;
        }
    }
}
