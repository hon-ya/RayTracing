namespace RayTracing
{
    public class Isotropic : MaterialBase
    {
        public ITexture Albedo { get; set; }

        public Isotropic(ITexture albedo)
        {
            Albedo = albedo;
        }

        public override ScatterRecord? Scatter(Ray rayIn, HitRecord hitRecord)
        {
            return new ScatterRecord()
            {
                Scattered = new Ray(hitRecord.Position, Base.Random.NextInUnitSphere()),
                Attenuation = Albedo.GetValue(hitRecord.TexCoord, hitRecord.Position),
            };
        }
    }
}