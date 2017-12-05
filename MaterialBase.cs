using SharpDX;

namespace RayTracing
{
    public abstract class MaterialBase : IMaterial
    {
        public abstract ScatterRecord? Scatter(Ray rayIn, HitRecord hitRecord);

        public virtual Color3 Emitted(TexCoord texCoord, Vector3 position)
        {
            return Color3.Black;
        }
    }
}