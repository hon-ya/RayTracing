using SharpDX;

namespace RayTracing
{
    public class Camera
    {
        public Vector3 LowerLeftCorner { get; private set; } = new Vector3(-2.0f, -1.0f, -1.0f);
        public Vector3 Horizontal { get; private set; } = new Vector3(4.0f, 0.0f, 0.0f);
        public Vector3 Vertical { get; private set; } = new Vector3(0.0f, 2.0f, 0.0f);
        public Vector3 Origin { get; private set; } = new Vector3(0.0f);

        public Ray GetRay(float u, float v)
        {
            return new Ray(Origin, LowerLeftCorner + u * Horizontal + v * Vertical);
        }
    }
}