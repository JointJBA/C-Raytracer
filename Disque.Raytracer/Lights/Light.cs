using System;
using System.Collections.Generic;
using System.Text;
using Disque.Math;

namespace Disque.Raytracer.Lights
{
    public abstract class Light
    {        
        public bool Shadows = true;

        public abstract Vector3 GetDirection(ShadeRec sr);

        public virtual Light Clone() { return null; }

        public abstract Vector3 L(ShadeRec sr);

        public virtual float G(ShadeRec sr){return 1;}

        public virtual float PDF(ShadeRec sr){ return 1;}

        public abstract bool InShadow(Ray shadowRay, ShadeRec sr);
    }
}
