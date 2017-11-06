using System;
using SharpDX;

namespace RayTracing
{
    public class Camera
    {
        public Vector3 LowerLeftCorner { get; private set; }
        public Vector3 Horizontal { get; private set; }
        public Vector3 Vertical { get; private set; }
        public Vector3 Origin { get; private set; } = new Vector3(0.0f);

        public Camera(float vfov, float aspect)
        {
            var theta = MathUtil.DegreesToRadians(vfov);
            var halfHeight = (float)Math.Tan(theta / 2.0f);
            var halfWidth = aspect * halfHeight;

            LowerLeftCorner = new Vector3(-halfWidth, -halfHeight, -1.0f);
            Horizontal = new Vector3(2.0f * halfWidth, 0.0f, 0.0f);
            Vertical = new Vector3(0.0f, 2.0f * halfHeight, 0.0f);
        }

        public Ray GetRay(float u, float v)
        {
            return new Ray(Origin, LowerLeftCorner + u * Horizontal + v * Vertical - Origin);
        }
    }
}