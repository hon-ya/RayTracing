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

            var nx = 192;
            var ny = 108;
            var ns = 100;

            builder.AppendFormat($"P3\n");
            builder.AppendFormat($"{nx} {ny}\n");
            builder.AppendFormat($"255\n");

            //var hitables = GetSimpleScene();
            //var hitables = GetTwoSphereScene();
            //var hitables = GetTwoPerlinSphereScene();
            //var hitables = GetTwoImageSphereScene();
            //var hitables = GetSimpleLightScene();
            //var hitables = GetCornellBoxScene();
            var hitables = GetCornellSmokeScene();
            //var hitables = GetRandomScene();
            //var world = new HitableList(hitables);
            var world = new BvhNode(hitables, 0.0f, 1.0f);

            var lookFrom = new Vector3(278.0f, 278.0f, -800.0f);
            var lookAt = new Vector3(278.0f, 278.0f, 0.0f);
            var focusDistance = 10.0f;
            var aperture = 0.0f;
            float vfov = 40.0f;
            var up = new Vector3(0.0f, 1.0f, 0.0f);
            var camera = new Camera(lookFrom, lookAt, up, vfov, 1.0f * nx / ny, aperture, focusDistance, 0.0f, 1.0f);

            for (var j = ny - 1; j >= 0; j--)
            {
                for (var i = 0; i < nx; i++)
                {
                    var color = Color3.Black;

                    for (var s = 0; s < ns; s++)
                    {
                        var u = (1.0f * i + Base.Random.NextInUnitFloat()) / nx;
                        var v = (1.0f * j + Base.Random.NextInUnitFloat()) / ny;

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

        private static Color3 GetColor(Ray ray, IHitable world, int depth)
        {
            var result = world.Hit(ray, 0.001f, float.MaxValue);
            if (result.HasValue)
            {
                var hitRecord = result.Value;
                var scatterRecordResult = hitRecord.Material.Scatter(ray, hitRecord);
                var emitted = hitRecord.Material.Emitted(hitRecord.TexCoord, hitRecord.Position);

                if (depth < 50 && scatterRecordResult.HasValue)
                {
                    var scatterRecord = scatterRecordResult.Value;
                    return Color3.Add(emitted,
                        Vector3.Multiply(scatterRecord.Attenuation,
                            GetColor(scatterRecord.Scattered, world, depth + 1)));
                }
                else
                {
                    return emitted;
                }
            }

            return Color3.Black;
        }

        private static List<IHitable> GetSimpleScene()
        {
            return new List<IHitable>()
            {
                new Sphere(new Vector3(0.0f, 0.0f, -1.0f), 0.5f, new Lambertian(new ConstantTexture(new Vector3(0.1f, 0.2f, 0.5f)))),
                new Sphere(new Vector3(0.0f, -100.5f, -1.0f), 100.0f, new Lambertian(new ConstantTexture(new Vector3(0.8f, 0.8f, 0.0f)))),
                new Sphere(new Vector3(1.0f, 0.0f, -1.0f), 0.5f, new Metal(new Vector3(0.8f, 0.6f, 0.2f), 0.0f)),
                new Sphere(new Vector3(-1.0f, 0.0f, -1.0f), 0.5f, new Dielectric(1.5f)),
                new Sphere(new Vector3(-1.0f, 0.0f, -1.0f), -0.45f, new Dielectric(1.5f)),
            };
        }

        private static List<IHitable> GetTwoSphereScene()
        {
            var checkerTexture = new CheckerTexture(
                new ConstantTexture(new Color3(0.2f, 0.3f, 0.1f)),
                new ConstantTexture(new Color3(0.9f, 0.9f, 0.9f))
            );

            return new List<IHitable>
            {
                new Sphere(new Vector3(0.0f, -10.0f, 0.0f), 10.0f, new Lambertian(checkerTexture)),
                new Sphere(new Vector3(0.0f,  10.0f, 0.0f), 10.0f, new Lambertian(checkerTexture))
            };
        }

        private static List<IHitable> GetTwoPerlinSphereScene()
        {
            var perlinTexture = new NoiseTexture(5.0f);

            return new List<IHitable>
            {
                new Sphere(new Vector3(0.0f, -1000.0f, 0.0f), 1000.0f, new Lambertian(perlinTexture)),
                new Sphere(new Vector3(0.0f,  2.0f, 0.0f), 2.0f, new Lambertian(perlinTexture))
            };
        }

        private static List<IHitable> GetTwoImageSphereScene()
        {
            var filepath = @"earthmap.tif";
            var image = Util.LoadImage(filepath);
            var texture = new ImageTexture(image.pixels, image.width, image.height);

            return new List<IHitable>
            {
                new Sphere(new Vector3(0.0f, -1000.0f, 0.0f), 1000.0f, new Lambertian(texture)),
                new Sphere(new Vector3(0.0f,  2.0f, 0.0f), 2.0f, new Lambertian(texture))
            };
        }

        private static List<IHitable> GetSimpleLightScene()
        {
            var perlinTexture = new NoiseTexture(4.0f);

            return new List<IHitable>
            {
                new Sphere(new Vector3(0.0f, -1000.0f, 0.0f), 1000.0f, new Lambertian(perlinTexture)),
                new Sphere(new Vector3(0.0f,  2.0f, 0.0f), 2.0f, new Lambertian(perlinTexture)),
                new Sphere(new Vector3(0.0f,  7.0f, 0.0f), 2.0f, new DiffuseLight(new ConstantTexture(new Color3(4.0f, 4.0f, 4.0f)))),
                new XyRectangle(3.0f, 5.0f, 1.0f, 3.0f, -2.0f, new DiffuseLight(new ConstantTexture(new Color3(4.0f, 4.0f, 4.0f)))),
            };
        }

        private static List<IHitable> GetCornellBoxScene()
        {
            var red = new Lambertian(new ConstantTexture(new Vector3(0.65f, 0.05f, 0.05f)));
            var white = new Lambertian(new ConstantTexture(new Vector3(0.73f, 0.73f, 0.73f)));
            var green = new Lambertian(new ConstantTexture(new Vector3(0.12f, 0.45f, 0.15f)));
            var light = new DiffuseLight(new ConstantTexture(new Vector3(15.0f, 15.0f, 15.0f)));

            return new List<IHitable>
            {
                new FlipNormals(new YzRectangle(0, 555, 0, 555, 555, green)),
                new YzRectangle(0, 555, 0, 555, 0, red),
                new XzRectangle(213, 343, 227, 332, 554, light),
                new FlipNormals(new XzRectangle(0, 555, 0, 555, 555, white)),
                new XzRectangle(0, 555, 0, 555, 0, white),
                new FlipNormals(new XyRectangle(0, 555, 0, 555, 555, white)),
                new Translate(new RotateY(new Box(Vector3.Zero, new Vector3(165, 165, 165), new Lambertian(new ConstantTexture(Color.White.ToColor3()))), MathUtil.DegreesToRadians(-18.0f)), new Vector3(130.0f, 0.0f, 65.0f)),
                new Translate(new RotateY(new Box(Vector3.Zero, new Vector3(165, 330, 165), new Lambertian(new ConstantTexture(Color.White.ToColor3()))), MathUtil.DegreesToRadians(15.0f)), new Vector3(265.0f, 0.0f, 295.0f)),
            };
        }

        private static List<IHitable> GetCornellSmokeScene()
        {
            var red = new Lambertian(new ConstantTexture(new Vector3(0.65f, 0.05f, 0.05f)));
            var white = new Lambertian(new ConstantTexture(new Vector3(0.73f, 0.73f, 0.73f)));
            var green = new Lambertian(new ConstantTexture(new Vector3(0.12f, 0.45f, 0.15f)));
            var light = new DiffuseLight(new ConstantTexture(new Vector3(1.0f, 1.0f, 1.0f)));

            return new List<IHitable>
            {
                new FlipNormals(new YzRectangle(0, 555, 0, 555, 555, green)),
                new YzRectangle(0, 555, 0, 555, 0, red),
                new XzRectangle(113, 443, 127, 432, 554, light),
                new FlipNormals(new XzRectangle(0, 555, 0, 555, 555, white)),
                new XzRectangle(0, 555, 0, 555, 0, white),
                new FlipNormals(new XyRectangle(0, 555, 0, 555, 555, white)),
                new ConstantMedium(
                    new Translate(new RotateY(new Box(Vector3.Zero, new Vector3(165, 165, 165), white), MathUtil.DegreesToRadians(-18.0f)), new Vector3(130.0f, 0.0f, 65.0f)),
                    0.01f,
                    new ConstantTexture(Color3.White)
                ),
                new ConstantMedium(
                    new Translate(new RotateY(new Box(Vector3.Zero, new Vector3(165, 330, 165), white), MathUtil.DegreesToRadians(15.0f)), new Vector3(265.0f, 0.0f, 295.0f)),
                    0.01f,
                    new ConstantTexture(Color3.Black)
                ),
            };
        }

        private static List<IHitable> GetRandomScene()
        {
            var checkerTexture = new CheckerTexture(
                new ConstantTexture(new Color3(0.2f, 0.3f, 0.1f)),
                new ConstantTexture(new Color3(0.9f, 0.9f, 0.9f))
                );

            var hitables = new List<IHitable>()
            {
                new Sphere(new Vector3(0.0f, -1000.0f, 0.0f), 1000.0f, new Lambertian(checkerTexture))
            };

            var range = 1;

            for (var a = -range; a < range; a++)
            {
                for (int b = -range; b < range; b++)
                {
                    var chooseMat = Base.Random.NextInUnitFloat();
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
                                    new ConstantTexture(new Vector3(
                                        Base.Random.NextInUnitFloat() * Base.Random.NextInUnitFloat(),
                                        Base.Random.NextInUnitFloat() * Base.Random.NextInUnitFloat(),
                                        Base.Random.NextInUnitFloat() * Base.Random.NextInUnitFloat()
                                    ))));
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
                new Sphere(new Vector3(-4.0f, 1.0f, 0.0f), 1.0f, new Lambertian(new ConstantTexture(new Vector3(0.4f, 0.2f, 0.1f)))),
                new Sphere(new Vector3(4.0f, 1.0f, 0.0f), 1.0f, new Metal(new Vector3(0.7f, 0.6f, 0.5f), 0.0f)),
            });
            return hitables;
        }
    }
}
