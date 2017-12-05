using System.Security.Policy;
using SharpDX;

namespace RayTracing
{
    public class XyRectangle : IHitable
    {
        public float X0 { get; }
        public float X1 { get; }
        public float Y0 { get; }
        public float Y1 { get; }
        public float K { get; }
        public IMaterial Material { get; }

        public XyRectangle(float x0, float x1, float y0, float y1, float k, IMaterial material)
        {
            X0 = x0;
            X1 = x1;
            Y0 = y0;
            Y1 = y1;
            K = k;
            Material = material;
        }

        public HitRecord? Hit(Ray ray, float tMin, float tMax)
        {
            var t = (K - ray.Position.Z) / ray.Direction.Z;
            if (t < tMin || tMax < t)
            {
                return null;
            }

            var x = ray.Position.X + t * ray.Direction.X;
            var y = ray.Position.Y + t * ray.Direction.Y;

            if (x < X0 || X1 < x || y < Y0 || Y1 < y)
            {
                return null;
            }

            return new HitRecord
            {
                Hitable = this,
                T = t,
                TexCoord =
                {
                    U = (x - X0) / (X1 - X0),
                    V = (y - Y0) / (Y1 - Y0)
                },
                Material = Material,
                Normal = Vector3.UnitZ,
                Position = ray.GetPoint(t),
            };
        }

        public AABB GetBoundingBox(float time0, float time1)
        {
            return new AABB(new Vector3(X0, Y0, K - 0.0001f), new Vector3(X1, Y1, K + 0.0001f));
        }
    }
}