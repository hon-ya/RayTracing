using SharpDX;
using System;

namespace RayTracing
{
    static class Extensions
    {
        public static Vector3 GetPoint(this Ray ray, float t)
        {
            return ray.Position + t * ray.Direction;
        }

        public static Vector3 NextInUnitSphere(this Random random)
        {
            while (true)
            {
                var p = 2.0f * random.NextVector3(Vector3.Zero, Vector3.One) - Vector3.One;
                if (p.Length() < 1.0f)
                {
                    return p;
                }
            }
        }


        public static Vector3 NextInUnitDisk(this Random random)
        {
            while (true)
            {
                var max = new Vector3(1.0f, 1.0f, 0.0f);
                var p = 2.0f * random.NextVector3(Vector3.Zero, max) - max;
                if (Vector3.Dot(p, p) < 1.0f)
                {
                    return p;
                }
            }
        }
    }
}
