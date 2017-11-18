using SharpDX;

namespace RayTracing
{
    public struct TexCoord
    {
        public float U;
        public float V;

        public TexCoord(float u, float v)
        {
            U = u;
            V = v;
        }
    }
}