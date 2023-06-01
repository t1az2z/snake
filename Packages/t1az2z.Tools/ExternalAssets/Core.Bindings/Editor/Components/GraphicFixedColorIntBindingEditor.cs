using Core.Bindings.Components;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GraphicColorIntBinding))]
public class GraphicFixedColorIntBindingEditor : BaseBindingEditor {
    private GUIStyle buttonMiddleStyle {
        get {
            return _buttonMiddleStyle ?? (_buttonMiddleStyle =
                                               new GUIStyle("Button") {fixedWidth = 20f, fixedHeight = 14f, alignment = TextAnchor.MiddleCenter});
        }
    }

    private GUIStyle buttonLowerStyle {
        get {
            return _buttonLowerStyle ??
                   (_buttonLowerStyle = new GUIStyle("Button") {fixedWidth = 20f, fixedHeight = 14f, alignment = TextAnchor.LowerCenter});
        }
    }

    private GUIStyle _buttonMiddleStyle;
    private GUIStyle _buttonLowerStyle;

    private SerializedProperty _elements;
    private SerializedProperty _useBoundaryValues;

    private static bool _expanded = true;

    protected override void OnEnable() {
        base.OnEnable();

        _elements = serializedObject.FindProperty("_elements");
        _useBoundaryValues = serializedObject.FindProperty("_useBoundaryValues");
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        _useBoundaryValues.boolValue = GUILayout.Toggle(_useBoundaryValues.boolValue, "Use Boundary Values");

        GUILayout.BeginHorizontal();
        _expanded = EditorGUILayout.Foldout(_expanded, string.Format("Elements ({0})", _elements.arraySize));
        if (_expanded) {
            if (GUILayout.Button("+", buttonMiddleStyle)) {
                _elements.InsertArrayElementAtIndex(Mathf.Max(_elements.arraySize - 1, 0));
            }

            if (GUILayout.Button("-", buttonMiddleStyle)) {
                if (_elements.arraySize > 0) {
                    _elements.DeleteArrayElementAtIndex(_elements.arraySize - 1);
                }
            }
        }

        GUILayout.EndHorizontal();

        if (_expanded) {
            EditorGUI.indentLevel++;
            var index = 0;
            while (index < _elements.arraySize) {
                var item = _elements.GetArrayElementAtIndex(index);

                GUILayout.BeginHorizontal();
                var value = item.FindPropertyRelative("_value");
                var color = item.FindPropertyRelative("_color");
                EditorGUILayout.LabelField("Value", GUILayout.Width(50f));
                value.intValue = EditorGUILayout.IntField(value.intValue, GUILayout.Width(60f));
                EditorGUILayout.LabelField("Color", GUILayout.Width(50f));
                color.colorValue = EditorGUILayout.ColorField(color.colorValue);
                if (GUILayout.Button("↑", buttonLowerStyle)) {
                    if (index > 0) {
                        _elements.MoveArrayElement(index, index - 1);
                    }
                }

                if (GUILayout.Button("↓", buttonLowerStyle)) {
                    if (index < _elements.arraySize - 1) {
                        _elements.MoveArrayElement(index, index + 1);
                    }
                }

                if (GUILayout.Button("x", buttonMiddleStyle)) {
                    _elements.DeleteArrayElementAtIndex(index);
                    continue;
                }

                GUILayout.EndHorizontal();

                index++;
            }

            EditorGUI.indentLevel--;

            serializedObject.ApplyModifiedProperties();
        }
    }
}