using SharpDX;

namespace RayTracing
{
    public interface ITexture
    {
        Color3 Value(float u, float v, Vector3 position);
    }
}