using System;
using System.Collections.Generic;

using System.Text;
using Disque.Raytracer.Samplers;

namespace Disque.Raytracer.Worlds
{
    public class ViewPlane
    {
        public int HRes { get; set; }
        public int VRes { get; set; }
        public float S { get; set; } //Pixel size
        public float Gamma { get; set; }
        public float InvGamma { get { return 1.0f / Gamma; } }
        public bool Show_Out_Of_gammut { get; set; }
        public ViewPlane()
        {
            HRes = VRes = 400;
            S = Gamma = 1.0f;
            Show_Out_Of_gammut = false;
        }
        public ViewPlane(ViewPlane vp)
        {
            HRes = vp.HRes;
            VRes = vp.VRes;
            S = vp.S;
            Gamma = vp.Gamma;
            Show_Out_Of_gammut = vp.Show_Out_Of_gammut;
            sampler = vp.sampler;
            NumSamples = vp.NumSamples;
        }
        public int NumSamples { get; set; }
        Sampler sampler;
        public Sampler Sampler
        {
            get
            {
                return sampler;
            }
        }
        public void SetSampler(Sampler s)
        {
            sampler = s;
            NumSamples = s.NumSamples;
        }
        public void SetSamples(int num)
        {
            NumSamples = num;
            if (NumSamples > 1)
            {
                sampler = new MultiJittered(num);
            }
            else
            {
                sampler = new Regular(1);
            }
        }
        public int MaxDepth { get; set; }
    }
}
