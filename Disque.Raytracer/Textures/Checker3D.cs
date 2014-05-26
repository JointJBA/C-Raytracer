using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Disque.Math;

namespace Disque.Raytracer.Textures
{
    public class Checker3D : Texture
    {
        Vector3 color1, color2;
        float size;

        public void SetColors(Vector3 c1, Vector3 c2)
        {
            color1 = c1;
            color2 = c2;
        }

        public void SetSize(float s)
        {
            size = s;
        }

        public override Vector3 GetColor(ShadeRec sr)
        {
            float eps = -0.000187453738f;
            float x = sr.LocalHitPoint.X + eps;
            float y = sr.LocalHitPoint.Y + eps;
            float z = sr.LocalHitPoint.Z + eps;
            if (((int)MathHelper.Floor(x / size) + (int)MathHelper.Floor(y / size) + (int)MathHelper.Floor(z / size)) % 2 == 0)
                return (color1);
            else
                return (color2);
        }
    }
}
