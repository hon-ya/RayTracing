using SharpDX;
using System;

namespace RayTracing
{
    public class MovingSphere : IHitable
    {
        public Vector3 Center0 { get; set; }
        public Vector3 Center1 { get; set; }
        public float Time0 { get; set; }
        public float Time1 { get; set; }
        public float Radius { get; set; }
        public IMaterial Material { get; set; }

        public MovingSphere(Vector3 center0, Vector3 center1, float time0, float time1, float radius, IMaterial material)
        {
            Center0 = center0;
            Center1 = center1;
            Time0 = time0;
            Time1 = time1;
            Radius = radius;
            Material = material;
        }

        public Vector3 GetCenter(float time)
        {
            return Center0 + ((time - Time0) / (Time1 - Time0)) * (Center1 - Center0);
        }

        public HitRecord? Hit(Ray ray, float tMin, float tMax)
        {
            var center = GetCenter(ray.Time);
            var oc = ray.Position - center;
            var a = Vector3.Dot(ray.Direction, ray.Direction);
            var b = Vector3.Dot(oc, ray.Direction);
            var c = Vector3.Dot(oc, oc) - Radius * Radius;
            var discriminant = b * b - a * c;

            if (discriminant > 0)
            {
                var t1 = (-b - (float)Math.Sqrt(discriminant)) / a;
                if (tMin < t1 && t1 < tMax)
                {
                    var t = t1;
                    var position = ray.GetPoint(t);
                    var normal = (position - center) / Radius;

                    return new HitRecord { T = t, Position = position, Normal = normal, Material = Material };
                }

                var t2 = (-b + (float)Math.Sqrt(discriminant)) / a;
                if (tMin < t2 && t2 < tMax)
                {
                    var t = t2;
                    var position = ray.GetPoint(t);
                    var normal = (position - center) / Radius;

                    return new HitRecord { T = t, Position = position, Normal = normal, Material = Material };
                }
            }

            return null;
        }
    }
}
