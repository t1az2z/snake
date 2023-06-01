using UnityEngine;

namespace t1az2z.Tools.Tools.Common
{
    public static class MathTools
    {
        public static (Vector3, Vector3) EvaluateCubicBezier(Vector3 pt0, Vector3 pt1, Vector3 pt2, Vector3 pt3, float t)
        {
            var a = Vector3.Lerp(pt0, pt1, t);
            var b = Vector3.Lerp(pt1, pt2, t);
            var c = Vector3.Lerp(pt2, pt3, t);
            var d = Vector3.Lerp(a, b, t);
            var e = Vector3.Lerp(b, c, t);
            var position =Vector3.Lerp(d, e, t);
            var direction = (e - d).normalized;
            //return (position, direction);
            var mint = 1 - t;

            var res = Mathf.Pow(mint, 3) * pt0 + 3 * Mathf.Pow(mint, 2) * pt1 + 3 * mint * t * t * pt2 +
                      t * t * t * pt3;

            var vel = 3 * mint * mint * pt0 + 6 * mint * t * (pt2 - pt1) + 3 * t * t * (pt3 - pt2);
            
            return (res, vel);
        }
        
        public static (Vector3, Vector3) EvaluateCubicBezier(Vector3[] points, float t)
        {
            if (points.Length != 4)
            {
                Debug.LogError("Wring point array length");
                return (Vector3.zero, Vector3.zero);
            }
            
            var a = Vector3.Lerp(points[0], points[1], t);
            var b = Vector3.Lerp(points[1], points[2], t);
            var c = Vector3.Lerp(points[2], points[3], t);
            var d = Vector3.Lerp(a, b, t);
            var e = Vector3.Lerp(b, c, t);
            var position =Vector3.Lerp(d, e, t);
            var direction = (e - d).normalized;
            return (position, direction);
        }
    }
}