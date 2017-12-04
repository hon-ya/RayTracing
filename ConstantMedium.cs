using System;
using SharpDX;

namespace RayTracing
{
    public class ConstantMedium : IHitable
    {
        public ConstantMedium(IHitable boundary, float density, ITexture albedo)
        {
            Boundary = boundary;
            Density = density;
            PhaseFunction = new Isotropic(albedo);
        }

        public Isotropic PhaseFunction { get; set; }

        public float Density { get; set; }

        public IHitable Boundary { get; set; }

        public HitRecord? Hit(Ray ray, float tMin, float tMax)
        {
            // オブジェクト表面のヒット箇所を探す
            var result1 = Boundary.Hit(ray, float.MinValue, float.MaxValue);
            if (result1.HasValue)
            {
                var record1 = result1.Value;

                // 表側ヒット箇所を貫通し、閉じたオブジェクトの中を進んだ先にある裏側のヒット箇所を探す
                var result2 = Boundary.Hit(ray, record1.T + 0.0001f, float.MaxValue);
                if (result2.HasValue)
                {
                    var record2 = result2.Value;

                    record1.T = Math.Max(record1.T, tMin);
                    record2.T = Math.Min(record2.T, tMax);

                    if (record1.T >= record2.T)
                    {
                        return null;
                    }

                    record1.T = Math.Max(record1.T, 0);

                    // 表と裏のヒット位置から、オブジェクト内を進んだ距離を得る
                    var distanceInsideBoundary = (record2.T - record1.T) * ray.Direction.Length();
                    // オブジェクトの密度から、ヒットとするランダムな閾値を計算する
                    var hitDistance = -(1.0f / Density) * (float)Math.Log(Base.Random.NextInUnitFloat());

                    // 閾値を超えている場合、オブジェクトにヒットしたものとする
                    if (hitDistance < distanceInsideBoundary)
                    {
                        var t = record1.T + hitDistance / ray.Direction.Length();

                        return new HitRecord
                        {
                            Hitable = this,
                            T = t,
                            Position = ray.GetPoint(t),
                            Normal = Vector3.UnitX,
                            Material = PhaseFunction,
                        };
                    }
                }
            }

            return null;
        }

        public AABB GetBoundingBox(float time0, float time1)
        {
            return Boundary.GetBoundingBox(time0, time1);
        }
    }
}