using SharpDX;

namespace RayTracing
{
    public class ConstantTexture : ITexture
    {
        public Color3 Color { get; private set; }

        public ConstantTexture(Color3 color)
        {
            Color = color;
        }

        public Color3 Value(float u, float v, Vector3 position)
        {
            return Color;
        }
    }
}