using System;
using System.Drawing;
using System.IO;
using System.Net.Mime;
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
    }
}