using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Disque.Math;

namespace Disque.Raytracer.Textures.NoiseBased
{
    public class Wood : Texture
    {
        LatticeNoise noise_ptr;
        Vector3 light_color, dark_color;
        float ring_frequency, ring_uneveness, ring_noise, ring_noise_frequency, trunk_wobble;
        float trunk_wobble_frequency, angular_wobble, angular_wobble_frequency, grain_frequency, grainy, ringy;

        public Wood(
            LatticeNoise _noise_ptr, Vector3 _light_color, Vector3 _dark_color, float _ring_frequency,
            float _ring_uneveness, float _ring_noise, float _ring_noise_frequency, float _trunk_wobble,
            float _trunk_wobble_frequency, float _angular_wobble, float _angular_wobble_frequency,
            float _grain_frequency, float _grainy, float _ringy)
            : base()
        {
            noise_ptr = _noise_ptr;
            light_color = _light_color;
            dark_color = _dark_color;
            ring_frequency = _ring_frequency;
            ring_uneveness = _ring_uneveness;
            ring_noise = _ring_noise;
            ring_noise_frequency = _ring_noise_frequency;
            trunk_wobble = _trunk_wobble;
            trunk_wobble_frequency = _trunk_wobble_frequency;
            angular_wobble = _angular_wobble;
            angular_wobble_frequency = _angular_wobble_frequency;
            grain_frequency = _grain_frequency;
            grainy = _grainy;
            ringy = _ringy;
        }

        public Wood(Vector3 light, Vector3 dark)
            : this(
            new LatticeNoise(2, 4.0f, 0.5f), light, dark, 
            4.0f, 0.25f, 0.02f, 1.0f, 0.15f, 
            0.025f, 0.5f, 1.0f, 25.0f, 0.5f, 0.5f)
        {
        }

        public override Vector3 GetColor(ShadeRec sr)
        {
            Vector3 hit_point = sr.LocalHitPoint;
            Vector3 offset = noise_ptr.vector_fbm(hit_point * ring_noise_frequency);
            Vector3 ring_point = hit_point + ring_noise * offset;
            Vector3 temp_vec = trunk_wobble * noise_ptr.vector_noise(new Vector3(0, 0, hit_point.Y * trunk_wobble_frequency));
            ring_point.X += temp_vec.X;
            ring_point.Z += temp_vec.X;
            float r = MathHelper.Sqrt(ring_point.X * ring_point.X + ring_point.Z * ring_point.Z) * ring_frequency;
            Vector3 temp_vec2 = Vector3.Zero;
            temp_vec2.X = angular_wobble_frequency * ring_point.X;
            temp_vec2.Y = angular_wobble_frequency * ring_point.Y * 0.1f;
            temp_vec2.Z = angular_wobble_frequency * ring_point.Z;
            float delta_r = angular_wobble * MathHelper.SmoothStep(0.0f, 5.0f, r) * noise_ptr.value_noise(temp_vec2);
            r += delta_r;
            r += ring_uneveness * noise_ptr.value_noise(new Vector3(r));
            float temp = r;
            float in_ring = MathHelper.SmoothPulseTrain(0.1f, 0.55f, 0.7f, 0.95f, 1.0f, r);
            Vector3 grain_point = Vector3.Zero;
            grain_point.X = hit_point.X * grain_frequency;
            grain_point.Y = hit_point.Y * grain_frequency * 0.05f;
            grain_point.Z = hit_point.Z * grain_frequency;
            float dpgrain = 0.2f;
            float grain = 0.0f;
            float amplitude = 1.0f;
            for (int i = 0; i < 2; i++)
            {
                float grain_valid = 1.0f - MathHelper.SmoothStep(0.2f, 0.6f, dpgrain);
                if (grain_valid > 0.0f)
                {
                    float g = grain_valid * noise_ptr.value_noise(grain_point);
                    g *= (0.3f + 0.7f * in_ring);
                    g = MathHelper.Pow(MathHelper.Clamp(0.8f - g, 0.0f, 1.0f), 2.0f);
                    g = grainy * MathHelper.SmoothStep(0.5f, 1.0f, g);
                    if (i == 0)
                        in_ring *= (1.0f - 0.4f * grain_valid);
                    grain = amplitude * MathHelper.Max(grain, g);
                }
                grain_point = 2.0f * grain_point;
                dpgrain *= 2.0f;
                amplitude *= 0.5f;
            }
            float final_value = MathHelper.MixFloat(in_ring * ringy, 1.0f, grain);
            return (MathHelper.MixColor(light_color, dark_color, final_value));
        }
    }
}
