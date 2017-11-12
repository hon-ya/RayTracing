using System;
using SharpDX;

namespace RayTracing
{
    public class Camera
    {
        public float LensRadius { get; private set; }
        public Vector3 LowerLeftCorner { get; private set; }
        public Vector3 Horizontal { get; private set; }
        public Vector3 Vertical { get; private set; }
        public Vector3 Origin { get; private set; }
        public Vector3 U { get; private set; }
        public Vector3 V { get; private set; }
        public Vector3 W { get; private set; }

        public Camera(Vector3 lookFrom, Vector3 lookAt, Vector3 up, float vfov, float aspect, float aperture, float focusDistance)
        {
            var theta = MathUtil.DegreesToRadians(vfov);
            var halfHeight = (float)Math.Tan(theta / 2.0f);
            var halfWidth = aspect * halfHeight;

            Origin = lookFrom;

            W = Vector3.Normalize(lookFrom - lookAt);
            U = Vector3.Normalize(Vector3.Cross(Vector3.Up, W));
            V = Vector3.Cross(W, U);

            LensRadius = aperture / 2.0f;
            // 焦点距離にある平面を表現
            LowerLeftCorner = Origin - halfWidth * focusDistance * U - halfHeight * focusDistance * V - focusDistance * W;
            Horizontal = 2.0f * halfWidth * focusDistance * U;
            Vertical = 2.0f * halfHeight * focusDistance * V;
        }

        public Ray GetRay(float s, float t)
        {
            var rd = LensRadius * Base.Random.NextInUnitDisk();
            var offset = U * rd.X + V * rd.Y;

            // 焦点距離平面上の点は変わらず、Ray の開始地点を僅かにずらす
            // 焦点距離平面上にある物体は明瞭になり、そうでない物体はオフセットによってランダムに傾いた Ray によりボケる。
            return new Ray(Origin + offset, LowerLeftCorner + s * Horizontal + t * Vertical - (Origin + offset));
        }
    }
}