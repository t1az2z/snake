using Binding;
using DG.Tweening;
using UnityEngine;

namespace Core.Bindings.Components {
    public class ProgressBarBinding : BaseBinding<FloatProperty> {
        [SerializeField] private float SmoothTime;
        
        private RectTransform _rect;
        private Vector2 _anchorMinInitial;
        private Vector2 _anchorMaxInitial;
        private Vector2 _initialSizeDelta;
        private Tween _animationTween;

        protected override void Awake() {
            _rect = (RectTransform) transform;
            _anchorMinInitial = _rect.anchorMin;
            _anchorMaxInitial = _rect.anchorMax;
            _initialSizeDelta = _rect.sizeDelta;
            base.Awake();
        }

        protected override void OnValueUpdated() {
            _animationTween?.Kill();
            if (SmoothTime <= 0f) {
                SetProgress(Property.Value);
            }
            _animationTween = DOTween.To(
                getter: () => (_rect.anchorMax.x - _anchorMinInitial.x) / (_anchorMaxInitial.x - _anchorMinInitial.x),
                setter: SetProgress,
                endValue: Property.Value,
                duration: SmoothTime
            );
        }
        
        private void SetProgress(float value) {
            _rect.anchorMin = new Vector2(_anchorMinInitial.x, _rect.anchorMin.y);
            _rect.anchorMax = new Vector2(_anchorMinInitial.x + (_anchorMaxInitial.x - _anchorMinInitial.x) * value, _rect.anchorMax.y);
            _rect.sizeDelta = new Vector2(_initialSizeDelta.x, _rect.sizeDelta.y);
        }

        protected override void OnDestroy() {
            _animationTween?.Kill();
            base.OnDestroy();
        }
    }
}