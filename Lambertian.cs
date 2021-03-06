﻿using System;
using SharpDX;

namespace RayTracing
{
    public class Lambertian : MaterialBase
    {
        public ITexture Albedo { get; set; }

        public Lambertian(ITexture albedo)
        {
            Albedo = albedo;
        }

        public override ScatterRecord? Scatter(Ray rayIn, HitRecord hitRecord)
        {
            var target = hitRecord.Position + hitRecord.Normal + Base.Random.NextSphere();
            var scattered = new Ray(hitRecord.Position, target - hitRecord.Position, rayIn.Time);
            var attenuation = Albedo.GetValue(hitRecord.TexCoord, hitRecord.Position);

            return new ScatterRecord {Scattered = scattered, Attenuation = attenuation};
        }
    }
}