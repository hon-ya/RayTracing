﻿using SharpDX;

namespace RayTracing
{
    public class Metal : MaterialBase
    {
        public Vector3 Albedo { get; set; }
        public float Fuzz { get; set; }

        public Metal(Vector3 albedo, float fuzz)
        {
            Albedo = albedo;
            Fuzz = MathUtil.Clamp(fuzz, 0.0f, 1.0f);
        }

        public override ScatterRecord? Scatter(Ray rayIn, HitRecord hitRecord)
        {
            var reflected = Vector3.Reflect(rayIn.Direction, hitRecord.Normal);
            var scattered = new Ray(hitRecord.Position, reflected + Fuzz * Base.Random.NextSphere());
            var attenuation = Albedo;

            if (Vector3.Dot(scattered.Direction, hitRecord.Normal) > 0)
            {
                return new ScatterRecord {Scattered = scattered, Attenuation = attenuation};
            }
            else
            {
                return null;
            }
        }
    }
}