using SharpDX;
using System;

namespace RayTracing
{
    public class Sphere : IHitable
    {
        public Vector3 Center { get; set; }
        public float Radius { get; set; }
        public IMaterial Material { get; set; }

        public Sphere(Vector3 center, float radius, IMaterial material)
        {
            Center = center;
            Radius = radius;
            Material = material;
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
                    var texCoord = GetSphereUv((position - Center) / Radius);

                    return new HitRecord { Hitable = this, T = t, TexCoord = texCoord, Position = position, Normal = normal, Material = Material };
                }

                var t2 = (-b + (float)Math.Sqrt(discriminant)) / a;
                if (tMin < t2 && t2 < tMax)
                {
                    var t = t2;
                    var position = ray.GetPoint(t);
                    var normal = (position - Center) / Radius;
                    var texCoord = GetSphereUv(position);

                    return new HitRecord { Hitable = this, T = t, TexCoord = texCoord, Position = position, Normal = normal, Material = Material };
                }
            }

            return null;
        }

        public AABB GetBoundingBox(float time0, float time1)
        {
            return new AABB(Center - Radius, Center + Radius);
        }

        private TexCoord GetSphereUv(Vector3 position)
        {
            var phi = (float)Math.Atan2(position.Z, position.X);
            var theta = (float)Math.Asin(position.Y);

            return new TexCoord()
            {
                U = 1 - (phi + MathUtil.Pi) / (2 * MathUtil.Pi),
                V = (theta + MathUtil.PiOverTwo) / MathUtil.Pi,
            };
        }
    }
}
