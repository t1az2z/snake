using Binding;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Bindings.Components {
    [RequireComponent(typeof(Slider))]
    public class SliderBinding : BaseBinding<FloatProperty> {
        [SerializeField, Range(0, 3f)] private float SmoothTime = 0f;

        private Slider _slider;
        private float _targetValue;
        private float _currentValue;
        private float _smoothVelocity;

        private bool _selfChanging;

        protected override void Awake() {
            _slider = GetComponent<Slider>();
            base.Awake();

            _currentValue = Property.Value;
            _targetValue = _currentValue;
            _slider.value = _currentValue;
            _slider.onValueChanged.AddListener(SliderOnValueChangedHandler);
        }

        protected override void OnDestroy() {
            _slider.onValueChanged.RemoveListener(SliderOnValueChangedHandler);
            base.OnDestroy();
        }

        private void SliderOnValueChangedHandler(float value) {
            if (_selfChanging) {
                return;
            }

            _currentValue = value;
            _targetValue = value;
            Property.Value = value;
        }

        protected override void OnValueUpdated() {
            _targetValue = Property.Value;
        }

        private void Update() {
            if (Mathf.Approximately(_currentValue, _targetValue)) {
                return;
            }

            _currentValue = Mathf.SmoothDamp(_currentValue, _targetValue, ref _smoothVelocity, SmoothTime);

            _selfChanging = true;
            _slider.value = _currentValue;
            _selfChanging = false;
        }
    }
}