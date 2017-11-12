using SharpDX;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RayTracing
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new StringBuilder();

            var nx = 192;
            var ny = 108;
            var ns = 100;

            builder.AppendFormat($"P3\n");
            builder.AppendFormat($"{nx} {ny}\n");
            builder.AppendFormat($"255\n");

#if false
            var hitables = new IHitable[]
            {
                new Sphere(new Vector3(0.0f, 0.0f, -1.0f), 0.5f, new Lambertian(new Vector3(0.1f, 0.2f, 0.5f))),
                new Sphere(new Vector3(0.0f, -100.5f, -1.0f), 100.0f, new Lambertian(new Vector3(0.8f, 0.8f, 0.0f))),
                new Sphere(new Vector3(1.0f, 0.0f, -1.0f), 0.5f, new Metal(new Vector3(0.8f, 0.6f, 0.2f), 0.0f)),
                new Sphere(new Vector3(-1.0f, 0.0f, -1.0f), 0.5f, new Dielectric(1.5f)),
                new Sphere(new Vector3(-1.0f, 0.0f, -1.0f), -0.45f, new Dielectric(1.5f)),
            };
#endif
            var hitables = GetRandomScene();
            var world = new HitableList(hitables);

            var lookFrom = new Vector3(13.0f, 2.0f, 3.0f);
            var lookAt = new Vector3(0.0f, 0.0f, 0.0f);
            var focusDistance = 10.0f;
            var aperture = 0.0f;
            var up = new Vector3(0.0f, 1.0f, 0.0f);
            var camera = new Camera(lookFrom, lookAt, up, 20.0f, 1.0f * nx / ny, aperture, focusDistance, 0.0f, 1.0f);

            for (var j = ny - 1; j >= 0; j--)
            {
                for (var i = 0; i < nx; i++)
                {
                    var color = Color3.Black;

                    for (var s = 0; s < ns; s++)
                    {
                        var u = (1.0f * i + Base.Random.NextFloat(0.0f, 1.0f)) / nx;
                        var v = (1.0f * j + Base.Random.NextFloat(0.0f, 1.0f)) / ny;

                        var ray = camera.GetRay(u, v);
                        color += GetColor(ray, world, 0);
                    }

                    color = Vector3.Divide(color, ns);
                    color = new Vector3((float)Math.Sqrt(color.Red), (float)Math.Sqrt(color.Green),
                        (float)Math.Sqrt(color.Blue));

                    var ir = (int)(255.99f * color.Red);
                    var ig = (int)(255.99f * color.Green);
                    var ib = (int)(255.99f * color.Blue);

                    builder.AppendFormat($"{ir} {ig} {ib}\n");
                }
            }

            File.WriteAllText("output.ppm", builder.ToString());
        }

        private static Color3 GetColor(Ray ray, HitableList world, int depth)
        {
            var result = world.Hit(ray, 0.001f, float.MaxValue);
            if (result.HasValue)
            {
                var hitRecord = result.Value;
                if (depth < 50)
                {
                    var scatterRecordResult = hitRecord.Material.Scatter(ray, hitRecord);
                    if (scatterRecordResult.HasValue)
                    {
                        var scatterRecord = scatterRecordResult.Value;

                        return Vector3.Multiply(scatterRecord.Attenuation, GetColor(scatterRecord.Scattered, world, depth + 1));
                    }
                }

                return Color3.Black;
            }
            else
            {
                var directionUnit = Vector3.Normalize(ray.Direction);
                var t = 0.5f * (directionUnit.Y + 1.0f);
                return (1.0f - t) * Color3.White + t * new Color3(0.5f, 0.7f, 1.0f);
            }
        }

        static private List<IHitable> GetRandomScene()
        {
            var hitables = new List<IHitable>()
            {
                new Sphere(new Vector3(0.0f, -1000.0f, 0.0f), 1000.0f, new Lambertian(new Vector3(0.5f, 0.5f, 0.5f)))
            };

            for (var a = -10; a < 10; a++)
            {
                for (int b = -10; b < 10; b++)
                {
                    var chooseMat = Base.Random.NextFloat(0.0f, 1.0f);
                    var center = new Vector3(
                        a + 0.9f * Base.Random.NextInUnitFloat(),
                        0.2f,
                        b + 0.9f * Base.Random.NextInUnitFloat()
                    );

                    if ((center - new Vector3(4.0f, 0.2f, 0.0f)).Length() > 0.9f)
                    {
                        if (chooseMat < 0.8f)
                        {
                            var obj = new MovingSphere(
                                center,
                                center + new Vector3(0.0f, 0.5f * Base.Random.NextInUnitFloat(), 0.0f),
                                0.0f,
                                1.0f,
                                0.2f,
                                new Lambertian(
                                    new Vector3(
                                        Base.Random.NextInUnitFloat() * Base.Random.NextInUnitFloat(),
                                        Base.Random.NextInUnitFloat() * Base.Random.NextInUnitFloat(),
                                        Base.Random.NextInUnitFloat() * Base.Random.NextInUnitFloat()
                                    )));
                            hitables.Add(obj);
                        }
                        else if (chooseMat < 0.95f)
                        {
                            var obj = new Sphere(
                                center,
                                0.2f,
                                new Metal(
                                    new Vector3(
                                        0.5f * (1 + Base.Random.NextInUnitFloat()),
                                        0.5f * (1 + Base.Random.NextInUnitFloat()),
                                        0.5f * (1 + Base.Random.NextInUnitFloat())
                                    ),
                                    0.5f * Base.Random.NextInUnitFloat()
                                ));
                            hitables.Add(obj);
                        }
                        else
                        {
                            var obj = new Sphere(
                                center,
                                0.2f,
                                new Dielectric(1.5f)
                            );
                            hitables.Add(obj);
                        }
                    }
                }
            }

            hitables.AddRange(new Sphere[]
            {
                new Sphere(new Vector3(0.0f, 1.0f, 0.0f), 1.0f, new Dielectric(1.5f)),
                new Sphere(new Vector3(-4.0f, 1.0f, 0.0f), 1.0f, new Lambertian(new Vector3(0.4f, 0.2f, 0.1f))),
                new Sphere(new Vector3(4.0f, 1.0f, 0.0f), 1.0f, new Metal(new Vector3(0.7f, 0.6f, 0.5f), 0.0f)),
            });
            return hitables;
        }
    }
}
