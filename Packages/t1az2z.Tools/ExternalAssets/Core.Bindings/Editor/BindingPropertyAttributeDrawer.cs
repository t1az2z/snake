using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Core.Bindings.Components;
using Core.Bindings.Tools.Extensions;
using VectorExtensions = Core.Bindings.Tools.Extensions.VectorExtensions;
#if !BASE_GLOBAL_EXTENSIONS
#endif

namespace Binding.Base {
    [CustomPropertyDrawer(typeof(BindingPropertyAttribute), true)]
    public class BindingPropertyAttributeDrawer : PropertyDrawer {
        public class PropertyMenuItem {
            public string Value;
            public SerializedProperty Target;
        }

        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label) {
            var backup = GUI.color;
            var oldValue = property.stringValue;

            EditorGUI.LabelField(rect, label);
            rect.xMin += 132f;
            var list = DrawPropertyPathDropdownButton(new Rect(rect.position, VectorExtensions.WithX(rect.size, 16f)), property);
            GUI.color = Enumerable.Contains(list, oldValue) ? Color.green : Color.red;

            rect.xMin += 16f;
            var newValue = EditorGUI.TextField(rect, oldValue);
            GUI.color = backup;
            GUI.changed = oldValue != newValue;
            if (GUI.changed) {
                property.stringValue = newValue;
            }
        }

        private string[] DrawPropertyPathDropdownButton(Rect rect, SerializedProperty property) {
            var list = GetDropdownProperties(property);
            var dropdownButtonStyle = new GUIStyle(EditorStyles.miniButton) { border = new RectOffset(), padding = new RectOffset() };
            if (GUI.Button(rect, EditorGUIUtility.IconContent("Icon Dropdown"), dropdownButtonStyle)) {
                var oldValue = property.stringValue;
                GenericMenu menu = new GenericMenu();
                for (int i = 0; i < list.Length; i++) {
                    if (list[i] == "_") {
                        menu.AddSeparator("");
                    }
                    else if (list[i][0] == '@') {
                        menu.AddDisabledItem(new GUIContent(list[i]));
                    }
                    else {
                        var item = new PropertyMenuItem { Value = list[i], Target = property };
                        menu.AddItem(new GUIContent(list[i]), oldValue == list[i], OnPropertySelected, item);
                    }
                }
                menu.ShowAsContext();
            }
            return list;
        }

        private void OnPropertySelected(object data) {
            var item = data as PropertyMenuItem;
            item.Target.serializedObject.Update();
            item.Target.stringValue = item.Value;
            item.Target.serializedObject.ApplyModifiedProperties();
        }

        private void ExtractBindingTypes(SerializedProperty property, out System.Type bindingPropertyType, out System.Type bindingVarType) {
            var source = attribute as BindingPropertyAttribute;
            bindingPropertyType = source.BindingPropertyType;
            bindingVarType = source.BindingValueType;
            if ((bindingPropertyType != null) || (bindingVarType != null)) {
                return;
            }

            var targetType = property.serializedObject.targetObject.GetType();
            if (TypeExtensions.IsGenericType(targetType, typeof(BaseBinding<>), out var genericTargetType)) {
                bindingPropertyType = genericTargetType.GenericTypeArguments[0];
                if (TypeExtensions.IsGenericType(bindingPropertyType, typeof(Property<>), out var genericPropertyType)) {
                    bindingVarType = genericPropertyType.GenericTypeArguments[0];
                }
            }
        }

        private string[] GetDropdownProperties(SerializedProperty property) {
            var mono = (property.serializedObject.targetObject as MonoBehaviour);
            var transform = mono.transform;

            ExtractBindingTypes(property, out var bindingPropertyType, out var bindingVarType);

            if (bindingPropertyType == typeof(IProperty)) {
                bindingPropertyType = null; // optimize
            }

            var isList = TypeExtensions.IsGenericType(bindingPropertyType, typeof(IListProperty<>), out _);

            var result = new List<string>();
            var parent = transform;
            while (parent != null) {
                var target = parent.GetComponent<IBindingTarget>();
                if (target != null) {
                    var map = target.Editor_GetPropertiesFlatmap();
                    if (map.Count > 0) {
                        result.Add($"@{parent.name}");
                    }

                    foreach (var itr in map) {
                        if (bindingPropertyType == null) {
                            if (!TypeExtensions.InheritedOrInterfacedFromType(itr.Value.FieldType, typeof(IEditorNotAllowedDropdownAsIProperty))) {
                                result.Add(itr.Key);
                            }
                        }
                        else if (TypeExtensions.InheritedOrInterfacedFromType(itr.Value.FieldType, bindingPropertyType)) {
                            result.Add(itr.Key);
                        }
                        else if ((bindingVarType != null) && TypeExtensions.IsGenericType(itr.Value.FieldType, typeof(Property<>), out var genericPropertyType)) {
                            if (bindingVarType == genericPropertyType.GenericTypeArguments[0]) {
                                result.Add(itr.Key);
                            }
                        }
                        else if (isList && TypeExtensions.IsGenericType(itr.Value.FieldType, typeof(ListProperty<>), out _)) {
                            result.Add(itr.Key);
                        }
                    }

                    if (map.Count > 0) {
                        result.Add("_");
                    }
                }

                parent = parent.parent;
            }

            return result.ToArray();
        }
    }
}