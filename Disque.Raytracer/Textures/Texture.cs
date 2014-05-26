using System;
using System.Collections.Generic;
using System.Text;
using Disque.Math;

namespace Disque.Raytracer.Textures
{
    public class Texture
    {
        public virtual Vector3 GetColor(ShadeRec sr)
        {
            return Colors.White;
        }
    }
}
