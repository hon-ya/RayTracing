using System;
using SharpDX;

namespace RayTracing
{
    public class NoiseTexture : ITexture
    {
        public float Scale { get; private set; }

        public NoiseTexture(float scale)
        {
            Scale = scale;
        }

        public Color3 Value(float u, float v, Vector3 position)
        {
            //return Color3.White * Perlin.Noise(Scale * position);
            //return Color3.White * 0.5f * (1.0f + Perlin.Noise(Scale * position));
            //return Color3.White * Perlin.Turbulence(Scale * position);
            //return Color3.White * 0.5f * (1.0f + Perlin.Turbulence(Scale * position));
            return Color3.White * 0.5f * (1.0f + (float)Math.Sin(Scale * position.Z + 10.0f * Perlin.Turbulence(position)));
        }
    }
}