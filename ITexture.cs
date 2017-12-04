using SharpDX;

namespace RayTracing
{
    public interface ITexture
    {
        Color3 GetValue(TexCoord texCoord, Vector3 position);
    }
}