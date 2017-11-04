using System;
using SharpDX;

namespace RayTracing
{
    public class Lambertian : IMaterial
    {
        public Vector3 Albedo { get; set; }

        public Lambertian(Vector3 albedo)
        {
            Albedo = albedo;
        }

        public ScatterRecord? Scatter(Ray rayIn, HitRecord hitRecord)
        {
            var target = hitRecord.Position + hitRecord.Normal + Base.Random.NextInUnitSphere();
            var scattered = new Ray(hitRecord.Position, target - hitRecord.Position);
            var attenuation = Albedo;

            return new ScatterRecord {Scattered = scattered, Attenuation = attenuation};
        }
    }
}