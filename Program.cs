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

            for (var j = ny - 1; j >= 0; j--)
            {
                for (var i = 0; i < nx; i++)
                {
                    var r = 1.0 * i / nx;
                    var g = 1.0 * j / ny;
                    var b = 0.2;
                    var ir = (int)(255.99 * r);
                    var ig = (int)(255.99 * g);
                    var ib = (int)(255.99 * b);

                    builder.AppendFormat($"{ir} {ig} {ib}\n");
                }
            }

            File.WriteAllText("output.ppm", builder.ToString());
        }
    }
}
