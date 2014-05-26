using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Disque.Raytracer.Textures;
using Disque.Math;

namespace Disque.Raytracer.Materials
{
    public class SV_Emissive : Material
    {
        Texture t;
        float ls;

        public void SetTexture(Texture tex)
        {
            t = tex;
        }

        public void SetRadiance(float l)
        {
            ls = l;
        }

        public override Vector3 Get_Le(ShadeRec sr)
        {
            return ls * t.GetColor(sr);
        }

        public override Vector3 Shade(ShadeRec sr)
        {
            return ls * t.GetColor(sr);
        }

        public override Vector3 Global_Shade(ShadeRec sr)
        {
            if (sr.Depth == 1)
                return (Vector3.Zero);
            else
                return ls * t.GetColor(sr);
        }

    }
}
