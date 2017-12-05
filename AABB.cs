using System;
using SharpDX;

namespace RayTracing
{
    // Axis-Aligned Bounding Box
    public class AABB
    {
        public Vector3 Min { get; private set; }
        public Vector3 Max { get; private set; }

        public AABB(Vector3 a, Vector3 b)
        {
            Min = a;
            Max = b;
        }

        public bool Hit(Ray ray, float tMin, float tMax)
        {
            for (var a = 0; a < 3; a++)
            {
                var t0 = (Min[a] - ray.Position[a]) / ray.Direction[a];
                var t1 = (Max[a] - ray.Position[a]) / ray.Direction[a];

                if (t1 < t0)
                {
                    Util.Swap(ref t0, ref t1);
                }

                var min = Math.Max(t0, tMin);
                var max = Math.Min(t1, tMax);

                if (max <= min)
                {
                    return false;
                }
            }

            return true;
        }

        public static AABB CalculateSurroundingBox(AABB box0, AABB box1)
        {
            var small = new Vector3(
                Math.Min(box0.Min.X, box1.Min.X),
                Math.Min(box0.Min.Y, box1.Min.Y),
                Math.Min(box0.Min.Z, box1.Min.Z)
            );
            var big = new Vector3(
                Math.Max(box0.Max.X, box1.Max.X),
                Math.Max(box0.Max.Y, box1.Max.Y),
                Math.Max(box0.Max.Z, box1.Max.Z)
            );

            return new AABB(small, big);
        }
    }
}