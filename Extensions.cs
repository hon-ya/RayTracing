using SharpDX;

namespace RayTracing
{
    static class Extensions
    {
        public static Vector3 GetPoint(this Ray ray, float t)
        {
            return ray.Position + t * ray.Direction;
        }
    }
}
