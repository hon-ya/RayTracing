using System;

namespace RayTracing
{
    public class FlipNormals : IHitable
    {
        public IHitable Hitable { get; set; }

        public FlipNormals(IHitable hitable)
        {
            Hitable = hitable;
        }

        public HitRecord? Hit(Ray ray, float tMin, float tMax)
        {
            var result = Hitable.Hit(ray, tMin, tMax);
            if (result.HasValue)
            {
                var hitRecord = result.Value;
                hitRecord.Normal = -hitRecord.Normal;

                return hitRecord;
            }
            else
            {
                return null;
            }
        }

        public AABB BoundingBox(float time0, float time1)
        {
            return Hitable.BoundingBox(time0, time1);
        }
    }
}