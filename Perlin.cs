using System;
using System.Collections.Generic;
using System.Linq;
using SharpDX;

namespace RayTracing
{
    public static class Perlin
    {
        private static readonly Vector3[] RandomVector = PerlinGenerate();
        private static readonly int[] PermX = PerlinGeneratePerm();
        private static readonly int[] PermY = PerlinGeneratePerm();
        private static readonly int[] PermZ = PerlinGeneratePerm();

        public static float Noise(Vector3 position)
        {
            var u = position.X - (float)Math.Floor(position.X);
            var v = position.Y - (float)Math.Floor(position.Y);
            var w = position.Z - (float)Math.Floor(position.Z);
            var i = (int)Math.Floor(position.X);
            var j = (int)Math.Floor(position.Y);
            var k = (int)Math.Floor(position.Z);

            var c = new Vector3[2, 2, 2];

            for (var di = 0; di < 2; di++)
            for (var dj = 0; dj < 2; dj++)
            for (var dk = 0; dk < 2; dk++)
            {
                c[di, dj, dk] = RandomVector[PermX[(i + di) & 255] ^ PermY[(j + dj) & 255] ^ PermZ[(k + dk) & 255]];
            }

            return PerlinInterpolate(c, u, v, w);
        }

        public static float Turbulence(Vector3 position, int depth = 7)
        {
            var accumulate = 0.0f;
            var positionTemp = position;
            var weight = 1.0f;

            for (var i = 0; i < depth; i++)
            {
                accumulate += weight * Noise(positionTemp);
                weight *= 0.5f;
                positionTemp *= 2.0f;
            }

            return Math.Abs(accumulate);
        }

        private static float PerlinInterpolate(Vector3[,,] c, float u, float v, float w)
        {
            var uu = u * u * (3 - 2 * u);
            var vv = v * v * (3 - 2 * v);
            var ww = w * w * (3 - 2 * w);

            var accumulate = 0.0f;

            for (int i = 0; i < 2; i++)
            for (int j = 0; j < 2; j++)
            for (int k = 0; k < 2; k++)
            {
                var weight = new Vector3(u - i, v - j, w - k);

                accumulate += (i * uu + (1 - i) * (1 - uu))
                              * (j * vv + (1 - j) * (1 - vv))
                              * (k * ww + (1 - k) * (1 - ww))
                              * Vector3.Dot(c[i, j, k], weight);
            }

            return accumulate;
        }

        private static Vector3[] PerlinGenerate()
        {
            var p = new Vector3[256];
            for (var i = 0; i < p.Length; i++)
            {
                p[i] = Vector3.Normalize(Base.Random.NextVector3(-Vector3.One, Vector3.One));
            }

            return p;
        }

        private static void Permute(int[] p)
        {
            for (var i = p.Length - 1; i > 0; i--)
            {
                var target = (int) (Base.Random.NextFloat() * (i + 1));

                Util.Swap(ref p[i], ref p[target]);
            }
        }

        private static int[] PerlinGeneratePerm()
        {
            var p = Enumerable.Range(0, 256).ToArray();
            Permute(p);

            return p;
        }

    }
}