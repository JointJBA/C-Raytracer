using System;
using System.Collections.Generic;
using System.Text;
using Disque.Math;

namespace Disque.Raytracer.Samplers
{
    public class MultiJittered : Sampler
    {
        public MultiJittered(int n)
            : base(n)
        {
            GenerateSamples();
        }

        public MultiJittered(int n, int sets)
            : base(n, sets)
        {
            GenerateSamples();
        }

        public override void GenerateSamples()
        {
            int n = (int)MathHelper.Sqrt(NumSamples);
            float subcell_width = 1.0f / ((float)NumSamples);
            Vector2 fill_point = new Vector2();
            for (int j = 0; j < NumSamples * NumSets; j++)
                samples.Add(fill_point);
            for (int p = 0; p < NumSets; p++)
                for (int i = 0; i < n; i++)
                    for (int j = 0; j < n; j++)
                    {
                        int s = i * n + j + p * NumSamples;
                        samples[s] = SetX(samples[s], (i * n + j) * subcell_width + MathHelper.RandomFloat(0, subcell_width));
                        samples[s] = SetY(samples[s], (j * n + i) * subcell_width + MathHelper.RandomFloat(0, subcell_width));
                    }

            for (int p = 0; p < NumSets; p++)
                for (int i = 0; i < n; i++)
                    for (int j = 0; j < n; j++)
                    {
                        int k = MathHelper.RandomInt(j, n - 1);
                        float t = samples[i * n + j + p * NumSamples].X;
                        int a = i * n + j + p * NumSamples;
                        int b = i * n + k + p * NumSamples;
                        samples[a] = SetX(samples[a], samples[i * n + k + p * NumSamples].X);
                        samples[b] = SetX(samples[b], t);
                    }

            for (int p = 0; p < NumSets; p++)
                for (int i = 0; i < n; i++)
                    for (int j = 0; j < n; j++)
                    {
                        int k = MathHelper.RandomInt(j, n - 1);
                        float t = samples[j * n + i + p * NumSamples].Y;
                        int a = j * n + i + p * NumSamples;
                        int b = k * n + i + p * NumSamples;
                        samples[a] = SetY(samples[a], samples[b].Y);
                        samples[b] = SetY(samples[b], t);
                    }
        }

        Vector2 SetX(Vector2 p, float x)
        {
            Vector2 c = new Vector2(x, p.Y);
            return c;
        }

        Vector2 SetY(Vector2 p, float y)
        {
            Vector2 c = new Vector2(p.X, y);
            return c;
        }
    }
}
