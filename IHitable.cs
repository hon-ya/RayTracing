using SharpDX;

namespace RayTracing
{
    public struct HitRecord
    {
        public IHitable Hitable;
        public float T;
        public TexCoord TexCoord;
        public Vector3 Position;
        public Vector3 Normal;
        public IMaterial Material;
    }

    public interface IHitable
    {
        HitRecord? Hit(Ray ray, float tMin, float tMax);
        AABB GetBoundingBox(float time0, float time1);
    }
}
