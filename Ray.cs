using SharpDX;

namespace RayTracing
{
    public class Ray
    {
        public Vector3 Position { get; private set; }
        public Vector3 Direction { get; private set; }
        public float Time { get; private set; }

        public Ray(Vector3 position, Vector3 direction, float time = 0.0f)
        {
            Position = position;
            Direction = direction;
            Time = time;
        }

        public Vector3 GetPoint(float t)
        {
            return Position + t * Direction;
        }
    }
}