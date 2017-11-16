using System;
using SharpDX;

namespace RayTracing
{
    public class CheckerTexture : ITexture
    {
        public ITexture Odd { get; private set; }
        public ITexture Even { get; private set; }

        public CheckerTexture(ITexture odd, ITexture even)
        {
            Odd = odd;
            Even = even;
        }

        public Color3 Value(float u, float v, Vector3 position)
        {
            var sines = Math.Sin(10.0 * position.X) * Math.Sin(10.0 * position.Y) * Math.Sin(10.0 * position.Z);
            if (sines < 0)
            {
                return Odd.Value(u, v, position);
            }
            else
            {
                return Even.Value(u, v, position);
            }
        }
    }
}