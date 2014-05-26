using System;
using System.Collections.Generic;
using System.Text;
using Disque.Math;

namespace Disque.Raytracer.Samplers
{
    public class Hammersley : Sampler
    {
        public Hammersley(int n)
            : base(n)
        {
            GenerateSamples();
        }

        public override void GenerateSamples()
        {
            for (int p = 0; p < NumSets; p++)
                for (int j = 0; j < NumSamples; j++)
                {
                    Vector2 pv = new Vector2((float)j / (float)NumSamples, phi(j));
                    samples.Add(pv);
                }
        }

        float phi(int j)
        {
            float x = 0.0f;
            float f = 0.5f;
            while (j == 1)
            {
                x += f * (j % 2);
                j /= 2;
                f *= 0.5f;
            }
            return (x);
        }
    }
}
