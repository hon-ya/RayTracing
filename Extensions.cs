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
                var p = random.NextVector3(-Vector3.One, Vector3.One);
                if (Vector3.Dot(p, p) < 1.0f)
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
                var p = random.NextVector3(-max, max);
                if (Vector3.Dot(p, p) < 1.0f)
                {
                    return p;
                }
            }
        }

        public static float NextInUnitFloat(this Random random)
        {
            return random.NextFloat(0.0f, 1.0f);
        }
    }
}
