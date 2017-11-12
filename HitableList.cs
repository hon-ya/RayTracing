using System;
using System.Collections.Generic;
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
    }
}