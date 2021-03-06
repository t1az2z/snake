using Binding;
using Core.Bindings.Tools.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Bindings.Components {
    [RequireComponent(typeof(Text))]
    public class TextColorBinding : BaseBinding<ColorProperty> {
        private Text _text;

        protected override void Awake() {
            _text = GetComponent<Text>();
            base.Awake();
        }

        protected override void OnValueUpdated() {
            _text.SetColorOnly(Property.Value);
        }
    }
}