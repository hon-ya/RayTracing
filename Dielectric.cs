using System;
using SharpDX;

namespace RayTracing
{
    public class Dielectric : IMaterial
    {
        public float RefractiveIndex { get; set; }

        public Dielectric(float refractiveIndex)
        {
            RefractiveIndex = refractiveIndex;
        }

        public ScatterRecord? Scatter(Ray rayIn, HitRecord hitRecord)
        {
            const float airRefractiveIndex = 1.0f;

            Vector3 outwardNormal;
            float niOverNt; // 屈折率の比

            if (Vector3.Dot(rayIn.Direction, hitRecord.Normal) > 0)
            {
                // 物質から大気へ光が入る場合
                outwardNormal = -hitRecord.Normal;
                niOverNt = RefractiveIndex / airRefractiveIndex;
            }
            else
            {
                // 大気から物質へ光が入る場合
                outwardNormal = hitRecord.Normal;
                niOverNt = airRefractiveIndex / RefractiveIndex;
            }

            var result = Refract(rayIn.Direction, outwardNormal, niOverNt);
            if (result.HasValue)
            {
                var refracted = result.Value;
                var scattered = new Ray(hitRecord.Position, refracted);
                return new ScatterRecord {Scattered = scattered, Attenuation = Vector3.One};
            }
            else
            {
                return null;
            }
        }

        // スネルの法則
        private Vector3? Refract(Vector3 v, Vector3 n, float niOverNt)
        {
            var uv = Vector3.Normalize(v);
            var dt = Vector3.Dot(uv, n);
            var discriminant = 1.0f - niOverNt * niOverNt * (1.0f - dt * dt);
            if (discriminant > 0)
            {
                return niOverNt * (uv - n * dt) - n * (float)Math.Sqrt(discriminant);
            }
            else
            {
                return null;
            }
        }
    }
}