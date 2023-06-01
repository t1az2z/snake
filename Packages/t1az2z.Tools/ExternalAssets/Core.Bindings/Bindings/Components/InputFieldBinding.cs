using Binding;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Bindings.Components {
    public class InputFieldBinding : BaseBinding<StringProperty> {
        [SerializeField] private bool ChangePropertyValue = true;

        private InputField _inputField;
        private TMP_InputField _inputTMP;

        protected override void Awake() {
            _inputField = GetComponent<InputField>();
            _inputField?.onValueChanged.AddListener(InputValueChangedHandler);

            _inputTMP = GetComponent<TMP_InputField>();
            _inputTMP?.onValueChanged.AddListener(InputValueChangedHandler);

            base.Awake();
        }

        protected override void OnDestroy() {
            _inputField?.onValueChanged.RemoveListener(InputValueChangedHandler);
            _inputTMP?.onValueChanged.RemoveListener(InputValueChangedHandler);
            base.OnDestroy();
        }

        protected override void OnValueUpdated() {
            if (_inputField) {
                _inputField.text = Property.ToString();
            }

            if (_inputTMP) {
                _inputTMP.text = Property.ToString();
            }
        }

        private void InputValueChangedHandler(string value) {
            if (ChangePropertyValue) {
                Property.Value = value;
            }
        }
    }
}