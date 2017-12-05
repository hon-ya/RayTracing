using System;
using System.Linq;
using SharpDX;

namespace RayTracing
{
    public class RotateY : IHitable
    {
        public IHitable Hitable { get; set; }
        public float CosTheta { get; set; }
        public float SinTheta { get; set; }
        public AABB BoundingBox { get; set; }

        public RotateY(IHitable hitable, float rotationRadian)
        {
            Hitable = hitable;

            SinTheta = (float)Math.Sin(rotationRadian);
            CosTheta = (float)Math.Cos(rotationRadian);
            var box = hitable.GetBoundingBox(0.0f, 1.0f);

            var min = new Vector3(float.MaxValue);
            var max = new Vector3(float.MinValue);

            foreach (var (i, j, k) in Util.GenerateIndex(2, 2, 2))
            {
                var x = i == 0 ? box.Min.X : box.Max.X;
                var y = j == 0 ? box.Min.Y : box.Max.Y;
                var z = k == 0 ? box.Min.Z : box.Max.Z;

                var xNew = CosTheta * x + SinTheta * z;
                var zNew = -SinTheta * x + CosTheta * z;

                var tester = new Vector3(xNew, y, zNew);

                min = Vector3.Min(tester, min);
                max = Vector3.Max(tester, max);
            }

            BoundingBox = new AABB(min, max);
        }

        public AABB GetBoundingBox(float time0, float time1)
        {
            return BoundingBox;
        }

        public HitRecord? Hit(Ray ray, float tMin, float tMax)
        {
            var position = ray.Position;
            var direction = ray.Direction;

            // 指定角度に対し、オブジェクトを回転させるのではなく、ray を逆回転させる。
            position[0] = CosTheta * ray.Position[0] - SinTheta * ray.Position[2];
            position[2] = SinTheta * ray.Position[0] + CosTheta * ray.Position[2];
            direction[0] = CosTheta * ray.Direction[0] - SinTheta * ray.Direction[2];
            direction[2] = SinTheta * ray.Direction[0] + CosTheta * ray.Direction[2];

            var rayRotated = new Ray(position, direction, ray.Time);

            var result = Hitable.Hit(rayRotated, tMin, tMax);
            if (result.HasValue)
            {
                var hitRecord = result.Value;
                var hitRecordRotated = hitRecord;

                // ヒット位置等を指定角度だけ回転させる。
                hitRecordRotated.Position[0] = CosTheta * hitRecord.Position[0] + SinTheta * hitRecord.Position[2];
                hitRecordRotated.Position[2] = -SinTheta * hitRecord.Position[0] + CosTheta * hitRecord.Position[2];
                hitRecordRotated.Normal[0] = CosTheta * hitRecord.Normal[0] + SinTheta * hitRecord.Normal[2];
                hitRecordRotated.Normal[2] = -SinTheta * hitRecord.Normal[0] + CosTheta * hitRecord.Normal[2];

                return hitRecordRotated;
            }
            else
            {
                return null;
            }
        }
    }
}