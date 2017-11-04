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
            var t = HitSphere(new Vector3(0.0f, 0.0f, -1.0f), 0.5f, ray);
            if (t > 0.0f)
            {
                var n = Vector3.Normalize(ray.GetPoint(t) - new Vector3(0.0f, 0.0f, -1.0f));
                return 0.5f * new Vector3(n.X + 1, n.Y + 1, n.Z + 1);
            }

            var directionUnit = Vector3.Normalize(ray.Direction);
            t = 0.5f * (directionUnit.Y + 1.0f);
            return (1.0f - t) * new Color3(1.0f) + t * new Color3(0.5f, 0.7f, 1.0f);
        }

        static float HitSphere(Vector3 center, float radius, Ray ray)
        {
            var oc = ray.Position - center;
            var a = Vector3.Dot(ray.Direction, ray.Direction);
            var b = 2.0f * Vector3.Dot(oc, ray.Direction);
            var c = Vector3.Dot(oc, oc) - radius * radius;
            var discriminant = b * b - 4 * a * c;

            if(discriminant < 0)
            {
                return -1.0f;
            }
            else
            {
                return (-b - (float)Math.Sqrt(discriminant)) / (2.0f * a);
            }
        }

        
    }
}
