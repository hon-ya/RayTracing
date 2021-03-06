﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Mime;
using System.Security.Cryptography;
using SharpDX;

namespace RayTracing
{
    public static class Util
    {
        public static void Swap<T>(ref T right, ref T left)
        {
            var temp = right;
            right = left;
            left = temp;
        }

        public static (byte[] pixels, int width, int height) LoadImage(string filename)
        {
            switch (Path.GetExtension(filename))
            {
                case ".tif":
                    {
                        var bitmap = new Bitmap(filename);
                        var pixels = new byte[3 * bitmap.Width * bitmap.Height];
                        for (var y = 0; y < bitmap.Height; y++)
                        {
                            for (var x = 0; x < bitmap.Width; x++)
                            {
                                var c = bitmap.GetPixel(x, y);
                                pixels[3 * x + 3 * y * bitmap.Width + 0] = c.R;
                                pixels[3 * x + 3 * y * bitmap.Width + 1] = c.G;
                                pixels[3 * x + 3 * y * bitmap.Width + 2] = c.B;
                            }
                        }

                        return (pixels, bitmap.Width, bitmap.Height);
                    }
                default:
                    throw new NotSupportedException();
            }
        }

        public static IEnumerable<Tuple<int, int>> GenerateIndex(int iEnd, int jEnd)
        {
            for (var i = 0; i < iEnd; i++)
            for (var j = 0; j < jEnd; j++)
            {
                yield return new Tuple<int, int>(i, j);
            }
        }

        public static IEnumerable<Tuple<int, int, int>> GenerateIndex(int iEnd, int jEnd, int kEnd)
        {
            for (var i = 0; i < iEnd; i++)
            for (var j = 0; j < jEnd; j++)
            for (var k = 0; k < kEnd; k++)
            {
                yield return new Tuple<int, int, int>(i, j, k);
            }
        }
    }
}