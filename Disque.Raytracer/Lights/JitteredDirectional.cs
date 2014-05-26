using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Disque.Math;

namespace Disque.Raytracer.Lights
{
    public class JitteredDirectional : Directional
    {
        float r;
        public void SetJitterAmount(float f)
        {
            r = f;
        }

        public override Vector3 GetDirection(ShadeRec sr)
        {
            Vector3 finaldir = Vector3.Zero;
            finaldir.X = Direction.X + r * (2.0f * MathHelper.RandomFloat() - 1.0f);
            finaldir.Y = Direction.Y + r * (2.0f * MathHelper.RandomFloat() - 1.0f);
            finaldir.Z = Direction.Z + r * (2.0f * MathHelper.RandomFloat() - 1.0f);
            finaldir.Normalize();
            return finaldir;
        }
    }
}
