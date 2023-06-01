using System;
using UnityEngine;

namespace t1az2z.Tools.Tools
{
    public static class VectorTools
    {
        ///<summary>Returns coordinate on parabolic function between start and end points depending on  time.</summary>
        ///<param name="start">Start point.</param>
        ///<param name="end">End point.</param>
        ///<param name="height">Height of the parabola.</param>
        ///<param name="t">Normalized travel time (between 0 and 1).</param>
        public static Vector3 Parabola(Vector3 start, Vector3 end, float height, float t)
        {
            Func<float, float> f = x => -4 * height * x * x + 4 * height * x;

            var mid = Vector3.Lerp(start, end, t);

            return new Vector3(mid.x, f(t) + Mathf.Lerp(start.y, end.y, t), mid.z);
        }
        
        public static Vector2 GetWorldPositionOfCanvasElement(RectTransform element)
        {
            RectTransformUtility.ScreenPointToWorldPointInRectangle(element, element.position, Camera.main, out var result);
            return result;
        }
        public static Vector3 GetPointOnCircle(Vector3 center, float angle, float radius)
        {
            var targetPos = Vector3.zero;
            targetPos.x = center.x + (radius * Mathf.Cos(angle / (180f / Mathf.PI)));
            targetPos.y = center.y;
            targetPos.z = center.z + (radius * Mathf.Sin(angle / (180f / Mathf.PI)));
            return targetPos;
        }

        public static float GetAngle(Vector3 position, Vector3 target)
        {
            var difference = (target - position).normalized;
            return Mathf.Atan2(difference.x, difference.z) * Mathf.Rad2Deg;
        }
        
        public static Vector3 GetForceVector(Vector3 attackerPos, Vector3 targetPos)
        {
            var res = (targetPos - attackerPos).normalized;
            return res;
        }

        public static Vector3 AngleToDir(float angleRad) => new (Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }
}