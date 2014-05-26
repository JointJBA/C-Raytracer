using System;
using System.Collections.Generic;
using System.Text;
using Disque.Math;

namespace Disque.Raytracer.Samplers
{
    public class PureRandom : Sampler
    {
        public PureRandom(int n)
            : base(n)
        {
            GenerateSamples();
        }

        public override void GenerateSamples()
        {
            for (int p = 0; p < NumSets; p++)
                for (int q = 0; q < NumSamples; q++)
                    samples.Add(new Vector2(MathHelper.RandomFloat(), MathHelper.RandomFloat()));
        }
    }
}
