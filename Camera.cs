using System;
using SharpDX;

namespace RayTracing
{
    public class Camera
    {
        public Vector3 LowerLeftCorner { get; private set; }
        public Vector3 Horizontal { get; private set; }
        public Vector3 Vertical { get; private set; }
        public Vector3 Origin { get; private set; }

        public Camera(Vector3 lookFrom, Vector3 lookAt, Vector3 up, float vfov, float aspect)
        {
            var theta = MathUtil.DegreesToRadians(vfov);
            var halfHeight = (float)Math.Tan(theta / 2.0f);
            var halfWidth = aspect * halfHeight;

            Origin = lookFrom;

            var w = Vector3.Normalize(lookFrom - lookAt);
            var u = Vector3.Normalize(Vector3.Cross(Vector3.Up, w));
            var v = Vector3.Cross(w, u);

            LowerLeftCorner = Origin - halfWidth * u - halfHeight * v - w;
            Horizontal = 2.0f * halfWidth * u;
            Vertical = 2.0f * halfHeight * v;
        }

        public Ray GetRay(float u, float v)
        {
            return new Ray(Origin, LowerLeftCorner + u * Horizontal + v * Vertical - Origin);
        }
    }
}