using System;
using System.Collections.Generic;
using System.Linq;
using SharpDX;

namespace RayTracing
{
    public class HitableList : IHitable
    {
        public IEnumerable<IHitable> Hitables;

        public HitableList(IEnumerable<IHitable> hitables)
        {
            Hitables = hitables;
        }

        public HitRecord? Hit(Ray ray, float tMin, float tMax)
        {
            HitRecord? hitRecord = null;
            var closestSoFar = tMax;

            foreach (var o in Hitables)
            {
                var result = o.Hit(ray, tMin, closestSoFar);
                if (result.HasValue)
                {
                    hitRecord = result;
                    closestSoFar = hitRecord.Value.T;
                }
            }

            return hitRecord;
        }

        public AABB BoundingBox(float time0, float time1)
        {
            if (!Hitables.Any())
            {
                return null;
            }

            var aabb0 = Hitables.ElementAt(0).BoundingBox(time0, time1);
            if (aabb0 == null)
            {
                return null;
            }

            foreach (var hitable in Hitables.Skip(1))
            {
                var aabb1 = hitable.BoundingBox(time0, time1);
                if (aabb1 == null)
                {
                    return null;
                }

                aabb0 = AABB.CalculateSurroundingBox(aabb0, aabb1);
            }

            return aabb0;
        }
    }
}