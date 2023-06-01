using Binding;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Bindings.Components {
    public class ButtonCommandIntBinding : BaseBinding<CommandIntProperty> {
        [SerializeField] private int Value;
        
        private Button _button;

        protected override void Awake() {
            _button = GetComponent<Button>();
            base.Awake();
        }

        private void Invoke() => Property?.Invoke(Value);

        protected override void Bind(bool total = false) {
            base.Bind(total);
            if (_button)
                _button.onClick.AddListener(Invoke);
        }

        protected override void Unbind(bool total = false) {
            base.Unbind(total);
            if (_button)
                _button.onClick.RemoveListener(Invoke);
        }
    }
}