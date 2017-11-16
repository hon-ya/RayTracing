﻿using System;
using SharpDX;

namespace RayTracing
{
    public class Lambertian : IMaterial
    {
        public ITexture Albedo { get; set; }

        public Lambertian(ITexture albedo)
        {
            Albedo = albedo;
        }

        public ScatterRecord? Scatter(Ray rayIn, HitRecord hitRecord)
        {
            var target = hitRecord.Position + hitRecord.Normal + Base.Random.NextInUnitSphere();
            var scattered = new Ray(hitRecord.Position, target - hitRecord.Position, rayIn.Time);
            var attenuation = Albedo.Value(0, 0, hitRecord.Position);

            return new ScatterRecord {Scattered = scattered, Attenuation = attenuation};
        }
    }
}