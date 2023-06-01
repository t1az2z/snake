using System.Collections;
using Core.Core.Bindings.Bindings.Properties;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Bindings.Components {
    public class ColorLerpBinding : BaseBinding<ColorLerpProperty> {
        private Graphic _graphic;
        private Color _color;
        private bool _isInitialized;

        protected override void Awake() {
            _graphic = GetComponent<Graphic>();
            base.Awake();
        }

        protected override void OnDestroy() {
            base.OnDestroy();

            StopAllCoroutines();
        }

        protected override void OnValueUpdated() {
            StopAllCoroutines();
            if (!_isInitialized) {
                _isInitialized = true;
                SetColor(Property.Value);
                return;
            }
            
            
            if ((Property.LerpTime <= 0f) || Property.Force) {
                SetColor(Property.Value);
            }
            else {
                StartCoroutine(LerpCoroutine());
            }
        }

        private void SetColor(Color color) {
            if (_graphic != null) {
                _graphic.color = color;
            }

            _color = color;
        }

        private IEnumerator LerpCoroutine() {
            var startColor = _color;
            var targetColor = Property.Value;
            var lerpTime = Property.LerpTime;
            var delta = lerpTime;
            while (delta > 0f) {
                SetColor(Color.Lerp(startColor, targetColor, (lerpTime - delta) / lerpTime));
                delta -= Time.deltaTime;
                yield return null;
            }

            SetColor(targetColor);
        }
    }
}