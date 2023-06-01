using Binding;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Bindings.Components {
    public class FluidFloatBinding : BaseBinding<FloatProperty> {
        [SerializeField] private float LerpTime = 1f;
        [SerializeField] private string FormatString = "";

        private MaskableGraphic _graphic;
        private Text _text;
        private TextMeshProUGUI _tmpPro;
        private Outline _outlineEffect;
        private float _startValue;
        private float _currentValue;
        private float _endValue;
        private float _lastTimeStamp;
        private bool _isInitialValue;

        protected override void Awake() {
            FormatString = FormatString?.Trim();
            _text = GetComponent<Text>();
            _graphic = _text;
            _outlineEffect = GetComponent<Outline>();
            
            if (!_text) {
                _tmpPro = GetComponent<TextMeshProUGUI>();
                _graphic = _tmpPro;
            }

            base.Awake();
            
            _isInitialValue = true;
        }

        private void SetText(string text) {
            if (_text) {
                _text.text = text;
            }

            if (_tmpPro) {
                _tmpPro.text = text;
            }
        }

        private void Update() {
            if (_currentValue != _endValue) {
                var t = LerpTime > 0.01f ? Mathf.Clamp01((Time.unscaledTime - _lastTimeStamp) / LerpTime) : 1f;
                _currentValue = Mathf.Lerp(_startValue, _endValue, Mathf.Sqrt(t));
                SetText(_currentValue.ToString("0.0"));

                if (_outlineEffect != null) {
                    var color = _graphic.color;
                    color.a = 1f - t;
                    _outlineEffect.effectColor = color;
                }
            }
        }

        protected override void OnValueUpdated() {
            if (_isInitialValue) {
                _isInitialValue = false;
                _startValue = _endValue;
                _currentValue = _endValue;
                _endValue = Property.Value;
            }
            else {
                _endValue = Property.Value;
                _currentValue = _endValue;
                _startValue = _endValue;

                SetText(string.Format(string.IsNullOrEmpty(FormatString) ? "0.0" : FormatString, _endValue));
                if (_outlineEffect != null) {
                    _outlineEffect.effectColor = new Color(0f, 0f, 0f, 0f);
                }
            }

            _lastTimeStamp = Time.unscaledTime;
        }
    }
}