using System.Collections;
using Binding;
using Core.Bindings.Tools.Extensions;
using TMPro;
using UnityEngine;

namespace Core.Bindings.Components {
    public class TextAlphaFadeBinding : BaseBinding<AlphaFadeProperty>
    {
        [SerializeField] private float FadeDelay;
        private TMP_Text _tmpText;
        private float _alpha;
        private WaitForSeconds _delay;
        
        protected override void Awake() {
            _tmpText = GetComponent<TMP_Text>();
            
            if (FadeDelay > 0)
                _delay = new WaitForSeconds(FadeDelay);
            
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
            if (_tmpText != null) {
                _tmpText.SetAlpha(alpha);
            }

            _alpha = alpha;
        }

        private IEnumerator FadeCoroutine()
        {
            _alpha = 1;
            SetAlpha(_alpha);
            yield return _delay;
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