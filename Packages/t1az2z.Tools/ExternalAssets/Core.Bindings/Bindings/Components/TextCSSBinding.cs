using Binding;
using Core.Bindings.Tools.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Bindings.Components {
    [RequireComponent(typeof(Text))]
    public class TextCSSBinding : BaseBinding<ColorStrStrProperty> {
        [SerializeField] private CSSComponentsEnum ElementType = default;

        private Text _text;

        protected override void Awake() {
            _text = GetComponent<Text>();
            base.Awake();
        }

        protected override void OnValueUpdated() {
            if (ElementType == CSSComponentsEnum.Color) {
                if (Property.Value != null) {
                    _text.SetColorOnly(Property.Value.Color);
                }
            }
            else if (ElementType == CSSComponentsEnum.Str1) {
                if (Property.Value != null) {
                    _text.text = Property.GetFormattedString(Property.Value.Str1);
                }
                else {
                    _text.text = null;
                }
            }
            else {
                if (Property.Value != null) {
                    _text.text = Property.GetFormattedString(Property.Value.Str2);
                }
                else {
                    _text.text = null;
                }
            }
        }

        private enum CSSComponentsEnum {
            Color,
            Str1,
            Str2
        }
    }
}