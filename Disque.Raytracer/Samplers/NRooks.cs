using System;
using System.Collections.Generic;
using System.Text;
using Disque.Math;

namespace Disque.Raytracer.Samplers
{
    public class NRooks : Sampler
    {
        public NRooks(int n)
            : base(n)
        {
            GenerateSamples();
        }

        public NRooks(int n, int sets)
            : base(n, sets)
        {
            GenerateSamples();
        }

        public override void GenerateSamples()
        {
            for (int p = 0; p < NumSets; p++)
                for (int j = 0; j < NumSamples; j++)
                {
                    Vector2 sp = new Vector2((j + MathHelper.RandomFloat()) / NumSamples, (j + MathHelper.RandomFloat()) / NumSamples);
                    samples.Add(sp);
                }
            ShuffleXCoordinates();
            ShuffleYCoordinates();
        }
    }
}
