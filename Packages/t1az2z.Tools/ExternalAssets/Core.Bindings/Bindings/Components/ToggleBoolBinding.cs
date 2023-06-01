using Binding;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Bindings.Components {
    [RequireComponent(typeof(Toggle))]
    public class ToggleBoolBinding : BaseBinding<BoolProperty> {
        [SerializeField] private bool ChangePropertyValue = true;
        [SerializeField] private bool Invert = false;

        private Toggle _toogle;

        protected override void Awake() {
            _toogle = GetComponent<Toggle>();
            _toogle.onValueChanged.AddListener(ToggleValueChangeHandler);
            base.Awake();
        }

        protected override void OnValueUpdated() {
            _toogle.isOn = Invert ? !Property.Value : Property.Value;
        }

        private void ToggleValueChangeHandler(bool value) {
            if (ChangePropertyValue) {
                Property.Value = value;
            }
        }
    }
}