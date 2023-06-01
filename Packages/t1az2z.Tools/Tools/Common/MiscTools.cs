using UnityEditor;
using UnityEngine;

namespace t1az2z.Tools.Tools.Common
{
    public static class MiscTools
    {
#if UNITY_EDITOR
        // ReSharper disable once InconsistentNaming
        public static T GetSOConfig<T>() where T : ScriptableObject
        {
            string[]
                guids = AssetDatabase.FindAssets("t:" +
                                                 typeof(T)
                                                     .Name); //FindAssets uses tags check documentation for more info
            T[] a = new T[guids.Length];

            for (int i = 0; i < guids.Length; i++) //probably could get optimized 
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                a[i] = AssetDatabase.LoadAssetAtPath<T>(path);
            }

            return a[0];
        }
#endif
        
    }
}