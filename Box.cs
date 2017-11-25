using SharpDX;

namespace RayTracing
{
    public class Box : IHitable
    {
        public Vector3 PositionMin { get; set; }
        public Vector3 PositionMax { get; set; }
        public HitableList Hitables { get; set; }

        public Box(Vector3 position0, Vector3 position1, IMaterial material)
        {
            PositionMin = position0;
            PositionMax = position1;

            Hitables = new HitableList()
            {
                new XyRectangle(position0.X, position1.X, position0.Y, position1.Y, position1.Z, material),
                new FlipNormals(new XyRectangle(position0.X, position1.X, position0.Y, position1.Y, position0.Z, material)),
                new XzRectangle(position0.X, position1.X, position0.Z, position1.Z, position1.Y, material),
                new FlipNormals(new XzRectangle(position0.X, position1.X, position0.Z, position1.Z, position0.Y, material)),
                new YzRectangle(position0.Y, position1.Y, position0.Z, position1.Z, position1.X, material),
                new FlipNormals(new YzRectangle(position0.Y, position1.Y, position0.Z, position1.Z, position0.X, material)),
            };
        }

        public HitRecord? Hit(Ray ray, float tMin, float tMax)
        {
            return Hitables.Hit(ray, tMin, tMax);
        }

        public AABB GetBoundingBox(float time0, float time1)
        {
            return Hitables.GetBoundingBox(time0, time1);
        }
    }
}