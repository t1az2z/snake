using Binding;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Bindings.Components {
    [RequireComponent(typeof(Scrollbar))]
    public class ScrollBarBinding : BaseBinding<FloatProperty> {
        private Scrollbar _scrollBar;

        protected override void Awake() {
            _scrollBar = GetComponent<Scrollbar>();
            _scrollBar.onValueChanged.AddListener(ScrollValueChangeHandler);
            base.Awake();
        }

        protected override void OnValueUpdated() {
            _scrollBar.value = Property.Value;
        }

        private void ScrollValueChangeHandler(float value) {
            Property.Value = value;
        }
    }
}