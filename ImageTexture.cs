using System;
using SharpDX;

namespace RayTracing
{
    public class ImageTexture : ITexture
    {
        public byte[] Pixels { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public ImageTexture(byte[] pixels, int width, int height)
        {
            Pixels = pixels;
            Width = width;
            Height = height;
        }

        public Color3 Value(TexCoord texCoord, Vector3 position)
        {
            var i = (int) (texCoord.U * Width);
            var j = (int) ((1 - texCoord.V) * Height - 0.001);

            i = MathUtil.Clamp(i, 0, Width - 1);
            j = MathUtil.Clamp(j, 0, Height - 1);

            var r = Pixels[3 * i + 3 * Width * j + 0] / 255.0f;
            var g = Pixels[3 * i + 3 * Width * j + 1] / 255.0f;
            var b = Pixels[3 * i + 3 * Width * j + 2] / 255.0f;

            return new Color3(r, g, b);
        }
    }
}