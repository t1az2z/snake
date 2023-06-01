using Binding;
using UnityEngine;

namespace Core.Bindings.Components {
    [RequireComponent(typeof(Canvas))]
    public class CanvasSortingOrderBinding : BaseBinding<IntProperty> {
        private Canvas _canvas;

        protected override void Awake() {
            _canvas = GetComponent<Canvas>();
            base.Awake();
        }

        protected override void OnValueUpdated() {
            _canvas.sortingOrder = Property.Value;
        }
    }
}