using System;
using Binding;
using UnityEngine;

namespace Core.Bindings.Components {
    public class PositionBinding : BaseBinding<Vector2Property> {
        private RectTransform _rectTransform;
        private bool _changing;
        
        [SerializeField] private ControlMode Mode;

        protected override void Awake() {
            _changing = false;
            _rectTransform = transform as RectTransform;
            base.Awake();
            OnRectTransformDimensionsChange();
        }
        
        private void OnRectTransformDimensionsChange() {
            if (_changing || (Property == null)) {
                return;
            }

            Property.Value = Mode switch {
                ControlMode.AnchoredPosition => _rectTransform.anchoredPosition,
                ControlMode.WorldPosition => _rectTransform.position,
                _ => Property.Value
            };
        }

        protected override void OnValueUpdated() {
            _changing = true;
            switch (Mode) {
                case ControlMode.AnchoredPosition:
                    _rectTransform.anchoredPosition = Property.Value;
                    break;
                case ControlMode.WorldPosition:
                    _rectTransform.position = Property.Value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            _changing = false;
        }

        private enum ControlMode {
            AnchoredPosition = 0,
            WorldPosition = 1
        }
    }
}