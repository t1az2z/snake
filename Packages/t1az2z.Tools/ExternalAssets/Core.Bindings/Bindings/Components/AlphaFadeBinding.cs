using System.Collections;
using Binding;
using Core.Bindings.Tools.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Bindings.Components {
    public class AlphaFadeBinding : BaseBinding<AlphaFadeProperty> {
        private CanvasGroup _canvasGroup;
        private Graphic _graphic;
        private float _alpha;

        protected override void Awake() {
            _canvasGroup = GetComponent<CanvasGroup>();
            _graphic = GetComponent<Graphic>();
            base.Awake();
        }

        protected override void OnDestroy() {
            base.OnDestroy();

            StopAllCoroutines();
        }

        protected override void OnPropertyValueUpdated() {
            if (this == null) {
                return;
            }

            StopAllCoroutines();

            if ((Property.FadeTime == 0f) || Property.Force) {
                SetAlpha(Property.Value);
            }
            else {
                StartCoroutine(FadeCoroutine());
            }
        }

        protected override void OnValueUpdated() {
            SetAlpha(Property.Value);
        }

        private void SetAlpha(float alpha) {
            if (_canvasGroup != null) {
                _canvasGroup.alpha = alpha;
            }

            if (_graphic != null) {
                _graphic.SetAlpha(alpha);
            }

            _alpha = alpha;
        }

        private IEnumerator FadeCoroutine() {
            var fade = _alpha > Property.Value;
            var targetAlpha = Property.Value;
            var delta = Mathf.Abs(targetAlpha - _alpha);
            while (delta > 0f) {
                var alpha = fade ? targetAlpha + delta : targetAlpha - delta;
                SetAlpha(alpha);

                if (Property.FadeTime == 0f) {
                    delta = 0f;
                }
                else {
                    delta -= Time.deltaTime * (1f / Property.FadeTime);
                }

                yield return null;
            }

            SetAlpha(targetAlpha);

            Property.Finish();
        }
    }
}