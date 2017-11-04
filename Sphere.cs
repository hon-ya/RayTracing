using SharpDX;
using System;

namespace RayTracing
{
    public class Sphere : IHitable
    {
        public Vector3 Center { get; set; }
        public float Radius { get; set; }

        public Sphere(Vector3 center, float radius)
        {
            Center = center;
            Radius = radius;
        }

        public HitRecord? Hit(Ray ray, float tMin, float tMax)
        {
            var oc = ray.Position - Center;
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
                    var normal = (position - Center) / Radius;

                    return new HitRecord { T = t, Position = position, Normal = normal };
                }

                var t2 = (-b + (float)Math.Sqrt(discriminant)) / a;
                if (tMin < t2 && t2 < tMax)
                {
                    var t = t2;
                    var position = ray.GetPoint(t);
                    var normal = (position - Center) / Radius;

                    return new HitRecord { T = t, Position = position, Normal = normal };
                }
            }

            return null;
        }
    }
}
