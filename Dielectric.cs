using System;
using SharpDX;

namespace RayTracing
{
    public class Dielectric : MaterialBase
    {
        public float RefractiveIndex { get; set; }

        public Dielectric(float refractiveIndex)
        {
            RefractiveIndex = refractiveIndex;
        }

        public override ScatterRecord? Scatter(Ray rayIn, HitRecord hitRecord)
        {
            const float airRefractiveIndex = 1.0f;

            Vector3 outwardNormal;
            float niOverNt; // 屈折率の比
            float reflectProbe;
            float cos;
            Ray scattered;
            var reflected = Vector3.Reflect(rayIn.Direction, hitRecord.Normal);

            if (Vector3.Dot(rayIn.Direction, hitRecord.Normal) > 0)
            {
                // 物質から大気へ光が入る場合
                outwardNormal = -hitRecord.Normal;
                niOverNt = RefractiveIndex / airRefractiveIndex;
                cos = RefractiveIndex * Vector3.Dot(rayIn.Direction, hitRecord.Normal) / rayIn.Direction.Length();
            }
            else
            {
                // 大気から物質へ光が入る場合
                outwardNormal = hitRecord.Normal;
                niOverNt = airRefractiveIndex / RefractiveIndex;
                cos = -Vector3.Dot(rayIn.Direction, hitRecord.Normal) / rayIn.Direction.Length();
            }

            var result = Refract(rayIn.Direction, outwardNormal, niOverNt);
            if (result.HasValue)
            {
                // フレネル反射率に応じて、反射光と屈折光のどちらを返すかを決定する
                reflectProbe = Schlick(cos, RefractiveIndex);
                if (Base.Random.NextFloat(0.0f, 1.0f) <= reflectProbe)
                {
                    scattered = new Ray(hitRecord.Position, reflected);
                }
                else
                {
                    var refracted = result.Value;
                    scattered = new Ray(hitRecord.Position, refracted);
                }
            }
            else
            {
                scattered = new Ray(hitRecord.Position, reflected);
            }

            return new ScatterRecord { Scattered = scattered, Attenuation = Vector3.One };
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

        // Schlick approximation, フレネル反射率の近似
        private float Schlick(float cos, float refractiveIndex)
        {
            float r0 = (1.0f - refractiveIndex) / (1 + refractiveIndex);
            r0 = r0 * r0;

            return r0 + (1 - r0) * (float)Math.Pow(1 - cos, 5);
        }
    }
}