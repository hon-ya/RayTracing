using SharpDX;

namespace RayTracing
{
    public class Metal : IMaterial
    {
        public Vector3 Albedo { get; set; }

        public Metal(Vector3 albedo)
        {
            Albedo = albedo;
        }

        public ScatterRecord? Scatter(Ray rayIn, HitRecord hitRecord)
        {
            var reflected = Vector3.Reflect(rayIn.Direction, hitRecord.Normal);
            var scattered = new Ray(hitRecord.Position, reflected);
            var attenuation = Albedo;

            if (Vector3.Dot(scattered.Direction, hitRecord.Normal) > 0)
            {
                return new ScatterRecord {Scattered = scattered, Attenuation = attenuation};
            }
            else
            {
                return null;
            }
        }
    }
}