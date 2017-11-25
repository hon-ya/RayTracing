using System.Security.Policy;
using SharpDX;

namespace RayTracing
{
    public class YzRectangle : IHitable
    {
        public float Y0 { get; }
        public float Y1 { get; }
        public float Z0 { get; }
        public float Z1 { get; }
        public float K { get; }
        public IMaterial Material { get; }

        public YzRectangle(float y0, float y1, float z0, float z1, float k, IMaterial material)
        {
            Y0 = y0;
            Y1 = y1;
            Z0 = z0;
            Z1 = z1;
            K = k;
            Material = material;
        }

        public HitRecord? Hit(Ray ray, float tMin, float tMax)
        {
            var t = (K - ray.Position.X) / ray.Direction.X;
            if (t < tMin || tMax < t)
            {
                return null;
            }

            var y = ray.Position.Y + t * ray.Direction.Y;
            var z = ray.Position.Z + t * ray.Direction.Z;

            if (y < Y0 || Y1 < y || z < Z0 || Z1 < z)
            {
                return null;
            }

            return new HitRecord
            {
                T = t,
                TexCoord =
                {
                    U = (y - Y0) / (Y1 - Y0),
                    V = (z - Z0) / (Z1 - Z0)
                },
                Material = Material,
                Normal = Vector3.UnitX,
                Position = ray.GetPoint(t),
            };
        }

        public AABB BoundingBox(float time0, float time1)
        {
            return new AABB(new Vector3(Y0, Z0, K - 0.0001f), new Vector3(Y1, Z1, K + 0.0001f));
        }
    }
}