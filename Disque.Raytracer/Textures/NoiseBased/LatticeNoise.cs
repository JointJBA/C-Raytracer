using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Disque.Math;
using Disque.Raytracer.Samplers;

namespace Disque.Raytracer.Textures.NoiseBased
{
    public class LatticeNoise
    {
        public const int kTableSize = 256;
        public const int kTableMask = kTableSize - 1;
        public const int seed_value = 253;
        protected static readonly byte[] permutation_table = new byte[kTableSize]
        {
            225,155,210,108,175,199,221,144,203,116, 70,213, 69,158, 33,25,
            5, 82,173,133,222,139,174, 27,  9, 71, 90,246, 75,130, 91,191,
            169,138,  2,151,194,235, 81,  7, 25,113,228,159,205,253,134,142,
            248, 65,224,217, 22,121,229, 63, 89,103, 96,104,156, 17,201,129,
            36,  8,165,110,237,117,231, 56,132,211,152, 20,181,111,239,218,
            170,163, 51,172,157, 47, 80,212,176,250, 87, 49, 99,242,136,189,
            162,115, 44, 43,124, 94,150, 16,141,247, 32, 10,198,223,255, 72,
            53,131, 84, 57,220,197, 58, 50,208, 11,241, 28,  3,192, 62,202,
            18,215,153, 24, 76, 41, 15,179, 39, 46, 55,  6,128,167, 23,188,
            106, 34,187,140,164, 73,112,182,244,195,227, 13, 35, 77,196,185,
            26,200,226,119, 31,123,168,125,249, 68,183,230,177,135,160,180,
            12,  1,243,148,102,166, 38,238,251, 37,240,126, 64, 74,161, 40,
            184,149,171,178,101, 66, 29, 59,146, 61,254,107, 42, 86,154,  4,
            236,232,120, 21,233,209, 45, 98,193,114, 78, 19,206, 14,118,127,
            48, 79,147, 85, 30,207,219, 54, 88,234,190,122, 95, 67,143,109,
            137,214,145, 93, 92,100,245,  0,216,186, 60, 83,105, 97,204, 52
        };

        protected int num_octaves;
        protected float lacunarity, gain;
        protected float[] value_table = new float[kTableSize];
        protected Vector3[] vector_table = new Vector3[kTableSize];
        float fbm_min, fbm_max;

        public LatticeNoise(int nc, float lac, float g)
        {
            num_octaves = nc;
            lacunarity = lac;
            gain = g;
            init_value_table(seed_value);
            init_vector_table(seed_value);
            compute_fbm_bounds();
        }

		void init_value_table(int seed_value)
        {
            MathHelper.SetRandomSeed(seed_value);
            for (int i = 0; i < kTableSize; i++)
                value_table[i] = 1.0f - 2.0f * MathHelper.RandomFloat();
        }

        void init_vector_table(int seed)
        {
            float r1, r2;
            float x, y, z;
            float r, phi;
            MathHelper.SetRandomSeed(seed_value);
            MultiJittered sample_ptr = new MultiJittered(256, 1);
            for (int j = 0; j < kTableSize; j++)
            {
                Vector2 sample_point = sample_ptr.SampleOneSet();
                r1 = sample_point.X;
                r2 = sample_point.Y;
                z = 1.0f - 2.0f * r1;
                r = MathHelper.Sqrt(1.0f - z * z);
                phi = MathHelper.TwoPI * r2;
                x = r * MathHelper.Cos(phi);
                y = r * MathHelper.Sin(phi);
                vector_table[j] = Vector3.Normalize(new Vector3(x, y, z));
            }
        }
		
		void compute_fbm_bounds()
        {
            if (gain == 1.0f)
                fbm_max = num_octaves;
            else
                fbm_max = (1.0f - MathHelper.Pow(gain, num_octaves)) / (1.0f - gain);
            fbm_min = -fbm_max;
        }

        public virtual float value_noise(Vector3 p)
        {
            return 0.0f;
        }

        public virtual Vector3 vector_noise(Vector3 p)
        {
            return Vector3.Zero;
        }

        public virtual float value_fractal_sum(Vector3 p)
        {
            float amplitude = 1.0f;
            float frequency = 1.0f;
            float fractal_sum = 0.0f;
            for (int j = 0; j < num_octaves; j++)
            {
                fractal_sum += amplitude * value_noise(frequency * p);
                amplitude *= 0.5f;
                frequency *= 2.0f;
            }
            fractal_sum = (fractal_sum - fbm_min) / (fbm_max - fbm_min);  // map to [0, 1]
            return (fractal_sum);
        }

        public virtual Vector3 vector_fractal_sum(Vector3 p)
        {
            float amplitude = 1.0f;
            float frequency = 1.0f;
            Vector3 sum = new Vector3(0.0f);
            for (int j = 0; j < num_octaves; j++)
            {
                sum += amplitude * vector_noise(frequency * p);
                amplitude *= 0.5f;
                frequency *= 2.0f;
            }
            return (sum);
        }

        public virtual float value_turbulence(Vector3 p)
        {
            float amplitude = 1.0f;
            float frequency = 1.0f;
            float turbulence = 0.0f;
            for (int j = 0; j < num_octaves; j++)
            {
                turbulence += amplitude * MathHelper.Abs(value_noise(frequency * p));
                amplitude *= 0.5f;
                frequency *= 2.0f;
            }
            turbulence /= fbm_max;
            return (turbulence);
        }
				
		public virtual float value_fbm(Vector3 p) 
        {
            float amplitude = 1.0f;
            float frequency = 1.0f;
            float fbm = 0.0f;
            for (int j = 0; j < num_octaves; j++)
            {
                fbm += amplitude * value_noise(frequency * p);
                amplitude *= gain;
                frequency *= lacunarity;
            }
            fbm = (fbm - fbm_min) / (fbm_max - fbm_min);
            return (fbm);
        }

        public virtual Vector3 vector_fbm(Vector3 p)
        {
            float amplitude = 1.0f;
            float frequency = 1.0f;
            Vector3 sum = new Vector3(0.0f);
            for (int j = 0; j < num_octaves; j++)
            {
                sum += amplitude * vector_noise(frequency * p);
                amplitude *= gain;
                frequency *= lacunarity;
            }
            return (sum);
        }

        public void set_num_octaves(int octaves)
        {
            num_octaves = octaves;
            compute_fbm_bounds();
        }

        public void set_lacunarity(float _lacunarity)
        {
            lacunarity = _lacunarity;
        }

        void set_gain(float _gain)
        {
            gain = _gain;
            compute_fbm_bounds();
        }
    }
}
