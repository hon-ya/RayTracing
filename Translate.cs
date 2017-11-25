using SharpDX;

namespace RayTracing
{
    public class Translate : IHitable
    {
        public IHitable Hitable { get; set; }
        public Vector3 Offset { get; set; }

        public Translate(IHitable hitable, Vector3 offset)
        {
            Hitable = hitable;
            Offset = offset;
        }

        public HitRecord? Hit(Ray ray, float tMin, float tMax)
        {
            var rayMoved = new Ray(ray.Position - Offset, ray.Direction, ray.Time);
            var result = Hitable.Hit(rayMoved, tMin, tMax);
            if (result.HasValue)
            {
                var hitRecord = result.Value;
                hitRecord.Position += Offset;

                return hitRecord;
            }
            else
            {
                return null;
            }
        }

        public AABB GetBoundingBox(float time0, float time1)
        {
            var box = Hitable.GetBoundingBox(time0, time1);
            if (box != null)
            {
                return new AABB(box.Min + Offset, box.Max + Offset);
            }
            else
            {
                return null;
            }
        }
    }
}