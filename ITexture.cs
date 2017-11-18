using SharpDX;

namespace RayTracing
{
    public interface ITexture
    {
        Color3 Value(TexCoord texCoord, Vector3 position);
    }
}