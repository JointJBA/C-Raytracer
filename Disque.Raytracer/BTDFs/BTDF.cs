using System;
using System.Collections.Generic;
using System.Text;
using Disque.Math;

namespace Disque.Raytracer.BTDFs
{
    public class BTDF
    {
        public virtual Vector3 F(ShadeRec sr, Vector3 wo, Vector3 wi) { return Colors.Black; }

        public virtual Vector3 Sample_F(ShadeRec sr, Vector3 wo, ref Vector3 wt) { return Colors.Black; }

        public virtual Vector3 RHO(ShadeRec sr, Vector3 wo) { return Colors.Black; }
    }
}
