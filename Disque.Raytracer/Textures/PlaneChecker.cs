using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Disque.Math;

namespace Disque.Raytracer.Textures
{
    public class PlaneChecker : Texture
    {
        Vector3 color1, color2, outline_color;
        float outline_width, size;

        public void SetColors(Vector3 c1, Vector3 c2, Vector3 otl)
        {
            color1 = c1;
            color2 = c2;
            outline_color = otl;
        }

        public void SetDimensions(float owidth, float s)
        {
            outline_width = owidth;
            size = s;
        }

        public override Vector3 GetColor(ShadeRec sr)
        {
            float x = sr.LocalHitPoint.X;
            float z = sr.LocalHitPoint.Z;
            int ix = MathHelper.Floor(x / size);
            int iz = MathHelper.Floor(z / size);
            float fx = x / size - ix;
            float fz = z / size - iz;
            float width = 0.5f * outline_width / size;
            bool in_outline = (fx < width || fx > 1.0 - width) || (fz < width || fz > 1.0 - width);
            if ((ix + iz) % 2 == 0)
            {
                if (!in_outline)
                    return (color1);
            }
            else
            {
                if (!in_outline)
                    return (color2);
            }

            return (outline_color);
        }
    }
}
