using SharpDX;

namespace RayTracing
{
    public class DiffuseLight : MaterialBase
    {
        public ITexture Emit { get; private set; }

        public DiffuseLight(ITexture emit)
        {
            Emit = emit;
        }

        public override ScatterRecord? Scatter(Ray rayIn, HitRecord hitRecord)
        {
            return null;
        }

        public override Color3 Emitted(TexCoord texCoord, Vector3 position)
        {
            return Emit.Value(texCoord, position);
        }
    }
}