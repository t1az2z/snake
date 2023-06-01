using Binding;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Bindings.Components
{
    public class FluidIntBinding : BaseBinding<IntProperty>
    {
        [SerializeField] protected float LerpTime = 2f;
        [SerializeField] private string FormatString = "";
        
        private MaskableGraphic _graphic;
        private Text _text;
        private TextMeshProUGUI _tmpPro;
        private Outline _outlineEffect;
        private int _startValue;
        private int _currentValue;
        private int _endValue;
        private float _lastTimeStamp;
        private bool _isInitialValue;
        private float _currentDelay;

        protected override void Awake()
        {
            _text = GetComponent<Text>();
            _graphic = _text;
            if (!_text)
            {
                _tmpPro = GetComponent<TextMeshProUGUI>();
                _graphic = _tmpPro;
            }

            FormatString = FormatString?.Trim();
            _outlineEffect = GetComponent<Outline>();

            base.Awake();

            _isInitialValue = true;
        }

        private void SetText(string text)
        {
            if (_text)
            {
                _text.text = text;
            }

            if (_tmpPro)
            {
                _tmpPro.text = text;
            }
        }

        private void Update()
        {
            if (_currentValue != _endValue)
            {
                var t = LerpTime > 0.01f ? Mathf.Clamp01((Time.unscaledTime - _lastTimeStamp) / LerpTime) : 1f;
                _currentValue = Mathf.RoundToInt(Mathf.Lerp(_startValue, _endValue, Mathf.Sqrt(t)));

                if (_outlineEffect != null)
                {
                    var color = _graphic.color;
                    color.a = 1f - t;
                    _outlineEffect.effectColor = color;
                }

                SetText(Format(_currentValue));
            }
        }

        protected override void OnValueUpdated()
        {
            if (_isInitialValue)
            {
                _isInitialValue = false;
                _endValue = Property.Value;
                _currentValue = _endValue;
                _startValue = _endValue;

                if (_outlineEffect != null)
                {
                    _outlineEffect.effectColor = new Color(0f, 0f, 0f, 0f);
                }

                SetText(Format(_endValue));
            }
            else
            {
                _startValue = _currentValue;
                _endValue = Property.Value;

                SetText(Format(_currentValue));
            }

            _lastTimeStamp = Time.unscaledTime;
        }

        private string Format(int value)
        {
            return !string.IsNullOrEmpty(FormatString) ? string.Format(FormatString, value) : value.ToString();
        }
    }
}