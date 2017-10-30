using SharpDX;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            for (var j = ny - 1; j >= 0; j--)
            {
                for (var i = 0; i < nx; i++)
                {
                    var u = 1.0f * i / nx;
                    var v = 1.0f * j / ny;

                    var ray = new Ray(origin, lowerLeftCorner + u * horizontal + v * vertical);
                    var color = GetColor(ray);

                    var ir = (int)(255.99 * color.Red);
                    var ig = (int)(255.99 * color.Green);
                    var ib = (int)(255.99 * color.Blue);

                    builder.AppendFormat($"{ir} {ig} {ib}\n");
                }
            }

            File.WriteAllText("output.ppm", builder.ToString());
        }

        static Color3 GetColor(Ray ray)
        {
            var directionUnit = Vector3.Normalize(ray.Direction);
            var t = 0.5f * (directionUnit.Y + 1.0f);
            return (1.0f - t) * new Color3(1.0f) + t * new Color3(0.5f, 0.7f, 1.0f);
        }
    }
}
