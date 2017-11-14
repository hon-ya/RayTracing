using System;
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
    }
}