using System;
using Binding;
using TMPro;
using UnityEngine;

namespace Core.Bindings.Components {
    public class InputFieldSubmitBinding : BaseBinding<CommandProperty> {
        [SerializeField] private bool SubmitOnDeselect;
        
        private TMP_InputField _inputTMP;
        private string _lastSubmitValue;

        protected override void Awake() {
            _inputTMP = GetComponent<TMP_InputField>();
            _inputTMP?.onSelect.AddListener(InputOnSelectHandler);
            _inputTMP?.onSubmit.AddListener(InputOnSubmitHandler);
            if (SubmitOnDeselect)
                _inputTMP?.onDeselect.AddListener(InputOnSubmitHandler);
            base.Awake();
        }

        protected override void OnDestroy() {
            _inputTMP?.onSelect.RemoveListener(InputOnSelectHandler);
            _inputTMP?.onSubmit.RemoveListener(InputOnSubmitHandler);
            if (SubmitOnDeselect)
                _inputTMP?.onDeselect.RemoveListener(InputOnSubmitHandler);
            base.OnDestroy();
        }

        private void InputOnSelectHandler(string value) {
            _lastSubmitValue = null;
        }

        private void InputOnSubmitHandler(string value) {
            if (_lastSubmitValue == value)
                return;

            _lastSubmitValue = value;
            Property?.Invoke();
        }
    }
}