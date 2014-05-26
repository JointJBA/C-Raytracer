using System;
using System.Collections.Generic;
using System.Text;
using Disque.Math;

namespace Disque.Raytracer.Samplers
{
    public class Sampler
    {             
        public int NumSamples, NumSets;
        protected List<Vector2> samples = new List<Vector2>();
        List<int> ShuffledIndices = new List<int>();
        List<Vector3> SphereSamples = new List<Vector3>();
        List<Vector2> DiskSamples = new List<Vector2>();
        List<Vector3> HemisphereSamples = new List<Vector3>();
        int count, jump;

        public Sampler(int n)
        {
            NumSamples = n;
            NumSets = 83;
            count = jump = 0;
            SetupShuffledIndices();
        }

        public Sampler(int n, int sets)
        {
            NumSamples = n;
            NumSets = sets;
            count = jump = 0;
            SetupShuffledIndices();
        }

        public virtual void GenerateSamples()
        {
        }

        public void ShuffleXCoordinates()
        {
            for (int p = 0; p < NumSets; p++)
                for (int i = 0; i < NumSamples; i++)
                {
                    int target = MathHelper.RandomInt() % NumSamples + p * NumSamples;
                    float temp = samples[i + p * NumSamples + 1].X;
                    int a = i + p * NumSamples + 1;
                    samples[a] = SetX(samples[a], samples[target].X);
                    samples[target] = SetX(samples[target], temp);
                }
        }

        public void ShuffleYCoordinates()
        {
            for (int p = 0; p < NumSets; p++)
                for (int i = 0; i < NumSamples; i++)
                {
                    int target = MathHelper.RandomInt() % NumSamples + p * NumSamples;
                    float temp = samples[i + p * NumSamples + 1].Y;
                    int a= i + p * NumSamples + 1;
                    samples[a] = SetY(samples[a], samples[target].Y);
                    samples[target] = SetY(samples[target], temp);
                }
        }

        public void SetupShuffledIndices()
        {
            List<int> indices = new List<int>();
            for (int j = 0; j < NumSamples; j++)
                indices.Add(j);
            for (int p = 0; p < NumSets; p++)
            {
                random_shuffle(indices);
                for (int j = 0; j < NumSamples; j++)
                    ShuffledIndices.Add(indices[j]);
            }
        }

        public void MapSamplesToDisk()
        {
            int size = samples.Count;
            float r, phi;		// polar coordinates
            Vector2 sp; 		// sample point on unit disk
            for (int j = 0; j < size; j++)
            {
                DiskSamples.Add(new Vector2());
                sp = new Vector2();
                sp.X = 2.0f * samples[j].X - 1.0f;
                sp.Y = 2.0f * samples[j].Y - 1.0f;
                if (sp.X > -sp.Y)
                {			// sectors 1 and 2
                    if (sp.X > sp.Y)
                    {		// sector 1
                        r = sp.X;
                        phi = sp.Y / sp.X;
                    }
                    else
                    {					// sector 2
                        r = sp.Y;
                        phi = 2 - sp.X / sp.Y;
                    }
                }
                else
                {						// sectors 3 and 4
                    if (sp.X < sp.Y)
                    {		// sector 3
                        r = -sp.X;
                        phi = 4 + sp.Y / sp.X;
                    }
                    else
                    {					// sector 4
                        r = -sp.Y;
                        if (sp.Y != 0.0f)	// avoid division by zero at origin
                            phi = 6 - sp.X / sp.Y;
                        else
                            phi = 0.0f;
                    }
                }
                phi *= MathHelper.PI / 4.0f;
                DiskSamples[j] = SetX(DiskSamples[j], r * MathHelper.Cos(phi));
                DiskSamples[j] = SetY(DiskSamples[j], r * MathHelper.Sin(phi));
            }
            samples.Clear();
        }

        public void MapSamplesToHemisphere(float exp)
        {
            int size = samples.Count;
            for (int j = 0; j < size; j++)
            {
                float cos_phi = MathHelper.Cos(2.0f * MathHelper.PI * samples[j].X);
                float sin_phi = MathHelper.Sin(2.0f * MathHelper.PI * samples[j].X);
                float cos_theta = MathHelper.Pow((1.0f - samples[j].Y), 1.0f / (exp + 1.0f));
                float sin_theta = MathHelper.Sqrt(1.0f - cos_theta * cos_theta);
                float pu = sin_theta * cos_phi;
                float pv = sin_theta * sin_phi;
                float pw = cos_theta;
                HemisphereSamples.Add(new Vector3(pu, pv, pw));
            }
        }

        public void MapSamplesToSphere()
        {
            float r1, r2;
            float x, y, z;
            float r, phi;
            for (int j = 0; j < NumSamples * NumSets; j++)
            {
                r1 = samples[j].X;
                r2 = samples[j].Y;
                z = 1.0f - 2.0f * r1;
                r = MathHelper.Sqrt(1.0f - z * z);
                phi = MathHelper.PI * 2.0f * r2;
                x = r * MathHelper.Cos(phi);
                y = r * MathHelper.Sin(phi);
                SphereSamples.Add(new Vector3(x, y, z));
            }
        }

        public Vector2 SampleUnitSquare()
        {
            if (count % NumSamples == 0)  									// start of a new pixel
                jump = (MathHelper.RandomInt() % NumSets) * NumSamples;				// random index jump initialised to zero in constructor
            return (samples[jump + ShuffledIndices[jump + count++ % NumSamples]]);
        }

        public Vector2 SampleUnitDisk()
        {
            if (count % NumSamples == 0)  									// start of a new pixel
                jump = (MathHelper.RandomInt() % NumSets) * NumSamples;

            return (DiskSamples[jump + ShuffledIndices[jump + count++ % NumSamples]]);
        }

        public Vector3 SampleHemisphere()
        {
            if (count % NumSamples == 0)  									// start of a new pixel
                jump = (MathHelper.RandomInt() % NumSets) * NumSamples;

            return (HemisphereSamples[jump + ShuffledIndices[jump + count++ % NumSamples]]);
        }

        public Vector3 SampleSphere()
        {
            if (count % NumSamples == 0)  									// start of a new pixel
                jump = (MathHelper.RandomInt() % NumSets) * NumSamples;

            return (SphereSamples[jump + ShuffledIndices[jump + count++ % NumSamples]]);
        }

        public Vector2 SampleOneSet()
        {
            return (samples[count++ % NumSamples]);
        }

        void random_shuffle(List<int> indices)
        {
            int n = indices.Count;
            for (int i = n - 1; i > 0; --i)
            {
                int temp = indices[i];
                int ind = MathHelper.RandomInt(i + 1);
                indices[i] = indices[ind];
                indices[ind] = temp;
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
