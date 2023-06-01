using Binding;
using UnityEngine;

namespace Core.Bindings.Components {
    public class AnchoredAxisPositionBinding : BaseBinding<FloatProperty> {
        [SerializeField] private AxisEnum Axis;
        private RectTransform _rect;
        private Vector2 _anchorMinInitial;
        private Vector2 _anchorMaxInitial;
        private Vector2 _initialSizeDelta;

        protected override void Awake() {
            _rect = (RectTransform) transform;
            _anchorMinInitial = _rect.anchorMin;
            _anchorMaxInitial = _rect.anchorMax;
            _initialSizeDelta = _rect.sizeDelta;
            base.Awake();
        }

        protected override void OnValueUpdated() {
            _rect.anchorMin = new Vector2(Axis == AxisEnum.Horizontal ? Property.Value : _anchorMinInitial.x, Axis == AxisEnum.Vertical ? Property.Value : _anchorMinInitial.y);
            _rect.anchorMax = new Vector2(Axis == AxisEnum.Horizontal ? Property.Value : _anchorMaxInitial.x, Axis == AxisEnum.Vertical ? Property.Value : _anchorMaxInitial.y);
            _rect.sizeDelta = new Vector2(_initialSizeDelta.x, _initialSizeDelta.y);
        }
        
        private enum AxisEnum {
            Horizontal = 0,
            Vertical = 1,
        }
    }
}