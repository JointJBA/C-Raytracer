using System;
using System.Collections.Generic;
using System.Text;
using Disque.Math;

namespace Disque.Raytracer.Samplers
{
    public class Jittered : Sampler
    {
        public Jittered(int n)
            : base(n)
        {
            GenerateSamples();
        }

        public Jittered(int n, int set)
            : base(n, set)
        {
            GenerateSamples();
        }

        public override void GenerateSamples()
        {
            int n = (int)MathHelper.Sqrt(NumSamples);
            for (int p = 0; p < NumSets; p++)
                for (int j = 0; j < n; j++)
                {
                    for (int k = 0; k < n; k++)
                    {
                        Vector2 sp = new Vector2((((float)k) + MathHelper.RandomFloat()) / ((float)n), (((float)j) + MathHelper.RandomFloat()) / ((float)n));
                        samples.Add(sp);
                    }
                }
        }
    }
}
