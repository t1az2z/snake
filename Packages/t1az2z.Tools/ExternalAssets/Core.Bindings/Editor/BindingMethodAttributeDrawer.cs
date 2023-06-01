using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using System.Reflection;
using Core.Bindings.Tools;
using Core.Bindings.Tools.Extensions;
#if !BASE_GLOBAL_EXTENSIONS
#endif

namespace Binding.Base {
    [CustomPropertyDrawer(typeof(BindingMethodAttribute), true)]
    public class BindingMethodAttributeDrawer : PropertyDrawer {
        public class PropertyMenuItem {
            public string Value;
            public SerializedProperty Target;
        }

        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label) {
            EditorGUI.LabelField(rect, label);
            rect.xMin += 132f;
            DrawPropertyPathDropdownButton(new Rect(rect.position, rect.size.WithX(16f)), property);

            rect.xMin += 16f;
            property.stringValue = EditorGUI.TextField(rect, property.stringValue);
        }

        private void DrawPropertyPathDropdownButton(Rect rect, SerializedProperty property) {
            var dropdownButtonStyle = new GUIStyle(EditorStyles.miniButton) { border = new RectOffset(), padding = new RectOffset() };
            if (GUI.Button(rect, EditorGUIUtility.IconContent("Icon Dropdown"), dropdownButtonStyle)) {
                GenericMenu menu = new GenericMenu();
                var list = GetDropdownProperties(property);
                for (int i = 0; i < list.Length; i++) {
                    var item = new PropertyMenuItem { Value = list[i], Target = property };
                    menu.AddItem(new GUIContent(list[i]), false, OnMethodSelected, item);
                }
                menu.ShowAsContext();
            }
        }

        private void OnMethodSelected(object data) {
            var item = data as PropertyMenuItem;
            item.Target.serializedObject.Update();
            item.Target.stringValue = item.Value;
            item.Target.serializedObject.ApplyModifiedProperties();
        }

        private string[] GetDropdownProperties(SerializedProperty property) {
            var atr = attribute as BindingMethodAttribute;
            var transform = (property.serializedObject.targetObject as MonoBehaviour).transform;

            var result = new HashSet<string>();
            var parent = transform;
            while (parent != null) {
                var target = parent.GetComponent<IBindingTarget>();
                if (target != null) {
                    var methods = target.GetType().GetCachedMethods(BindingFlags.Instance | BindingFlags.Public).Filter(m => !m.IsSpecialName && !m.IsConstructor && !m.IsGenericMethod);
                    switch (atr.Type) {
                        case BindingMethodType.Action:
                            methods = methods.Filter(m => m.GetParameters().Length == 0);
                            break;
                    }

                    foreach (var itr in methods) {
                        result.Add(itr.Name);
                    }
                }

                parent = parent.parent;
            }

            return result.ToArray();
        }
    }
}