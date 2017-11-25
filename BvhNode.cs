using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace RayTracing
{
    public class BvhNode : IHitable
    {
        public AABB Box { get; private set; }
        public IHitable Left { get; private set; }
        public IHitable Right { get; private set; }

        public BvhNode(List<IHitable> hitables, float time0, float time1)
        {
            var axis = (int) (3 * Base.Random.NextInUnitFloat());

            hitables.Sort((x, y) => BoxComparison(x, y, axis));

            switch (hitables.Count)
            {
                case 1:
                    {
                        Left = Right = hitables[0];
                    }
                    break;
                case 2:
                    {
                        Left = hitables[0];
                        Right = hitables[1];
                    }
                    break;
                default:
                    {
                        Left = new BvhNode(hitables.GetRange(0, hitables.Count / 2), time0, time1);
                        Right = new BvhNode(hitables.GetRange(hitables.Count / 2, hitables.Count - hitables.Count / 2), time0, time1);
                    }
                    break;
            }

            var boxLeft = Left.GetBoundingBox(time0, time1);
            var boxRight = Right.GetBoundingBox(time0, time1);

            if (boxLeft == null || boxRight == null)
            {
                throw new Exception("boxLeft or boxRight is null");
            }

            Box = AABB.CalculateSurroundingBox(boxLeft, boxRight);
        }

        public HitRecord? Hit(Ray ray, float tMin, float tMax)
        {
            if (Box.Hit(ray, tMin, tMax))
            {
                var hitLeft = Left.Hit(ray, tMin, tMax);
                var hitRight = Right.Hit(ray, tMin, tMax);

                if (hitLeft.HasValue && hitRight.HasValue)
                {
                    var leftRecord = hitLeft.Value;
                    var rightRecord = hitRight.Value;

                    if (leftRecord.T < rightRecord.T)
                    {
                        return leftRecord;
                    }
                    else
                    {
                        return rightRecord;
                    }
                }

                return hitLeft ?? hitRight;
            }
            else
            {
                return null;
            }
        }

        public AABB GetBoundingBox(float time0, float time1)
        {
            return Box;
        }

        private static int BoxComparison(IHitable left, IHitable right, int axis)
        {
            var boxLeft = left.GetBoundingBox(0, 0);
            var boxRight = right.GetBoundingBox(0, 0);

            if (boxLeft == null || boxRight == null)
            {
                throw new Exception("boxLeft or boxRight is null");
            }

            if ((boxLeft.Min[axis] - boxRight.Min[axis]) < 0.0f)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }
    }
}