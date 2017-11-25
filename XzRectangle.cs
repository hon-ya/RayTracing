using System.Security.Policy;
using SharpDX;

namespace RayTracing
{
    public class XzRectangle : IHitable
    {
        public float X0 { get; }
        public float X1 { get; }
        public float Z0 { get; }
        public float Z1 { get; }
        public float K { get; }
        public IMaterial Material { get; }

        public XzRectangle(float x0, float x1, float z0, float z1, float k, IMaterial material)
        {
            X0 = x0;
            X1 = x1;
            Z0 = z0;
            Z1 = z1;
            K = k;
            Material = material;
        }

        public HitRecord? Hit(Ray ray, float tMin, float tMax)
        {
            var t = (K - ray.Position.Y) / ray.Direction.Y;
            if (t < tMin || tMax < t)
            {
                return null;
            }

            var x = ray.Position.X + t * ray.Direction.X;
            var z = ray.Position.Z + t * ray.Direction.Z;

            if (x < X0 || X1 < x || z < Z0 || Z1 < z)
            {
                return null;
            }

            return new HitRecord
            {
                T = t,
                TexCoord =
                {
                    U = (x - X0) / (X1 - X0),
                    V = (z - Z0) / (Z1 - Z0)
                },
                Material = Material,
                Normal = Vector3.UnitY,
                Position = ray.GetPoint(t),
            };
        }

        public AABB BoundingBox(float time0, float time1)
        {
            return new AABB(new Vector3(X0, Z0, K - 0.0001f), new Vector3(X1, Z1, K + 0.0001f));
        }
    }
}