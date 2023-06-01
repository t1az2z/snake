using Binding;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Bindings.Components {
    [RequireComponent(typeof(ScrollRect))]
    public class SetActiveScrollRectBinding : BaseBinding<BoolProperty> {
        private ScrollRect _scrollRect;

        private bool _isHorizontal;
        private bool _isVertical;

        protected override void Awake() {
            _scrollRect = GetComponent<ScrollRect>();
            _isHorizontal = _scrollRect.horizontal;
            _isVertical = _scrollRect.vertical;
            base.Awake();
        }

        protected override void OnValueUpdated() {
            _scrollRect.horizontal = Property.Value && _isHorizontal;
            _scrollRect.vertical = Property.Value && _isVertical;
        }
    }
}