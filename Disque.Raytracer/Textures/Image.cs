using System;
using System.Collections.Generic;
using System.Text;
using Disque.Math;

namespace Disque.Raytracer.Textures
{
    public class Image
    {
        public int VRes, HRes;
        public Vector3[] Pixels;
        public Vector3 GetColor(int row, int column)
        {
            int index = column + HRes * (VRes - row - 1);
            int pixels_size = Pixels.Length;
            if (index < pixels_size && index >= 0)
                return (Pixels[index]);
            else
                return (Colors.Red);
        }
    }
}
