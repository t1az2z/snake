using Binding.Base;
using Core.Bindings.Components;
using UnityEditor;

[CustomEditor(typeof(BaseBinding))]
public class BaseBindingEditor : Editor {
    protected SerializedProperty Path;

    protected virtual void OnEnable() {
        Path = serializedObject.FindProperty("Path");
    }

    public override void OnInspectorGUI() {
        EditorGUILayout.PropertyField(Path);
    }
}