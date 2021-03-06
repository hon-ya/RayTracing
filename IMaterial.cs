﻿using SharpDX;

namespace RayTracing
{
    public struct ScatterRecord
    {
        public Ray Scattered;
        public Vector3 Attenuation;
    }

    public interface IMaterial
    {
        ScatterRecord? Scatter(Ray rayIn, HitRecord hitRecord);

        Color3 Emitted(TexCoord texCoord, Vector3 position);
    }
}