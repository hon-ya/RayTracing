using SharpDX;
using System;
using System.IO;
using System.Text;

namespace RayTracing
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new StringBuilder();

            var nx = 200;
            var ny = 100;
            var ns = 100;

            builder.AppendFormat($"P3\n");
            builder.AppendFormat($"{nx} {ny}\n");
            builder.AppendFormat($"255\n");

            var lowerLeftCorner = new Vector3(-2.0f, -1.0f, -1.0f);
            var horizontal = new Vector3(4.0f, 0.0f, 0.0f);
            var vertical = new Vector3(0.0f, 2.0f, 0.0f);
            var origin = new Vector3(0.0f);

            var hitables = new IHitable[]
            {
                new Sphere(new Vector3(0.0f, 0.0f, -1.0f), 0.5f),
                new Sphere(new Vector3(0.0f, -100.5f, -1.0f), 100.0f),
            };
            var world = new HitableList(hitables);
            var camera = new Camera();
            var random = new Random();

            for (var j = ny - 1; j >= 0; j--)
            {
                for (var i = 0; i < nx; i++)
                {
                    var color = Color3.Black;

                    for (var s = 0; s < ns; s++)
                    {
                        var u = (1.0f * i + random.NextFloat(0.0f, 1.0f)) / nx;
                        var v = (1.0f * j + random.NextFloat(0.0f, 1.0f)) / ny;

                        var ray = camera.GetRay(u, v);
                        color += GetColor(ray, world, random);
                    }

                    color = Vector3.Divide(color, ns);
                    color = new Vector3((float) Math.Sqrt(color.Red), (float) Math.Sqrt(color.Green),
                        (float) Math.Sqrt(color.Blue));

                    var ir = (int)(255.99f * color.Red);
                    var ig = (int)(255.99f * color.Green);
                    var ib = (int)(255.99f * color.Blue);

                    builder.AppendFormat($"{ir} {ig} {ib}\n");
                }
            }

            File.WriteAllText("output.ppm", builder.ToString());
        }

        private static Color3 GetColor(Ray ray, HitableList world, Random random)
        {
            var result = world.Hit(ray, 0.001f, float.MaxValue);
            if (result.HasValue)
            {
                var record = result.Value;
                var target = record.Position + record.Normal + RandomInUnitSphere(random);
                return 0.5f * GetColor(new Ray(record.Position, target - record.Position), world, random);
            }
            else
            {
                var directionUnit = Vector3.Normalize(ray.Direction);
                var t = 0.5f * (directionUnit.Y + 1.0f);
                return (1.0f - t) * Color3.White + t * new Color3(0.5f, 0.7f, 1.0f);
            }
        }

        private static Vector3 RandomInUnitSphere(Random random)
        {
            while (true)
            {
                var p = 2.0f * random.NextVector3(Vector3.Zero, Vector3.One) - Vector3.One;
                if (p.Length() < 1.0f)
                {
                    return p;
                }
            }
        }
    }
}
