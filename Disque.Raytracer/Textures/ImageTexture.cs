using System;
using System.Collections.Generic;
using System.Text;
using Disque.Math;
using Disque.Raytracer.Mappings;

namespace Disque.Raytracer.Textures
{
    public class ImageTexture : Texture
    {
        Image image;
        Mapping mapping;
        int hres, vres;

        public void SetImage(Image im)
        {
            image = im;
            hres = im.HRes;
            vres = im.VRes;
        }

        public void SetMapping(Mapping m)
        {
            mapping = m;
        }

        public override Vector3 GetColor(ShadeRec sr)
        {
            int row = 0;
            int column = 0;
            if (mapping != null)
            {
                mapping.GetTexelCoordinates(sr.LocalHitPoint, hres, vres, ref row, ref column);
            }
            else
            {
                row = (int)(sr.V * (vres - 1.0f));
                column = (int)(sr.U * (hres - 1.0f));
            }
            return (image.GetColor(row, column));
        }
    }
}
