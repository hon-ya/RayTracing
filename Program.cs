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

            for (var j = ny - 1; j >= 0; j--)
            {
                for (var i = 0; i < nx; i++)
                {
                    var u = 1.0f * i / nx;
                    var v = 1.0f * j / ny;

                    var ray = new Ray(origin, lowerLeftCorner + u * horizontal + v * vertical);
                    var color = GetColor(ray, world);

                    var ir = (int)(255.99f * color.Red);
                    var ig = (int)(255.99f * color.Green);
                    var ib = (int)(255.99f * color.Blue);

                    builder.AppendFormat($"{ir} {ig} {ib}\n");
                }
            }

            File.WriteAllText("output.ppm", builder.ToString());
        }

        private static Color3 GetColor(Ray ray, HitableList world)
        {
            var result = world.Hit(ray, 0.0f, float.MaxValue);
            if (result.HasValue)
            {
                var record = result.Value;

                return 0.5f * Color3.Add(record.Normal, Color3.White);
            }
            else
            {
                var directionUnit = Vector3.Normalize(ray.Direction);
                var t = 0.5f * (directionUnit.Y + 1.0f);
                return (1.0f - t) * Color3.White + t * new Color3(0.5f, 0.7f, 1.0f);
            }
        }
    }
}
