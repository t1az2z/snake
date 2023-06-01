using System;
using System.Collections.Generic;
using Leopotam.Ecs;
using UnityEngine;
using Random = UnityEngine.Random;

namespace t1az2z.Tools.Tools
{
    public static class Extensions
    {
        public static Vector3 Where(this Vector3 original, float? x = null, float? y = null, float? z = null)
        {
            return new Vector3(x ?? original.x, y ?? original.y, z ?? original.z);
        }

        public static Vector2 Where(this Vector2 original, float? x = null, float? y = null)
        {
            return new Vector2(x ?? original.x, y ?? original.y);
        }

        ///<summary>Remaps value from one range to another.</summary>
        ///<param name="value">Value.</param>
        ///<param name="from1">Initial range start value.</param>
        ///<param name="from2">Initial range end value.</param>
        ///<param name="to1">Result range start value.</param>
        ///<param name="to2">Result range end value.</param>
        
        public static float Remap(this float value, float from1, float to1, float from2, float to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }

        ///<summary>Remaps value from initial range to 0-1 range.</summary>
        ///<param name="value">Value.</param>
        ///<param name="from">Initial range start value.</param>
        ///<param name="to">Initial range end value.</param>
        public static float Remap01(this float value, float from, float to)
        {
            return Remap(value, from, to, 0, 1);
        }
        

        ///<summary>Finds the index of the first item matching an expression in an enumerable.</summary>
        ///<param name="items">The enumerable to search.</param>
        ///<param name="predicate">The expression to test the items against.</param>
        ///<returns>The index of the first matching item, or -1 if no items match.</returns>
        public static int FindIndex<T>(this IEnumerable<T> items, Func<T, bool> predicate)
        {
            if (items == null) throw new ArgumentNullException("items");
            if (predicate == null) throw new ArgumentNullException("predicate");

            int retVal = 0;
            foreach (var item in items)
            {
                if (predicate(item)) return retVal;
                retVal++;
            }
            return -1;
        }
        
        ///<summary>Finds the index of the first occurrence of an item in an enumerable.</summary>
        ///<param name="items">The enumerable to search.</param>
        ///<param name="item">The item to find.</param>
        ///<returns>The index of the first matching item, or -1 if the item was not found.</returns>
        public static int IndexOf<T>(this IEnumerable<T> items, T item) { return items.FindIndex(i => EqualityComparer<T>.Default.Equals(item, i)); }
        
        
        ///<summary>Returns random alive not null entity from filter.</summary>
        public static EcsEntity GetRandomEntity(this EcsFilter filter)
        {
            var targetIndex = Random.Range(0, filter.GetEntitiesCount());
            EcsEntity res = EcsEntity.Null;

            foreach (var index in filter)
            {
                if (index != targetIndex)
                    continue;

                if (filter.GetEntity(index).IsNull() || !filter.GetEntity(index).IsAlive())
                {
                    targetIndex += 1;
                    continue;
                }

                res = filter.GetEntity(index);
            }

            return res;
        }
        
        ///<summary>Returns coordinate on parabolic function between start and end points depending on time.</summary>
        ///<param name="start">Start point.</param>
        ///<param name="end">End point.</param>
        ///<param name="height">Height of the parabola.</param>
        ///<param name="t">Normalized travel time (between 0 and 1).</param>
        ///<param name="normalDirection">Parabolic growth direction.</param>
        public static Vector3 Parabola(Vector3 start, Vector3 end, float height, float t, Vector3 normalDirection)
        {
            Func<float, float> f = x => -4 * height * x * x + 4 * height * x;

            var mid = Vector3.Lerp(start, end, t);
            var parabolicAddition = new Vector3(
                (f(t) + Mathf.Lerp(start.x, end.x, t)) * normalDirection.x,
                (f(t) + Mathf.Lerp(start.y, end.y, t)) * normalDirection.y,
                (f(t) + Mathf.Lerp(start.z, end.z, t)) * normalDirection.z);
            return mid+parabolicAddition;
        }

        public static bool TryGet<T>(this EcsEntity entity, out T component) where T : struct
        {
            if (entity.Has<T>())
            {
                component = entity.Get<T>();
                return true;
            }

            component = default;
            return false;
        } 
        
        public static Vector3 AngleToDir(float angleRad) => new (Mathf.Cos(angleRad), Mathf.Sin(angleRad));
        
        public static Bounds TransformBounds(this Transform transform, Bounds localBounds )
        {
            var center = transform.TransformPoint(localBounds.center);
 
            // transform the local extents' axes
            var extents = localBounds.extents;
            var axisX = transform.TransformVector(extents.x, 0, 0);
            var axisY = transform.TransformVector(0, extents.y, 0);
            var axisZ = transform.TransformVector(0, 0, extents.z);
 
            // sum their absolute value to get the world extents
            extents.x = Mathf.Abs(axisX.x) + Mathf.Abs(axisY.x) + Mathf.Abs(axisZ.x);
            extents.y = Mathf.Abs(axisX.y) + Mathf.Abs(axisY.y) + Mathf.Abs(axisZ.y);
            extents.z = Mathf.Abs(axisX.z) + Mathf.Abs(axisY.z) + Mathf.Abs(axisZ.z);
 
            return new Bounds { center = center, extents = extents };
        }
    }
}
